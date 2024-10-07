using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILegendViewComponent))]
    public static partial class UILegendViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.UILegendViewComponent self)
        {
            var uiObj = self.GetParent<UI>().GameObject;
            self.Points = new Transform[25];
            for (int i = 0; i < 25; i++)
            {
                self.Points[i] = uiObj.Get<Transform>($"PlaceHolder_{i}");
                self.AddChildWithId<Tile>(i).AddComponent<TileViewComponent>();
            }
            
        }
        [EntitySystem]
        private static void Destroy(this ET.Client.UILegendViewComponent self)
        {

        }
    }
}

