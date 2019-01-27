using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    [UpdateAfter(typeof(MoveForwardSystem))]
    [UpdateAfter(typeof(JumpComponent))]
    public class GravitySystem : ComponentSystem
    {
        private struct ObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public SubtractiveComponent<JumpComponent> Jump;
            public SubtractiveComponent<BlockComponent> Block;
            public SubtractiveComponent<StaticComponent> Static;
            public SubtractiveComponent<BulletComponent> Bullet;
            public SubtractiveComponent<IdleComponent> Idle;
        }

        [Inject] private ObjectData data;

        protected override void OnUpdate()
        {
            var puc = PostUpdateCommands;
            for (int i = 0; i < data.Length; i++)
            {
                if (!Physics.Raycast(data.Transform[i].position + new Vector3(0.2f, 0f, 0f), Vector3.down, out RaycastHit hit, .6f))
                {
                    data.Transform[i].position = new Vector3(data.Transform[i].position.x + .5f, data.Transform[i].position.y - 1, 0f);
                }
            }
        }
    }
}
