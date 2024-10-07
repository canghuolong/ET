using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Tile))]
    public class TileViewComponent : Entity,IAwake
    {
        
#pragma warning disable ET0004
        private GameObject gameObject;
        
        public GameObject GameObject
        {
            get
            {
                return this.gameObject;
            }
            set
            {
                this.gameObject = value;
                this.Transform = value.transform;
            }
        }

        public Transform Transform { get; private set; }
        
#pragma warning restore ET0004

        
    }
}

