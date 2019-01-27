using ODT.Component;
using Unity.Entities;
using UnityEngine;

public class BulletSystem : ComponentSystem
{
    private readonly int ENVIROMENT_LAYER_MASK = 1 << 16;
    private readonly int ENEMY_LAYER_MASK = 1 << 10;

    private struct ObjectData
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentArray<Transform> Transform;
        public ComponentDataArray<BulletComponent> Bullet;
        public SubtractiveComponent<IdleComponent> Idle;
    }

    [Inject] private ObjectData data;

    protected override void OnUpdate()
    {
        var puc = PostUpdateCommands;

        for (int i = 0; i < data.Length; i++)
        {
            Debug.DrawRay(data.Transform[i].position, -Vector3.up * .2f);
            if (Physics.Raycast(data.Transform[i].position, -Vector3.up, out RaycastHit hit, .2f, ENVIROMENT_LAYER_MASK))
            {
                data.Transform[i].position = Vector3.zero;
                puc.AddComponent(data.Entity[i], new IdleComponent());
            }
            else if (Physics.Raycast(data.Transform[i].position, -Vector3.up, out hit, .2f, ENEMY_LAYER_MASK))
            {
                data.Transform[i].position = Vector3.zero;
                puc.AddComponent(data.Entity[i], new IdleComponent());

                var entity = hit.transform.GetComponent<GameObjectEntity>();
                puc.AddComponent(entity.Entity, new DamageComponent { damange = data.Bullet[i].damage });
            }
            else
            {
                data.Transform[i].Translate(Vector3.right * data.Bullet[i].speed * Time.deltaTime);
            }
        }
    }
}
