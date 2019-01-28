using UnityEngine;

namespace ODT.Component
{
    public class WaveComponent : MonoBehaviour 
    {
        public int Count;
        public int Multiplay;
        [HideInInspector]
        public int CurrentWave;
        public float TimeStartFirstWave;
        public float TimeBetweenWaves;
        [HideInInspector]
        public float CurrentTime;

        public GameObject[] enemyPrefab;
        public Transform[] waveSpawn;
    }
}