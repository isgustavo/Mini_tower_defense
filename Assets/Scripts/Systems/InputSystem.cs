using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class InputSystem : ComponentSystem
    {
        private readonly int SLOT_LAYER = 12;
        private readonly int BUILD_LAYER = 15;

        private struct UIData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<UIPopUpComponent> UI;
        }
        [Inject] private UIData uiData;

        private struct UIPopUp
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<UIActiveComponent> UI;
        }
        [Inject] private UIPopUp popUpData;

        protected override void OnUpdate()
        {

            if (popUpData.Length > 0) 
            {
                return;
            }

            var puc = PostUpdateCommands;

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (uiData.Length != 1)
                {
                    Debug.LogError("UIPopUpComponent not found!");
                    return;
                }

                if (Physics.Raycast(ray, out hit, 100, 1 << BUILD_LAYER))
                {
                    var entity = hit.transform.GetComponent<GameObjectEntity>();
                    puc.AddComponent(entity.Entity, new ClickedComponent());
                    puc.AddComponent(entity.Entity, new RemoveBuildComponent());

                    uiData.UI[0].UITitle.text = string.Format("Remove Build?");
                    puc.AddComponent(uiData.Entity[0], new UIActiveComponent());

                } else if (Physics.Raycast(ray, out hit, 100, 1 << SLOT_LAYER))
                {
                    var build = hit.transform.GetComponent<BuilderComponent>();

                    if (build == null)
                    {
                        Debug.LogError("BuildComponent not found!");
                        return;
                    }

                    uiData.UI[0].UITitle.text = string.Format("Build a {0}?", build.buildName);
                    puc.AddComponent(uiData.Entity[0], new UIActiveComponent());

                    var entity = hit.transform.GetComponent<GameObjectEntity>();
                    puc.AddComponent(entity.Entity, new ClickedComponent());
                } 
            }
          
        }
    }
}

