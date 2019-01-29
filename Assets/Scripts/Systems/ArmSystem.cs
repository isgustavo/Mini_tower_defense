using Unity.Collections;
using ODT.Component;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace ODT.System
{
    [UpdateAfter(typeof(DamageSystem))]
    public class ArmSystem : JobComponentSystem
    {
        private struct ArmData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public ComponentArray<ArmComponent> Arm;
        }

        [Inject] private ArmData armData;

        private struct ObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public ComponentArray<HealthComponent> Health;
            public SubtractiveComponent<IdleComponent> Idle;
        }

        [Inject] private ObjectData objData;

        private struct ClosestObjectJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<Vector3> objArray;
            [ReadOnly] public Vector3 Arm;

            public NativeArray<float> distanceArray;

            public void Execute(int index)
            {
                distanceArray[index] = Vector3.Distance(objArray[index], Arm);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (objData.Length <= 0)
            {
                return inputDeps;
            }

            for (int i = 0; i < armData.Length; i++)
            {
              
               SetTargetTo(i, inputDeps);

                Vector3 difference = armData.Arm[i].target.position - armData.Arm[i].transform.position;
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                armData.Arm[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            }

            return inputDeps;
        }

        private void SetTargetTo(int armIndex, JobHandle inputDeps) {

            NativeArray<Vector3> objPositionArray = new NativeArray<Vector3>(objData.Length, Allocator.TempJob);
            NativeArray<float> objDistanceArray = new NativeArray<float>(objData.Length, Allocator.TempJob);

            for (int i = 0; i < objData.Length; i++)
            {
                objPositionArray[i] = objData.Transform[i].position;
            }

            var job = new ClosestObjectJob
            {
                objArray = objPositionArray,
                Arm = armData.Transform[0].position,
                distanceArray = objDistanceArray
            };

            var jobHandle = job.Schedule(objData.Length, 2, inputDeps);

            jobHandle.Complete();

            objPositionArray.Dispose();

            var closestObj = 999f;
            var index = 0;
            for (int i = 0; i < objDistanceArray.Length; i++)
            {
                if (closestObj > objDistanceArray[i])
                {
                    closestObj = objDistanceArray[i];
                    index = i;
                }
            }

            armData.Arm[armIndex].target = objData.Transform[index];

            objDistanceArray.Dispose();
        }
    }
}