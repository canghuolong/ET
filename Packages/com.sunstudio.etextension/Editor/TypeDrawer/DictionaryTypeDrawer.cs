using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ET
{
    [TypeDrawer]
    public class DictionaryIntTransformTypeDrawer : ITypeDrawer
    {
        public bool HandlesType(Type type)
        {
            if (type == typeof(Dictionary<int,Transform>))
            {
                return true;
            }

            return false;
        }

        public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            Dictionary<int, Transform> dictionary = value as Dictionary<int, Transform>;

            EditorGUILayout.LabelField($"{memberName}:");
            foreach ((int k, Transform v) in dictionary)
            {
                EditorGUILayout.ObjectField($"{k}:",v, typeof(Transform));
            }
            return value;
        }
    }
}
