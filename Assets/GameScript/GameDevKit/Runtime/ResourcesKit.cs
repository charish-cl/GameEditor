using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameDevKit
{
    //TODO:加载资源感觉还是不够方便
    public class ResourcesKit
    {
        /// <summary>
        ///  利用通配符AssetDatabase.FindAssets获取查找元素的路径
        /// </summary>
        /// <param name="patten"></param>
        /// <returns></returns>
        public static string[] GetFindPath(string patten)
        {
#if UNITY_EDITOR
            var assets = AssetDatabase.FindAssets(patten);
            string[] paths = new string[assets.Length];
            for (var i = 0; i < assets.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(assets[i]);
            }

            return paths;
#endif
            return null;
        }
        public static List<Object> FindByType<T>() where T : Object
        {
#if UNITY_EDITOR
            List<Object> objects = new List<Object>();
            var assets = AssetDatabase.FindAssets($"t:{typeof(T)}");
            string[] paths = new string[assets.Length];
            for (var i = 0; i < assets.Length; i++)
            {
                var asset =AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[i]));
                objects.Add(asset);
            }
            return objects;
#endif
            return null;
        }
        /// <summary>
        /// 利用通配符AssetDatabase.FindAssets获取查找元素的字典
        /// </summary>
        /// <param name="patten">比如  t:prefab</param>
        /// <returns></returns>
        public static Dictionary<string, GameObject> GetFindDic(string patten)
        {
#if UNITY_EDITOR
            Dictionary<string, GameObject> finddic = new Dictionary<string, GameObject>();
            var str = AssetDatabase.FindAssets(patten);
            foreach (var s in str)
            {
                var p = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(s));
                if (!finddic.ContainsKey(p.name))
                    finddic.Add(p.name, p);
            }

            return finddic;
#endif
            return null;
        }
        

        public static T LoadAssetAtPath<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<T>(path);
#endif
            return null;
        }

        public static Object LoadAssetAtPath(string path)
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath(path,typeof(Object));
#endif
            return null; 
        }
        
        //TODO:默认便利所有层
        /// <summary>
        /// 场景,瓦片一些资源无法加载,默认加载底层
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="option"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> LoadAllFile<T>(string fullPath, SearchOption option = SearchOption.AllDirectories)
            where T : Object
        {
            List<T> list = new List<T>();
            var files = FileKit.GetAllSubFile(fullPath,option);
            foreach (var file in files)
            {
               list.Add(LoadAssetAtPath<T>(file));;
            }
            return list;
        }
        public static void ForEachFile(string fullPath, Action<GameObject> action,SearchOption option = SearchOption.AllDirectories)
        {
            var allFile = LoadAllFile<GameObject>(fullPath,option);
            foreach (var gameObject in allFile)
            {
                action.Invoke(gameObject);
            }
        }
        /// <summary>
        /// AssetBundle
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T AssetLoad<T>(string path) where T : UnityEngine.Object
        {
            return AssetBundle.LoadFromFile(path).LoadAsset<T>(path);
        }
        public static T ResLoad<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }
        /// <summary>
        /// ResLoadAll
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isSub">是否遍历子目录</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] ResLoadAll<T>(string path,bool isSub=false) where T : UnityEngine.Object
        {
            if(!isSub)
                return Resources.LoadAll<T>(path);
            var list = new List<T>();
            
            var directories = Directory.GetDirectories(path);
            list.AddRange(Resources.LoadAll<T>(path.GetResourecesPath()));
            foreach (var directory in directories)
            {
                list.AddRange( Resources.LoadAll<T>(directory.GetResourecesPath()));
            }
            return list.ToArray();
        }
        /// <summary>
        /// 从一个png中加载切分后的精灵
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<Sprite> LoadFromSingleSprite(string path = null)
        {
            List<Sprite> sprites = new List<Sprite>();
#if UNITY_EDITOR
            if ( path == null )
            {
                var go=Selection.activeObject;
                Debug.Log(go + " "+go.GetType());
                var sp=AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(go.GetInstanceID()));
                sprites.AddRange(sp.Select(o => o as Sprite));
            }
            else
            {
                var sp=AssetDatabase.LoadAllAssetsAtPath(path);
                sprites.AddRange(sp.Select(o => o as Sprite));
            }
#endif
            sprites.RemoveAt(0);//把第一个移除，那个是大图集
            Debug.Log("加载图集完成-数量:"+sprites.Count);
            return sprites;
        }
        
       
    }
    
}