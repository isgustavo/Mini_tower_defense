using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{

    [Header ("Player")]
    [SerializeField]
    private GameObject machineGunPrefab;
    [SerializeField]
    private GameObject barrierPrefab;

    [Header("Waves")]
    [SerializeField]
    private GameObject[] enemiesPrefab;
    [SerializeField]
    private string wavesCount;
    [SerializeField]
    private int wavesEnemiesMultiple;
    [SerializeField]
    private float timeBetweenWaves;


    public void OnCreateMachineGun() 
    {
     
    }
}
