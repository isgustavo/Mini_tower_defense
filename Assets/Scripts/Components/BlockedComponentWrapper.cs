using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct BlockedComponent : IComponentData { }

    public class BlockedComponentWrapper : ComponentDataWrapper<BlockedComponent> { }
}