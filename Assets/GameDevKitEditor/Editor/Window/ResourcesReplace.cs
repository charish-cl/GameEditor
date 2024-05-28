namespace GameDevKitEditor
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;
    using System.Linq;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;

    /// <summary>
    /// 解决项目中 一样的资源（名字或者路径不同）存在两份的问题  （多人做UI出现的问题， 或者美术没有管理好资源）
    /// 如果是要替换资源的话， 那就直接替换好了
    /// 
    /// 以上可以这么操作的基础是，你的Unity项目内的.prefab .Unity 都可以直接用文本开看到数据，而不是乱码（二进制）。这一步很关键，怎么设置呢？
    /// 打开项目Unity编辑器：Edit —-> Project Settings —-> Editor 这样就会调到你的Inspector面板的Editor Settings 
    /// 设置 Asset Serialization 的Mode类型为：Force Text(默认是Mixed); 这样你就能看到你的prefab文件引用了哪些贴图，字体，prefab 等资源了
    /// </summary>
    public class ResourcesReplace
    {
        public static Object _sourceOld;
        public static Object _sourceNew;

        private static string _oldGuid;
        private static string _newGuid;

        private static List<string> withoutExtensions = new List<string>();
        private static string[] files;


        public static void StartReplace(Object old,Object newObj)
        {
            _sourceOld = old;
            _sourceNew = newObj;
            _oldGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_sourceOld));
            _newGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_sourceNew));

            Debug.Log($"{_sourceOld.name} oldGUID = {_oldGuid}  {_sourceNew.name} _newGuid = {_newGuid}");

            if (withoutExtensions == null)
            {
                withoutExtensions = new List<string>();

                withoutExtensions.Add(".unity");

                withoutExtensions.Add(".prefab");

                withoutExtensions.Add(".mat");

                withoutExtensions.Add(".asset");
            }

            Find();
        }

        
        /// <summary>
        /// 查找  并   替换 
        /// </summary>
        private static void Find()
        {
            if (withoutExtensions == null || withoutExtensions.Count == 0)
            {
                withoutExtensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };
            }

            files ??= Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
        
            if (files == null || files.Length == 0)
            {
                Debug.Log("没有找到 筛选的引用");
                return;
            }

            
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                if (Regex.IsMatch(content, _oldGuid))
                {
                    Debug.Log("替换了资源的路径：" + file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));

                    content = content.Replace(_oldGuid, _newGuid);

                    File.WriteAllText(file, content);
                }
                else
                {
                    Debug.Log("查看了的路径：" + file);
                }

            }
    
            AssetDatabase.Refresh();
            Debug.Log("替换结束");
        }

        
        
        private static string GetRelativeAssetsPath(string path)
        {
            return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "")
                .Replace('\\', '/');
        }

        public static List<Object> GetReferenceResult(Object sourceObj)
        {
            List<Object> results = new List<Object>();

            if (withoutExtensions == null || withoutExtensions.Count == 0)
            {
                withoutExtensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };
            }

            files ??= Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
            
            if (files == null || files.Length == 0)
            {
                Debug.Log("没有找到 筛选的引用");
                return null;
            }
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                if (Regex.IsMatch(content, _oldGuid))
                {
                    results.Add(AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
                }
            }
            return results;
        }
    }
}