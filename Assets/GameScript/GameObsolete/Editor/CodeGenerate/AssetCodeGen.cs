using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDevKit;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GameDevKitEditor
{
    public class AssetCodeGen
    {
        
        [MenuItem("FTool/批量重命名")]
        public static void RenameAssets()
        {
            var path=AssetDatabase.GetAssetPath(Selection.activeObject);
            List<FileKit.UnityFileInfo> infos = new List<FileKit.UnityFileInfo>();
            FileKit.GetAllSubUnityFileInfos(ref  infos,path);
            static void Test3(List<FileKit.UnityFileInfo> unityFileInfo, int i)
            {
                if (unityFileInfo == null) return;
                foreach (var fileInfo in unityFileInfo)
                {
                    if (fileInfo.IsDir)
                    {
                        Test3(fileInfo.subFileInfos,0);
                    }
                    else
                    {
                        AssetDatabase.RenameAsset(fileInfo.assetpath, $"{fileInfo.TopDir}_{i++}");
                    }
                }
            }
            Test3(infos,0);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public static HashSet<string> exclue_dir =new HashSet<string>()
        {
          "SpritAtalas","NPC","Sprites","Data"
        };
        
        //TODO:纯脑血栓写出来的不易阅读的递归代码,看的头疼,留作纪念,以此警示
        public static void Testwea()
        {
            string spritepath = "Assets/Resources/Sprite";
            string generatepath = "Assets/Script/Generate/";
            string filename = "FRes";
            
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("using GameDevKit;");
            builder.AppendLine("using UnityEngine;");
            builder.AppendLine("using DefaultNamespace;");
            builder.AppendLine("using DG.Tweening.Core;");
            builder.AppendLine($"public static class {filename}");
            builder.AppendLine("{");
            
            static void Test2(List<FileKit.UnityFileInfo> unityFileInfo,  ref StringBuilder builder)
            {
                if (unityFileInfo == null) return;
            
                foreach (var fileInfo in unityFileInfo)
                {
                    if (fileInfo.IsDir)
                    {
                    
                        //排除目录外不用遍历
                        if(exclue_dir.Contains(fileInfo.name)) 
                            continue;
                        Debug.Log("目录" + fileInfo.assetpath);
                        builder.AppendLine($"public static class {fileInfo.name}");
                        builder.AppendLine("{");
                        Test2(fileInfo.subFileInfos,ref builder);
                        builder.AppendLine("}");
                    
                    }
                    else
                    {
                        Debug.Log("文件" + fileInfo.assetpath);
                        string path = fileInfo.Resourcepath;
                        string typename=fileInfo.obj.GetType().Name;
                        if ( typename == "Texture2D" )
                        {
                            typename = "Sprite";
                        }
                        string temp =$"FResources.ResLoad<{typename}>(\"{path}\")";
                        builder.AppendLine(FCode.CreateInstance(fileInfo.name,
                            typename,temp
                        ));
                    }
                }
            }
            
            List<FileKit.UnityFileInfo> infos = new List<FileKit.UnityFileInfo>();
            Dictionary<string,string> paths = new Dictionary<string, string>();
            FileKit.GetAllSubUnityFileInfos(ref  infos,"Assets/Resources");
            Test2(infos,ref builder);
            FileKit.GetAllPaths(infos,ref paths);
            
                builder.AppendLine($"public static class Paths");
                builder.AppendLine("{");
                
                foreach ( var keyValuePair in paths )
                {
                    builder.AppendLine($"public const string {keyValuePair.Key} = \"{keyValuePair.Value}\";");
                }
                builder.AppendLine("}");
            builder.AppendLine("}");
            
            FileKit.CreateScriptAndRefresh(generatepath,filename,builder.ToString());

        }
    }
}