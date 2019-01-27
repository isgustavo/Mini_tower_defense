using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class InputSystem : ComponentSystem
    {
        private struct ObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<UIConfirmComponent> UI;
        }

        [Inject] private ObjectData data;

        protected override void OnUpdate()
        {
            var puc = PostUpdateCommands;

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100, 1 << 12))
                {
                    var builder = hit.transform.GetComponent<BuilderComponent>();

                    if(builder == null)
                    {
                        Debug.LogError("BuilderComponent not found!");
                        return;
                    }

                    if (data.Length != 1)
                    {
                        Debug.LogError("UIConfirmComponent not found!");
                        return;
                    }

                    var entity = hit.transform.GetComponent<GameObjectEntity>();
                    puc.AddComponent(entity.Entity, new SlotClickedComponent());

                    data.UI[0].UITitle.text = string.Format(data.UI[0].titlePrefix, builder.buildName);
                    data.UI[0].UIContainer.SetActive(true);
                } 
            }
          
        }
    }
}

