using ODT.Component;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(CannonComponent))]
public class DamageSystem : ComponentSystem
{
    private struct ObjectData
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentArray<Transform> Transform;
        public ComponentArray<HealthComponent> Health;
        public ComponentDataArray<DamageComponent> Damage;
        public SubtractiveComponent<IdleComponent> Idle;
    }

    [Inject] private ObjectData data;

    protected override void OnUpdate()
    {
        var puc = PostUpdateCommands;

        for (int i = 0; i < data.Length; i++) 
        {
            data.Health[i].currentHealth -= data.Damage[i].damange;

            if(data.Health[i].healthBar != null)
            {
                data.Health[i].healthBar.fillAmount = data.Health[i].currentHealth / data.Health[i].health;
            }

            puc.RemoveComponent<DamageComponent>(data.Entity[i]);

            if (data.Health[i].currentHealth <= 0) 
            {
                puc.AddComponent(data.Entity[i], new IdleComponent());
                data.Transform[i].position = new Vector3(0, -2, 2);
            }
        }
    }
}