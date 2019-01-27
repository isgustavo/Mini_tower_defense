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
            public SubtractiveComponent<JumpComponent> Jump;
            public SubtractiveComponent<BlockComponent> Block;
        }

        [Inject] private ObjectData data;

        protected override void OnUpdate()
        {
            var puc = PostUpdateCommands;
            var barrierLayerMask = 1 << 9;
            var enemyLayerMask = 1 << 10;

            for (int i = 0; i < data.Length; i++) 
            {
                if (Physics.Raycast(data.Transform[i].position, Vector3.right, out RaycastHit hit, .5f, barrierLayerMask))
                {
                    puc.AddComponent(data.Entity[i], new BlockComponent());
                } else if (Physics.Raycast(data.Transform[i].position, Vector3.right, out hit, .5f, enemyLayerMask))
                {
                    if (Physics.Raycast(hit.transform.position, Vector3.up, out hit, 1, enemyLayerMask))
                    {
                        puc.AddComponent(data.Entity[i], new BlockComponent());
                    } else
                    {
                        puc.AddComponent(data.Entity[i], new JumpComponent());
                    }
                } else
                {
                    data.Transform[i].Translate(Vector3.right * Time.deltaTime);
                }
            }
        }
    }
}
