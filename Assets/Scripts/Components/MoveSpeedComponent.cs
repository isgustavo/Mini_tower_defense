using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct MoveSpeed : IComponentData 
    {
        public float Value; 
    }

    public class MoveSpeedComponent : ComponentDataWrapper<MoveSpeed> { }
}