using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct StaticComponent : IComponentData { }

    public class StaticComponentWrapper : ComponentDataWrapper<StaticComponent> { }
}