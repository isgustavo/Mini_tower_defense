using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class ConfirmBuildSystem : ComponentSystem
    {

        private struct BuildData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<BuilderComponent> Build;
            public ComponentDataArray<ClickedComponent> Click;
            public ComponentDataArray<YesComponent> Slot;
        }

        [Inject] private BuildData buildData;

        protected override void OnUpdate()
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < buildData.Length; i++)
            {
                puc.RemoveComponent<ClickedComponent>(buildData.Entity[i]);
                puc.RemoveComponent<YesComponent>(buildData.Entity[i]);
                Object.Instantiate(buildData.Build[i].ObjPrefab, buildData.Build[i].SpawnObjPoint.position, Quaternion.identity);
            }
        }
    }
}