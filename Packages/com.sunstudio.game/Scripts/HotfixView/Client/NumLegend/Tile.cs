using UnityEngine;

namespace ET.Client
{
    [ChildOf()]
    public class Tile : Entity,IAwake
    {
#pragma warning disable ET0004
        public int Value;
        public Vector2 Position;
#pragma warning restore ET0004
    }    
}

