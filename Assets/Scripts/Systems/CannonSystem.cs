using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class CannonSystem : ComponentSystem
    {
        private struct CannonData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<CannonComponent> Cannon;
        }

        [Inject] private CannonData cannonData;

        private struct BulletData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public ComponentDataArray<BulletComponent> Bullet;
            public ComponentDataArray<IdleComponent> Idle;
        }

        [Inject] private BulletData bulletData;

        protected override void OnUpdate()
        {
            for (int i = 0; i < cannonData.Length; i++) 
            {
                var cannon = cannonData.Cannon[i];
                if (cannon.lastShootTime <= 0 && bulletData.Length > 0)
                {
                    ReSpawnBullet(cannon);
                    cannon.lastShootTime = cannon.timeBetweenShoot;
                }

                cannon.lastShootTime -= Time.deltaTime;
            }
        }

        private void ReSpawnBullet(CannonComponent cannon)
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < cannon.cannonTransform.Length; i++) 
            {
                puc.RemoveComponent<IdleComponent>(bulletData.Entity[i]);
                bulletData.Transform[i].position = cannon.cannonTransform[i].position;
                bulletData.Transform[i].rotation = cannon.cannonTransform[i].rotation;
            }
        }
    }
}
