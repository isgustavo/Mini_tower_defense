using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class MoveForwardSystem : ComponentSystem
    {
        private struct ObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public ComponentDataArray<MoveSpeed> Speed;
            public SubtractiveComponent<JumpComponent> Jump;
            public SubtractiveComponent<BlockedComponent> Block;
            public SubtractiveComponent<StaticComponent> Static;
            public SubtractiveComponent<IdleComponent> Idle;
        }

        [Inject] private ObjectData data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data.Transform[i].Translate(Vector3.forward * data.Speed[i].Value * Time.deltaTime);
            }
        }

    }
}
