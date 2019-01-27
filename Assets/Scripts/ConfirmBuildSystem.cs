using System.Collections.Generic;
using ODT.Component;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ODT.System
{
    public class ConfirmBuildSystem : ComponentSystem
    {
        private int YES_LAYER = 13;
        private int NO_LAYER = 14;

        private struct UIObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<UIConfirmComponent> UI;
        }

        [Inject] private UIObjectData data;

        private struct SlotClickedData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<SlotClickedComponent> Slot;
            public ComponentArray<BuilderComponent> Build;
        }

        [Inject] private SlotClickedData slotData;

        protected override void OnUpdate()
        {
            var puc = PostUpdateCommands;

            if (Input.GetMouseButtonDown(0))
            {
                List<RaycastResult> results = new List<RaycastResult>();

                if (data.Length > 0)
                {
                    ProcessCanvasRaycast(results);
                }

                foreach (RaycastResult result in results)
                {
                    for (int i = 0; i < slotData.Length; i++)
                    {
                        if (result.gameObject.layer == YES_LAYER)
                        {
                            BuildObject(slotData.Build[i].ObjPrefab, slotData.Build[i].SpawnObjPoint.position);

                            puc.RemoveComponent<SlotClickedComponent>(slotData.Entity[i]);

                        }
                        else if (result.gameObject.layer == NO_LAYER)
                        {
                            puc.RemoveComponent<SlotClickedComponent>(slotData.Entity[i]);
                        }
                    }
                }

                for (int i = 0; i < data.Length; i++)
                {
                    data.UI[i].UIContainer.SetActive(false);
                }
            }
        }

        private void ProcessCanvasRaycast(List<RaycastResult> results) 
        {
            GraphicRaycaster raycaster = data.UI[0].UIContainer.GetComponentInParent<GraphicRaycaster>();
            EventSystem eventSystem = data.UI[0].UIContainer.GetComponentInParent<EventSystem>();

            PointerEventData m_PointerEventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            raycaster.Raycast(m_PointerEventData, results);
        }

        private void BuildObject(GameObject obj, Vector3 position) 
        {
            Object.Instantiate(obj, position, Quaternion.identity);
        }
    }
}