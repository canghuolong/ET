using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainViewComponent))]
    public static partial class UIMainViewComponentSysten
    {
        [EntitySystem]
        private static void Awake(this ET.Client.UIMainViewComponent self)
        {
            var rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.Text = rc.Get<TextMeshProUGUI>("Text");
            self.Button = rc.Get<Button>("Button");

            self.Button.onClick.AddListener((() =>
            {

                UIHelper.CloseUI(self, self.GetParent<UI>().Address);
            }));
        }
        [EntitySystem]
        private static void Destroy(this ET.Client.UIMainViewComponent self)
        {
            Debug.Log("Destroy Me!");
        }
    }
}

