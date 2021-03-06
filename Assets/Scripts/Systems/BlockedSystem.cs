﻿using ODT.Component;
using Unity.Entities;
using UnityEngine;

namespace ODT.System
{
    public class BlockedSystem : ComponentSystem
    {
        private readonly int ENEMY_LAYER_MASK = 1 << 10;

        private struct ObjectData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<Transform> Transform;
            public ComponentDataArray<BlockedComponent> Block;
        }

        [Inject] private ObjectData data;

        protected override void OnUpdate()
        {
            var puc = PostUpdateCommands;

            for (int i = 0; i < data.Length; i++)
            {
                if (!Physics.Raycast(data.Transform[i].position, Vector3.right, out RaycastHit hit, 1f))
                {
                    if (!Physics.Raycast(data.Transform[i].position, Vector3.up, out hit, 1f)) 
                    {
                        puc.RemoveComponent<BlockedComponent>(data.Entity[i]);
                    }
                }
            }
        }
    }
}