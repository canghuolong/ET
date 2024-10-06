using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class UIComponent : Entity,IAwake<ReferenceCollector>
    {
        public List<EntityRef<UI>> UIs = new();
        public Dictionary<int, Transform> Layers = new();
    } 
}

