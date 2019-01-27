using System;
using Unity.Entities;

namespace ODT.Component
{
    [Serializable]
    public struct BulletComponent : IComponentData 
    {
        public int speed;
        public int damage; 
    }

    public class BulletComponentWrapper : ComponentDataWrapper<BulletComponent> { }
}