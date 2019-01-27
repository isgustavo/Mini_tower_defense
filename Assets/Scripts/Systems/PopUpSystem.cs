using System.Collections.Generic;
using ODT.Component;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ODT.System
{
    public class PopUpSystem : ComponentSystem
    {
        private readonly int YES_LAYER = 13;
        private readonly int NO_LAYER = 14;

        private struct UIData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<UIPopUpComponent> UI;
            public ComponentDataArray<UIActiveComponent> Active;
        }

        [Inject] private UIData uiData;

        private struct ObjectClickedData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<ClickedComponent> Object;
        }

        [Inject] private ObjectClickedData objectData;

        private struct ObjectRemoveData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<RemoveBuildComponent> Object;
        }

        [Inject] private ObjectRemoveData removeData;

        protected override void OnUpdate()
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < uiData.Length; i++)
            {
                if (!uiData.UI[i].UIContainer.activeInHierarchy)
                {
                    uiData.UI[i].UIContainer.SetActive(true);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                List<RaycastResult> results = new List<RaycastResult>();

                if (uiData.Length > 0)
                {
                    ProcessCanvasRaycast(results);
                }

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.layer == YES_LAYER)
                    {
                        for (int i = 0; i < objectData.Length; i++)
                        {
                            puc.AddComponent(objectData.Entity[i], new YesComponent());
                        }

                        RemovePopUp();
                    }
                    else if (result.gameObject.layer == NO_LAYER)
                    {
                        for (int i = 0; i < objectData.Length; i++)
                        {
                            puc.RemoveComponent<ClickedComponent>(objectData.Entity[i]);
                        }

                        for(int i = 0; i < removeData.Length; i++)
                        {
                            puc.RemoveComponent<RemoveBuildComponent>(objectData.Entity[i]);
                        }

                        RemovePopUp();
                    }
                }
            }
        }

        private void ProcessCanvasRaycast(List<RaycastResult> results)
        {
            GraphicRaycaster raycaster = uiData.UI[0].UIContainer.GetComponentInParent<GraphicRaycaster>();
            EventSystem eventSystem = uiData.UI[0].UIContainer.GetComponentInParent<EventSystem>();

            PointerEventData m_PointerEventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            raycaster.Raycast(m_PointerEventData, results);
        }


        private void RemovePopUp ()
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < uiData.Length; i++)
            {
                if (uiData.UI[i].UIContainer.activeInHierarchy)
                {
                    puc.RemoveComponent<UIActiveComponent>(uiData.Entity[i]);
                    uiData.UI[i].UIContainer.SetActive(false);
                }
            }
        }
    }
}