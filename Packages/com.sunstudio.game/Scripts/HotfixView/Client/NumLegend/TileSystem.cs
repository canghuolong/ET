using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(Tile))]
    public static partial class TileSystem
    {
        public static void SetValue(this Tile self, int value)
        {
            self.Value = value;
        }

        public static void SetPosition(this Tile self, Vector2 position)
        {
            self.Position = position;
        }
        [EntitySystem]
        private static void Awake(this ET.Client.Tile self)
        {

        }
    }
}

