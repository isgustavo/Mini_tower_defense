using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct EnemyComponent : IComponentData { }

    public class EnemyComponentWrapper : ComponentDataWrapper<EnemyComponent> { }
}