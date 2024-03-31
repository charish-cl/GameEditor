namespace GameEditor
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// 文件夹生成工具，初始化文件目录
    /// </summary>
    public class CreateFolders
    {
        public static string[] mainFolders = new string[]
        {
            "Scene", "Font", "Prefab", "Materials", "Textures", 
            "Animation", "Model", "Shader", "UIForm",
            "UIComponent","Config"
        };

        public static string[] subFolders = new string[]
        {
            "LocalResources", "Common"
        };

        [MenuItem("Tools/Create Game Folders")]
        static void CreateGameFolders()
        {
            string gameFolder = "Assets/Game";
            CreateFolder(gameFolder);


            foreach (string folder in mainFolders)
            {
                string fullPath = Path.Combine(gameFolder, folder);
                CreateFolder(fullPath);

                foreach (string subFolder in subFolders)
                {
                    CreateFolder(Path.Combine(fullPath, subFolder));
                }
            }

            AssetDatabase.Refresh();
        }

        static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.ImportAsset(path); // 让Unity识别新创建的文件夹
            }
        }

        [MenuItem("Tools/Create Specific Folder")]
        public static void CreateSpecificFolder()
        {
            string folderName = EditorUtility.SaveFolderPanel("Choose Location for New Folder", "", "");
            if (!string.IsNullOrEmpty(folderName))
            {
                CreateFolderWithSubFolders(folderName);
            }
        }

        public static void CreateFolderWithSubFolders(string folderPath)
        {
            string relativePath = "Assets" + folderPath.Substring(Application.dataPath.Length);
            CreateFolder(relativePath);

            foreach (string subFolder in subFolders)
            {
                CreateFolder(Path.Combine(relativePath, subFolder));
            }
        }
    }
}