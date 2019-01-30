using UnityEngine;

namespace ODT.Component
{
    public class WaveComponent : MonoBehaviour 
    {
        public int Count;
        [Header("Set config (small enemies | big enemies ,)"), Space(5), Header("Wave Config Mask: #N|N,N|N... ")]
        public string wavesConfig;
        [HideInInspector]
        public int CurrentWave;
        public float TimeStartFirstWave;
        public float TimeBetweenWaves;
        [HideInInspector]
        public float CurrentTime;

        public GameObject[] enemyPrefab;
        [HideInInspector]
        public int lastSpawnIndex;
        public Transform[] waveSpawn;
    }
}