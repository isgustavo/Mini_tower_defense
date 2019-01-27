using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class RemoveBuildSystem : ComponentSystem
    {
        private struct RemoveData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<RemoveBuildComponent> Build;
            public ComponentDataArray<YesComponent> Yes;
            public ComponentArray<Transform> Transform;
        }

        [Inject] private RemoveData removeData;

        protected override void OnUpdate()
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < removeData.Length; i++)
            {
                Object.Destroy(removeData.Transform[i].gameObject);
            }
        }
    }
}