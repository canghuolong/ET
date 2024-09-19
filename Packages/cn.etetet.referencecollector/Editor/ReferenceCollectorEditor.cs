using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
//Object并非C#基础中的Object，而是 UnityEngine.Object
using Object = UnityEngine.Object;


//自定义ReferenceCollector类在界面中的显示与功能
    [CustomEditor(typeof(ReferenceCollector))]
    public class ReferenceCollectorEditor : UnityEditor.Editor
    {
        //输入在textfield中的字符串
        private string searchKey
        {
            get { return _searchKey; }
            set
            {
                if (_searchKey != value)
                {
                    _searchKey = value;
                    heroPrefab = referenceCollector.Get<Object>(searchKey);
                }
            }
        }

        private ReferenceCollector referenceCollector;

        private Object heroPrefab;

        private string _searchKey = "";

        private static readonly List<Type> _typeList = new List<Type>();

        static ReferenceCollectorEditor()
        {
            List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(Object)) && !type.IsAbstract && !type.IsGenericType &&
                        CanShowType(type))
                    {
                        _typeList.Add(type);
                    }
                }
            }

            _typeList.Sort((t1, t2) => (string.Compare(t1.FullName, t2.FullName)));
            _typeList.Insert(0, typeof(GameObject));
        }

        private void DelNullReference()
        {
            var dataProperty = serializedObject.FindProperty("data");
            for (int i = dataProperty.arraySize - 1; i >= 0; i--)
            {
                var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("gameObject");
                if (gameObjectProperty.objectReferenceValue == null)
                {
                    dataProperty.DeleteArrayElementAtIndex(i);
                    EditorUtility.SetDirty(referenceCollector);
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.UpdateIfRequiredOrScript();
                }
            }
        }

        private void OnEnable()
        {
            //将被选中的gameobject所挂载的ReferenceCollector赋值给编辑器类中的ReferenceCollector，方便操作
            referenceCollector = (ReferenceCollector)target;
        }


        public override void OnInspectorGUI()
        {
            //使ReferenceCollector支持撤销操作，还有Redo，不过没有在这里使用
            Undo.RecordObject(referenceCollector, "Changed Settings");
            var dataProperty = serializedObject.FindProperty("data");
            //开始水平布局，如果是比较新版本学习U3D的，可能不知道这东西，这个是老GUI系统的知识，除了用在编辑器里，还可以用在生成的游戏中
            GUILayout.BeginHorizontal();
            //下面几个if都是点击按钮就会返回true调用里面的东西
            if (GUILayout.Button("添加引用"))
            {
                //添加新的元素，具体的函数注释
                // Guid.NewGuid().GetHashCode().ToString() 就是新建后默认的key
                AddReference(dataProperty, Guid.NewGuid().GetHashCode().ToString(), null);
            }

            if (GUILayout.Button("全部删除"))
            {
                referenceCollector.Clear();
            }

            if (GUILayout.Button("删除空引用"))
            {
                DelNullReference();
            }

            if (GUILayout.Button("排序"))
            {
                referenceCollector.Sort();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            //可以在编辑器中对searchKey进行赋值，只要输入对应的Key值，就可以点后面的删除按钮删除相对应的元素
            searchKey = EditorGUILayout.TextField(searchKey);
            //添加的可以用于选中Object的框，这里的object也是(UnityEngine.Object
            //第三个参数为是否只能引用scene中的Object
            EditorGUILayout.ObjectField(heroPrefab, typeof(Object), false);
            if (GUILayout.Button("删除"))
            {
                referenceCollector.Remove(searchKey);
                heroPrefab = null;
            }

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            var delList = new List<int>();

            //遍历ReferenceCollector中data list的所有元素，显示在编辑器中
            for (int i = referenceCollector.data.Count - 1; i >= 0; i--)
            {
                //这里的知识点在ReferenceCollector中有说
                SerializedProperty keyProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("key");
                SerializedProperty typeProperty =
                    dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("typeName");
                SerializedProperty gameObjectProperty =
                    dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("gameObject");

                GUILayout.BeginHorizontal();

                keyProperty.stringValue = EditorGUILayout.TextField(keyProperty.stringValue, GUILayout.Width(150));

                
                Type type = GetType(typeProperty.stringValue);
                
                if (GUILayout.Button(type.Name,GUILayout.Width(80),GUILayout.ExpandWidth(true)))
                {
                    var provider = CreateInstance<TypeSearchProvider>();
                    provider.SetData(_typeList, selectedName =>
                    {
                        bool valueChanged = true;// typeProperty.stringValue != selectedName.ToString();

                        Debug.Log($"选择的类型: {selectedName} - ValueChanged: {valueChanged}");

                        if (valueChanged)
                        {
                            typeProperty.stringValue = selectedName.ToString();
                            //更新类型
                            type = GetType(typeProperty.stringValue);

                            GameObject go = gameObjectProperty.objectReferenceValue as GameObject;
                            if (go == null)
                            {
                                go = (gameObjectProperty.objectReferenceValue as Component)?.gameObject;
                            }

                            if (selectedName.ToString() == "UnityEngine.GameObject")
                            {
                                gameObjectProperty.objectReferenceValue = go;
                            }
                            else
                            {
                                if (go != null && go.TryGetComponent(type, out var comp))
                                {
                                    gameObjectProperty.objectReferenceValue = comp;
                                }
                            }
                        }

                        serializedObject.ApplyModifiedProperties();
                    });

                    var lastRect = GUILayoutUtility.GetRect(100, 100);
                    SearchWindow.Open(
                        new SearchWindowContext(new Vector2(lastRect.x, lastRect.y), lastRect.width, lastRect.height),
                        provider);
                }
                
                gameObjectProperty.objectReferenceValue =
                    EditorGUILayout.ObjectField(gameObjectProperty.objectReferenceValue, typeof(Object), true);


                if (GUILayout.Button("X"))
                {
                    //将元素添加进删除list
                    delList.Add(i);
                }

                GUILayout.EndHorizontal();
            }


            var eventType = Event.current.type;
            //在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
            if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
            {
                // Show a copy icon on the drag
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (eventType == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (var o in DragAndDrop.objectReferences)
                    {
                        AddReference(dataProperty, o.name, o);
                    }
                }

                Event.current.Use();
            }

            //遍历删除list，将其删除掉
            foreach (var i in delList)
            {
                dataProperty.DeleteArrayElementAtIndex(i);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        //添加元素，具体知识点在ReferenceCollector中说了
        private void AddReference(SerializedProperty dataProperty, string key, Object obj)
        {
            int index = dataProperty.arraySize;
            dataProperty.InsertArrayElementAtIndex(index);
            var element = dataProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("key").stringValue = key;

            var typeName = element.FindPropertyRelative("typeName").stringValue;
            if (typeName == "UnityEngine.GameObject" || string.IsNullOrEmpty(typeName))
            {
                element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
            }
            else
            {
                if (obj != null && (obj as GameObject).TryGetComponent(GetType(typeName), out var comp))
                {
                    element.FindPropertyRelative("gameObject").objectReferenceValue = comp;
                }
                else
                {
                    element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
                }
            }
        }

        public static Type GetType(string typeName, bool deepSearch = true)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return typeof(Object);
            }

            foreach (Type type in _typeList)
            {
                if (type.FullName == typeName) return type;
            }

            if (deepSearch)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type type = assembly.GetType(typeName);
                    if (type != null) return type;
                }
            }

            return typeof(Object);
        }

        static bool CanShowType(Type type)
        {
            return true;
        }


        public class TypeSearchProvider : ScriptableObject, ISearchWindowProvider
        {
            private List<Type> _typeList;
            private GenericMenu.MenuFunction2 _callback;

            public void SetData(List<Type> typeList, GenericMenu.MenuFunction2 callback)
            {
                _typeList = typeList;
                _callback = callback;
            }

            public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
            {
                var list = new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent("检索组件")) };
                if (_typeList == null || _typeList.Count <= 0)
                    return list;
                var dic = new Dictionary<string, List<string>>();
                foreach (var type in _typeList)
                {
                    // var tSpace = type.Namespace;
                    var tName = type.Name;
                    var tFullName = type.FullName;
                    //如果是Editor的子类,则不在编辑窗口展示
                    if (string.IsNullOrEmpty(tFullName) || typeof(UnityEditor.Editor).IsAssignableFrom(type))
                        continue;

                    var lastIndex = tFullName.LastIndexOf('.');
                    var key = lastIndex >= 0 ? tFullName.Substring(0, lastIndex) : "[No Namespace]";

                    if (!dic.ContainsKey(key))
                        dic[key] = new List<string>();
                    dic[key].Add($"{tName}/{tFullName}");
                }

                // 键值排序
                var sortedKeys = dic.Keys.OrderBy(key => key).ToList();
                var groups = new List<string>();
                foreach (var key in sortedKeys)
                {
                    var groupSplit = key.Split('.');
                    if (groupSplit.Length <= 0)
                        continue;
                    var groupName = "";
                    var level = 1;
                    foreach (var content in groupSplit)
                    {
                        groupName += content;
                        if (!groups.Contains(groupName))
                        {
                            groups.Add(groupName);
                            list.Add(new SearchTreeGroupEntry(new GUIContent(content), level));
                        }

                        groupName += "/";
                        level++;
                    }

                    var value = dic[key];
                    foreach (var t in value)
                    {
                        var split = t.Split('/');
                        if (split.Length < 2)
                            continue;
                        var tName = split[0];
                        var tFullName = split[1];
                        var entry = new SearchTreeEntry(new GUIContent($"{tName}",
                            EditorGUIUtility.FindTexture("cs Script Icon")))
                        {
                            level = level,
                            userData = tFullName,
                        };
                        list.Add(entry);
                    }
                }

                return list;
            }

            public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
            {
                _callback?.Invoke(entry.userData);
                return true;
            }
        }
    }
