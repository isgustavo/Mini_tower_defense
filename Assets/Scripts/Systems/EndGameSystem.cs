using ODT.Component;
using Unity.Entities;
using UnityEngine;

public class EndGameSystem : ComponentSystem
{
    private readonly int TARGET_LAYER_MASK = 1 << 17;

    private struct ObjectData
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentArray<Transform> Transform;
        public ComponentArray<HealthComponent> Health;
        public SubtractiveComponent<BlockedComponent> Block;
    }

    [Inject] private ObjectData data;

    private struct UIEndGameData
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentArray<UIEndGameComponent> UI;
    }

    [Inject] private UIEndGameData UIdata;

    protected override void OnUpdate()
    {
        var puc = PostUpdateCommands;

        for(int i = 0; i < data.Length; i++)
        {
            if (Physics.Raycast(data.Transform[i].position, data.Transform[i].forward, out RaycastHit hit, .5f, TARGET_LAYER_MASK))
            {
                puc.AddComponent(data.Entity[i], new BlockedComponent());
                if(UIdata.Length > 0)
                {
                    UIdata.UI[0].UITitle.text = "U LOST!";
                    UIdata.UI[0].UIContainer.SetActive(true);
                } else
                {
                    Debug.LogError("UIEndGameComponent not found!"); 
                }
            }
        }
    }
}