using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIMainViewComponent : Entity,IAwake,IDestroy
    {
#pragma warning disable ET0004
        public TextMeshProUGUI Text { set; get; }

        public Button Button;
#pragma warning restore ET0004
        
    }
}

