using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameDevKit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevKit
{
    public class UIFormGenarate
    {
        public static Dictionary<string, string> dicWidget = new Dictionary<string, string>()
        {
            {"Text (TMP)","TextMeshProUGUI"},
            {"Image","Image"},
            
            
            {"ScrollView","GridWindow"},
            {"Scroll View","GridWindow"},
            
            
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

        static public void CreateNewCode()
        {
            GameObject go = Selection.activeObject as GameObject;
            if ( go.Equals(null) )
                Debug.Log("选择的物体为空");

            builder.Clear();
            builder_prop.Clear();
            builder_register.Clear();
            builder_Func.Clear();
            
            go.transform.ForEachChild(e =>
            {
                foreach ( var keyValuePair in dicWidget )
                {
                    if ( e.name.StartsWith(keyValuePair.Key) )
                    {
                        HandleChild(e, go.transform, keyValuePair.Key);
                    }
                }
            });

            builder.AddClassAndNamSpace(go.name,"DefaultNamespace","UIWindow");
            builder.AppendLine(builder_prop.ToString());
            builder.AddFunction("Awake")
                .AppendContent(builder_register.ToString())
                .AppendLine(builder_Func.ToString());

            SystemHelper.Copy(builder.ToString());
        }

        static string GetName(Transform transform)
        {
            string name = transform.name;
            foreach ( var keyValuePair in dicWidget )
            {
                if (name.StartsWith(keyValuePair.Key) )
                {
                    return name.Split('_')[1];
                }
            }

            return null;
        }
        private static void HandleChild(Transform transform, Transform parent, string key)
        {
            string name = GetName(transform);
            string path = transform.GetRelativePath(parent);
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

        private static string path = "Assets/Editor/GameDevKit/CodeGenerate/UI模板.txt";

    }
}