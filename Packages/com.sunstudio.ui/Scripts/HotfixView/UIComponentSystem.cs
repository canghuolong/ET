using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIComponent))]
    public static partial class UIComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.UIComponent self, ReferenceCollector referenceCollector)
        {
            self.Layers[(int)UILayer.Hidden] = referenceCollector.Get<Transform>("Hidden");
            self.Layers[(int)UILayer.Low] = referenceCollector.Get<Transform>("Low");
            self.Layers[(int)UILayer.Middle] = referenceCollector.Get<Transform>("Mid");
            self.Layers[(int)UILayer.High] = referenceCollector.Get<Transform>("High");
        }

        private static UI FindUI(this UIComponent self, string address)
        {
            foreach (UI v in self.UIs)
            {
                if (v.Address == address)
                {
                    return v;
                }
            }

            return null;
        }

        public static async ETTask<UI> Create(this UIComponent self, string address)
        {
            GameObject srcObj = await self.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(address);
            UI ui = self.AddChild<UI,string,GameObject>(address,
                UnityEngine.Object.Instantiate(srcObj));
            self.UIs.Add(ui);
            return ui;
        }

        public static void Remove(this UIComponent self, string address)
        {
            UI ui = self.FindUI(address);
            if (ui != null)
            {
                self.UIs.Remove(ui);
                ui.Dispose();
            }
        }
    }
}