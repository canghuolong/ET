using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(UIComponent))]
    public static class UIHelper
    {
        public static async ETTask<T> OpenUI<T>(Entity entity, string address,UILayer uiLayer) where T : Entity,IAwake,new()
        {
           UIComponent uiComponent = entity.Root().GetComponent<UIComponent>();
           UI ui = await uiComponent.Create(address);
           T component = ui.AddComponent<T>();
           var root = uiComponent.Layers[(int)uiLayer];
           ui.GameObject.transform.SetParent(root);
           ui.GameObject.transform.localScale = Vector3.one;
           return component;
        }

        public static void CloseUI(Entity entity, string address)
        {
            UIComponent uiComponent = entity.Root().GetComponent<UIComponent>();
            uiComponent.Remove(address);
        }
    }
}

