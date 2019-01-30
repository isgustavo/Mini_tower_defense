using System;
using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    [UpdateAfter(typeof(BulletSystem))]
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
            var bulletIndex = 0;
            for (int i = 0; i < cannonData.Length; i++) 
            {
                var cannon = cannonData.Cannon[i];
                if (cannon.lastShootTime <= 0)
                {
                    if(bulletData.Length > 0 && bulletIndex < bulletData.Length)
                    {
                       ReSpawnBullet(bulletIndex, cannon);
                    } else
                    {
                        SpawnBullet(cannon);
                    }
                    cannon.lastShootTime = cannon.timeBetweenShoot;
                }
                cannon.lastShootTime -= Time.deltaTime;
            }
        }

        private int ReSpawnBullet(int index, CannonComponent cannon)
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < cannon.cannonTransform.Length; i++) 
            {
                if(index < bulletData.Length)
                {
                    puc.RemoveComponent<IdleComponent>(bulletData.Entity[index]);
                    bulletData.Transform[index].position = cannon.cannonTransform[index].position;
                    bulletData.Transform[index].rotation = cannon.cannonTransform[index].rotation;
                    index += 1;
                }
            }

            return index;
        }

        private void SpawnBullet(CannonComponent cannon) 
        {
            for (int i = 0; i < cannon.cannonTransform.Length; i++)
            {
                UnityEngine.Object.Instantiate(cannon.bulletPrefab, 
                                        cannon.cannonTransform[i].position,
                                            cannon.cannonTransform[i].rotation);
            }
        }
    }
}
