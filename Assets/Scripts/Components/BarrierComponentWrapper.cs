using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct BarrierComponent : IComponentData { }

    public class BarrierComponentWrapper : ComponentDataWrapper<BarrierComponent> { }
}