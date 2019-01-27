using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class JumpForwardSystem : ComponentSystem
    {
        private readonly Vector3 JUMP = new Vector3(.3f, 1f, 0f);

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
                data.Transform[i].position += JUMP;
                puc.RemoveComponent<JumpComponent>(data.Entity[i]);
            }
        }
    }
}
