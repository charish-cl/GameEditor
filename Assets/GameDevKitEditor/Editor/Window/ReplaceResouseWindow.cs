using System.Diagnostics;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace GameDevKitEditor
{
    [TreeWindow("替换资源工具")]
    public class ReplaceResouseWindow : OdinEditorWindow
    {
        public Object findObj;
        public Object newObj;
        
        [LabelText("查询结果")]
        public List<Object> result = new List<Object>();
        
        /// <summary>
        /// 替换资源
        /// </summary>
        /// <param name="sourceObj">源Object</param>
        /// <param name="targetObj">目标Object</param>
        [Button("替换资源",ButtonHeight = 30)]
        public void ReplaceResource()
        {
            ResourcesReplace.StartReplace(findObj,newObj);
            AssetDatabase.Refresh();
        }
        [Button("获取引用",ButtonHeight = 30)]
        public bool CheckIsUnUse(Object sourceObj)
        {
            findObj = sourceObj;
            result = ResourcesReplace.GetReferenceResult(sourceObj);
            return result.Count == 0;
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

        [TabGroup("合并资源")]
        [Searchable] public List<List<Texture2D>> NeedMergeResources = new List<List<Texture2D>>();
        [TabGroup("合并资源")] 
        [LabelText("选中目录")] public string DirectoryPath;

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
                    ResourcesReplace.StartReplace(s, sprites[0]);
                }

                //删除只保留第一个
                for (var i = sprites.Length - 1; i > 0; i--)
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(sprites[i]));
                }
            }
            AssetDatabase.Refresh();
        }
        
        [TabGroup("清除资源")]
        public List<Object> ClearResourse = new List<Object>(); // 依赖于资源的物体列表
   
        [TabGroup("清除资源")]
        [Button("获取资源")]
        public void GetUnUseResources()
        {
            var path = GetCurrentAssetDirectory();
            
            var assets = AssetDataBaseGetAllFolderAsset(path)
                .Where(e=>e.GetType()==typeof(Texture2D))
                .Select(e=>e as Texture2D).ToList();
            foreach (var o in assets)
            {
                if (CheckIsUnUse(o))
                {
                    ClearResourse.Add(o);
                }
            }
        
        }
        [TabGroup("清除资源")]
        [Button("清除无用的")]
        public void ClearUnUseResources()
        {
            foreach (var o in ClearResourse)
            {
                var path = AssetDatabase.GetAssetPath(o);
                AssetDatabase.DeleteAsset(path);
                
            }
            AssetDatabase.Refresh();
        }
    }
}