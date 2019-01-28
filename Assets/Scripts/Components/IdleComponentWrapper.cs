using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ODT.Component
{
    [Serializable]
    public struct IdleComponent : IComponentData { }

    public class IdleComponentWrapper : ComponentDataWrapper<IdleComponent> { }
}