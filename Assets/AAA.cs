using System.Collections;
using System.Collections.Generic;
using ET;
using UnityEngine;


namespace ET
{
    [EntitySystemOf(typeof(H))]
    public static partial class AAA
    {
        [EntitySystem]
        private static void Awake(this H self)
        {

        }
    }

    [UniqueId]
    public static partial class U
    {
        public const int A = 1;
        public const int B = 1;
    }
}




public class H : Entity ,IAwake
    {
        
    }

