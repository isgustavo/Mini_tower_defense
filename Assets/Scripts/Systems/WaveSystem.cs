using System;
using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    [UpdateAfter(typeof(DamageSystem))]
    public class WaveSystem : ComponentSystem
    {
        private struct SmallObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public ComponentArray<HealthComponent> Health;
            public ComponentDataArray<SmallEnemyComponent> Enemy;
            public ComponentDataArray<IdleComponent> Idle;
        }

        [Inject] private SmallObjectData smallObjData;

        private struct BigObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public ComponentArray<HealthComponent> Health;
            public ComponentDataArray<BigEnemyComponent> Enemy;
            public ComponentDataArray<IdleComponent> Idle;
        }

        [Inject] private BigObjectData bigObjData;

        private struct WaveData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<WaveComponent> Wave;
        }

        [Inject] private WaveData waveData;

        private struct WaveUIData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<UIWaveComponent> Wave;
        }

        [Inject] private WaveUIData uiData;

        protected override void OnUpdate()
        {

            for(int i = 0; i < waveData.Length; i++) 
            {
                var wave = waveData.Wave[i];

                if(wave.CurrentWave > wave.Count)
                {
                    continue; 
                }

                if (wave.CurrentWave == 0)
                {
                    wave.CurrentWave = 1;
                    wave.CurrentTime = wave.TimeStartFirstWave; 
                }

                if (wave.CurrentTime <= 0)
                {

                    int[] waveConfig = GetWaveConfig(wave.CurrentWave, wave.wavesConfig);

                    int spawnPoint = 0;
                    while (spawnPoint == wave.lastSpawnIndex) 
                    {
                        spawnPoint = UnityEngine.Random.Range(0, wave.waveSpawn.Length);
                    }

                    wave.lastSpawnIndex = spawnPoint;

                    NewWave(waveConfig,
                                    wave.waveSpawn[spawnPoint],
                                        wave.enemyPrefab);

                    uiData.Wave[0].UIWave.text = string.Format("Wave: {0}", wave.CurrentWave.ToString("00"));
                    wave.CurrentWave += 1;
                    wave.CurrentTime = wave.TimeBetweenWaves;
                }

                wave.CurrentTime -= Time.deltaTime;

            }
        }

        private int[] GetWaveConfig(int currentWave, string waveConfig)
        {
            string[] waves = waveConfig.Split(',');
            string[] w = waves[currentWave - 1].Split('|');
            return new int[] { int.Parse(w[0]), int.Parse(w[1])};
        }

        private void NewWave(int[] wave, Transform startPoint, GameObject[] objPrefab)
        {
            var lastPosition = Vector3.zero;
            lastPosition = RespawnSmallObj(startPoint, lastPosition);
            lastPosition = SpawnIfNecessary(wave[0] - smallObjData.Length, startPoint, lastPosition, objPrefab[0]);
            lastPosition = RespawnBigObj(startPoint, lastPosition);
            SpawnIfNecessary(wave[1] - bigObjData.Length, startPoint, lastPosition, objPrefab[1]);

        }

        private Vector3 SpawnIfNecessary(int poolCount, Transform startPoint, Vector3 lastPosition, GameObject objPrefab) 
        {
            for (int i = 0; i < poolCount; i++) 
            {
                lastPosition += startPoint.position.x > 0 ? (Vector3.right * 2) : -(Vector3.right * 2);
                UnityEngine.Object.Instantiate(objPrefab, startPoint.position + lastPosition, startPoint.rotation);
            }

            return lastPosition;
        }

        private Vector3 RespawnSmallObj(Transform startPoint, Vector3 lastPosition)
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < smallObjData.Length; i++)
            {
                lastPosition += startPoint.position.x > 0 ? (Vector3.right * 2) : -(Vector3.right * 2);
                smallObjData.Transform[i].position = startPoint.position + lastPosition;
                smallObjData.Transform[i].rotation = startPoint.rotation;

                smallObjData.Health[i].currentHealth = smallObjData.Health[i].health;
                smallObjData.Health[i].healthBar.fillAmount = smallObjData.Health[i].currentHealth / smallObjData.Health[i].health;

                puc.RemoveComponent<IdleComponent>(smallObjData.Entity[i]);
            }

            return lastPosition;
        }

        private Vector3 RespawnBigObj(Transform startPoint, Vector3 lastPosition)
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < bigObjData.Length; i++)
            {
                lastPosition += startPoint.position.x > 0 ? (Vector3.right * 2) : -(Vector3.right * 2);
                bigObjData.Transform[i].position = startPoint.position + lastPosition;
                bigObjData.Transform[i].rotation = startPoint.rotation;

                bigObjData.Health[i].currentHealth = bigObjData.Health[i].health;
                bigObjData.Health[i].healthBar.fillAmount = bigObjData.Health[i].currentHealth / bigObjData.Health[i].health;

                puc.RemoveComponent<IdleComponent>(bigObjData.Entity[i]);
            }

            return lastPosition;
        }
    }
}