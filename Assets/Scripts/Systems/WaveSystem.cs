using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class WaveSystem : ComponentSystem
    {
        private struct ObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public ComponentDataArray<IdleComponent> Idle;
        }

        [Inject] private ObjectData idleData;

        private struct WaveData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<WaveComponent> Wave;
        }

        [Inject] private WaveData waveData;

        protected override void OnUpdate()
        {

            for(int i = 0; i < waveData.Length; i++) 
            {
                var wave = waveData.Wave[i];

                if(wave.CurrentWave >= wave.Count)
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
                    NewWave(wave.CurrentWave, 
                                wave.Multiplay, 
                                    wave.waveSpawn[Random.Range(0, wave.waveSpawn.Length - 1)],
                                        wave.enemyPrefab);

                    wave.CurrentWave += 1;
                    wave.CurrentTime = wave.TimeBetweenWaves;
                }

                wave.CurrentTime -= Time.deltaTime;

            }
        }

        private void NewWave(int wave, int multiplay, Transform startPoint, GameObject[] objPrefab)
        {
            int total = wave * multiplay;
            int poolCount = total - idleData.Length;

            SpawnIfNecessary(poolCount, startPoint, objPrefab);
            Respawn(startPoint);
        }

        private void SpawnIfNecessary(int poolCount, Transform startPoint, GameObject[] objPrefab) 
        {
            for (int i = 0; i < poolCount; i++) 
            {
                Object.Instantiate(objPrefab[Random.Range(0, objPrefab.Length - 1)],
                                                            startPoint.position - (Vector3.right * i * 2), startPoint.rotation);
            }
        }

        private void Respawn(Transform startPoint)
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < idleData.Length; i++) 
            {
                idleData.Transform[i].position = startPoint.position;
                idleData.Transform[i].rotation = startPoint.rotation;

                puc.RemoveComponent<IdleComponent>(idleData.Entity[i]);
            }
        }
    }
}