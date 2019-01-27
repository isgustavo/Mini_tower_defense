using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct BlockComponent : IComponentData { }

    public class BlockComponentWrapper : ComponentDataWrapper<BlockComponent> { }
}