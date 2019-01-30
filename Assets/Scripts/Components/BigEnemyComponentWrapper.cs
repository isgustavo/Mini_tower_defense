using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct BigEnemyComponent : IComponentData { }

    public class BigEnemyComponentWrapper : ComponentDataWrapper<BigEnemyComponent> { }
}