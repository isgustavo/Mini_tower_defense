using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct IdleComponent : IComponentData { }

    public class IdleComponentWrapper : ComponentDataWrapper<IdleComponent> { }
}