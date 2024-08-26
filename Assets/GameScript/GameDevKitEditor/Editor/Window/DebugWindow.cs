using System.Collections.Generic;
using System.Linq;
using GameEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GameDevKitEditor
{
    [TreeWindow("调试工具")]
    public class DebugWindow:OdinEditorWindow
    {   
        public string GetCurrentAssetDirectory()
        {
            // 获取当前选中的资源
            var GUIDs = Selection.assetGUIDs;
            foreach (var guid in GUIDs)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                return path;
                //输出结果为：Assets/测试文件.png
            }

            return null;
        }
        public List<Object> AssetDataBaseGetAllFolderAsset(string directoryPath)
        {
            // 获取目录下的所有资源GUID
            string[] guids = AssetDatabase.FindAssets("", new string[] { directoryPath });

            // 创建列表来存储所有资源
            List<Object> assetsList = new List<Object>();

            // 遍历所有资源GUID，并加载资源添加到列表中
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
                if (asset != null)
                {
                    assetsList.Add(asset);
                }
            }

            return assetsList;
        }
       
        [TabGroup("合并资源")] [Searchable] public List<List<Texture2D>> NeedMergeResources = new List<List<Texture2D>>();
        [TabGroup("合并资源")] [LabelText("选中目录")] public string DirectoryPath;

        [TabGroup("合并资源")]
        [Button("获取当前打开文件夹下的相似资源", ButtonHeight = 50)]
        public void GetSimilarAsset()
        {
            var path = GetCurrentAssetDirectory();


            if (DirectoryPath != null)
            {
                path = DirectoryPath;
            }

            var assets = AssetDataBaseGetAllFolderAsset(path)
                .Where(e => e.GetType() == typeof(Texture2D))
                .Select(e => e as Texture2D).ToList();

            NeedMergeResources?.Clear();
            NeedMergeResources = new List<List<Texture2D>>();
            ImageComparer.imageCache.Clear();
            foreach (var texture2D in assets)
            {
                ImageComparer.AddImage(texture2D);
            }

            Dictionary<string, List<Texture2D>> groupedAssets = new Dictionary<string, List<Texture2D>>();

            groupedAssets = ImageComparer.imageCache;
            // 将分组的资源添加到NeedMergeResources中
            foreach (var group in groupedAssets)
            {
                //只有一个不算是冗余资源
                if (group.Value.Count == 1)
                {
                    continue;
                }
                NeedMergeResources.Add(group.Value);
            }
        }

        [TabGroup("合并资源")]
        [Button("替换NeedMergeResources中的依赖对象", ButtonHeight = 50)]
        public void ReplaceNeedMergeResources()
        {
           
            foreach (var resources in NeedMergeResources)
            {
                var sprites = resources.Select(e =>
                    AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(e))).ToArray();
                //查找所有依赖，替换为第一张sprite
                for (var index = 1; index < sprites.Length; index++)
                {
                    var s = sprites[index];
               
                }

                //删除只保留第一个
                for (var i = sprites.Length - 1; i > 0; i--)
                {
                     AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(sprites[i]));
                }
            }
            AssetDatabase.Refresh();
        }
    }
}