using ODT.Component;
using Unity.Entities;
using UnityEngine;

public class WinGameSystem : ComponentSystem
{
    private struct WaveData
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentArray<WaveComponent> Wave;
    }

    [Inject] private WaveData waveData;

    private struct MoveObjectData
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentArray<HealthComponent> Health;
        public SubtractiveComponent<IdleComponent> Idle;
    }

    [Inject] private MoveObjectData moveObjData;

    private struct UIEndGameData
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentArray<UIEndGameComponent> UI;
    }

    [Inject] private UIEndGameData UIdata;

    protected override void OnUpdate()
    {
     
        if(waveData.Length > 0)
        {
            var wave = waveData.Wave[0];

            if (wave.CurrentWave > wave.Count)
            {
                if(moveObjData.Length == 0)
                {
                    UIdata.UI[0].UITitle.text = "U WON!";
                    UIdata.UI[0].UIContainer.SetActive(true);
                }
            }
        }

    }
}