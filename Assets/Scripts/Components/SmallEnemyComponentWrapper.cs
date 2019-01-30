using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct SmallEnemyComponent : IComponentData { }

    public class SmallEnemyComponentWrapper : ComponentDataWrapper<SmallEnemyComponent> { }
}