using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [EntitySystemOf(typeof(UI))]
    public static partial class UISystem
    {
        [EntitySystem]
        private static void Awake(this ET.UI self, string address, UnityEngine.GameObject gameObject)
        {
            self.GameObject = gameObject;
            self.Address = address;
        }
        [EntitySystem]
        private static void Destroy(this ET.UI self)
        {
            UnityEngine.Object.Destroy(self.GameObject);
        }
    }

    [ChildOf]
    public sealed class UI : Entity,IAwake<string,GameObject>,IDestroy
    {
        public GameObject GameObject { set; get; }
        public string Address { get; set; }
    }
}
