using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class JumpForwardSystem : ComponentSystem
    {
        private readonly Vector3 JUMP_TO_RIGHT = new Vector3(.3f, 1f, 0f);
        private readonly Vector3 JUMP_TO_LEFT = new Vector3(-.3f, 1f, 0f);

        private struct ObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public ComponentDataArray<JumpComponent> Jump;
        }

        [Inject] private ObjectData data;

        protected override void OnUpdate()
        {
            var puc = PostUpdateCommands;
            for (int i = 0; i < data.Length; i++)
            {
                if(data.Transform[i].position.x > 0)
                {
                    data.Transform[i].position += data.Transform[i].forward + JUMP_TO_RIGHT;
                } else 
                {
                    data.Transform[i].position += data.Transform[i].forward + JUMP_TO_LEFT;
                }

                puc.RemoveComponent<JumpComponent>(data.Entity[i]);
            }
        }
    }
}
