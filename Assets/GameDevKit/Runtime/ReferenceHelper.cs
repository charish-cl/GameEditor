using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GameDevKit
{
    //TODO，自定义Add方法 可以多选像深渊那时一样
    public class ReferenceHelper:SerializedMonoBehaviour
    {
        public class MonoReference
        {
            public string name;
            public GameObject go;
            [Button]
            void Rename()
            {
                go.name = $"{go.name.SplitBefore('_')}_{name}";
            }
        }
        [TableList(DrawScrollView = true, MinScrollViewHeight = 400)]
        public List<MonoReference> ReferencesList = new List<MonoReference>();

        private HashSet<string> _hashSet;
        public void Add(MonoReference monoReference)
        {
            ReferencesList??= new List<MonoReference>();
            _hashSet??= new HashSet<string>();
            if (_hashSet.Contains(monoReference.name))
            {
                Debug.Log($"已存在{monoReference.name}!");
                return;
            }
            ReferencesList.Add(monoReference);
            _hashSet.Add(monoReference.name);
        }
        
        public static Dictionary<string, string> dicWidget = new Dictionary<string, string>()
        {
            {"Text (TMP)","TextMeshProUGUI"},
            {"Image","Image"},
            
            {"ScrollView","GridWindow"},
            {"Scroll View","GridWindow"},
            {"Button","Button"},
            
            {"m_textmesh","TextMeshProUGUI"},
            {"m_go", "GameObject"},
            {"m_item", "GameObject"},
            {"m_tf", "Transform"},
            {"m_rect", "RectTransform"},
            {"m_text", "Text"},
            {"m_richText", "RichTextItem"},
            {"m_tbtn", "TextButtonItem"},
            {"m_btn", "Button"},
            {"m_img", "Image"},
            {"m_rimg", "RawImage"},
            {"m_scroll", "ScrollRect"},
            {"m_input", "InputField"},
            {"m_grid", "GridLayoutGroup"},
            {"m_clay", "CircleLayoutGroup"},
            {"m_hlay", "HorizontalLayoutGroup"},
            {"m_vlay", "VerticalLayoutGroup"},
            {"m_slider", "Slider"},
            {"m_group", "ToggleGroup"},
            {"m_toggle", "Toggle"},
            {"m_curve", "AnimationCurve"},
        };
        static FCodeBuilder builder = new FCodeBuilder();
        static FCodeBuilder builder_prop = new FCodeBuilder();
        static FCodeBuilder builder_register = new FCodeBuilder();
        static FCodeBuilder builder_Func = new FCodeBuilder();

        [Button]
        public void CreateNewCode()
        {
            builder.Clear();
            builder_prop.Clear();
            builder_register.Clear();
            builder_Func.Clear();
           foreach (var monoReference in ReferencesList)
           {
               if (monoReference.go == null)
               {
                   ReferencesList.Remove(monoReference);
                   continue;
               }

               bool isContain = false;
               foreach (var keyValuePair in dicWidget)
               {
                   if ( monoReference.go.name.StartsWith(keyValuePair.Key) )
                   {
                       isContain = true;
                       HandleChild(monoReference,this.transform, keyValuePair.Key);
                   }
               }

               if (!isContain)
               {
                   HandleChild(monoReference,this.transform, "m_go");
               }
           }

           builder.AddNameSpace("GameDevKit", "UnityEngine.UI");
           builder.AddClassAndNamSpace(name,"DefaultNamespace","UIWindow");
            builder.AppendLine(builder_prop.ToString());
            builder.AddFunction("Awake")
                .AppendContent(builder_register.ToString())
                .AppendLine(builder_Func.ToString());

            SystemHelper.Copy(builder.ToString());
        }
        
        //创建Partial类 不用组件
        [Button]
        public void CreatePartialClass()
        {
            builder.AddNameSpace("GameDevKit", "UnityEngine.UI");
            builder.AddClassAndNamSpace(name,"DefaultNamespace","UIWindow");
            builder.AppendLine(builder_prop.ToString());
            SystemHelper.Copy(builder.ToString());
        }
        private static void HandleChild(MonoReference monoReference, Transform parent, string key)
        {
            string name = monoReference.name;
            string path = monoReference.go.transform.GetRelativePath(parent);
            var type = dicWidget[key];
            builder_prop.AppendLine($"private {type} {name};");
            switch (type)
            {
                case "GameObject":
                    builder_register.AppendLine($"{name} = transform.Find(\"{path}\").gameObject;");
                    break;
                case "Button":
                    builder_register.AppendLine($"{name} = transform.FindChildComponent<{type}>(\"{path}\");");
                    builder_register.AppendLine($"{name}.onClick.AddListener({GetBtnFuncName(name)});");
                    builder_Func.AddFunction(GetBtnFuncName(name)).EndCharacter();
                    break;
                default:
                    builder_register.AppendLine($"{name} = transform.FindChildComponent<{type}>(\"{path}\");");
                    break;
            }
        }

        private static string GetBtnFuncName(string varName)
        {
            return "OnClick" + varName.Replace("m_btn", string.Empty) + "Btn";
        }
        
    }
}

