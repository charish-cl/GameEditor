using System.IO;
using System.Text;
using GameDevKit;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GameDevKitEditor
{
    [TreeWindow("代码生成工具")]
    public class CodeGenerateWindow:OdinEditorWindow
    {
 
        [InlineEditor()]
        public EnumGenerateData data;

        protected override void OnEnable()
        {
          data =  ResourcesKit.LoadAssetAtPath<GameDevKit.EnumGenerateData>("Assets/GameDevKit/Config/EnumGenerate/Enum Generate Data.asset");
          
        }

        [BoxGroup("Group1")]
        [Button("生成Tag枚举")]
        public static void GeneratTagEnum()
        {
            var tags = InternalEditorUtility.tags;
            var arg = "";
            foreach (var tag in tags)
            {
                arg += "\t" + tag + ",\n";
            }

            string filename = "EnumTag";
            var res = "public enum \t" +filename+"\n{\n" + arg + "}";
       
            var path = Application.dataPath + $"/Script/Generate/Tag/";
         
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText(path+$"{filename}.cs", res, Encoding.UTF8);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        [BoxGroup("Group1")]
        [InfoBox("右键资源可以添加文件夹路径,此功能把该文件夹下的子文件路径生成一个枚举,用于快速访问")]
        [Button("全部生成枚举")]
        public static void GeneratEnum()
        {
            var data = ResourcesKit.LoadAssetAtPath<GameDevKit.EnumGenerateData>("Assets/GameDevKit/Config/EnumGenerate/Enum Generate Data.asset");
            data.Generate();
        }
    }
}