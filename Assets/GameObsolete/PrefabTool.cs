using System;
using System.Collections.Generic;
using GameDevKit;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameDevKit
{
    //TODO：这个也属实没用，狂怒传说当时因为不了解才会做这种操作
    public class PrefabTool
    {
        /// <summary>
        /// 批量操作预制体
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        public static void BatchOperation(string path,Action<GameObject> action)
        {
            var gos=ResourcesKit.LoadAllFile<GameObject>(path);
            for (var i = 0; i < gos.Count; i++)
            {
                GameObject go= PrefabUtility.InstantiatePrefab(gos[i]) as GameObject;
                action.Invoke(go);
                PrefabUtility.SaveAsPrefabAsset(go,AssetDatabase.GetAssetPath(gos[i]));
                Object.DestroyImmediate(go);
            }
            AssetDatabase.Refresh();
        }     
        public static void BatchOperation(List<GameObject> gos,Action<GameObject> action)
        {
            for (var i = 0; i < gos.Count; i++)
            {
                GameObject go= PrefabUtility.InstantiatePrefab(gos[i]) as GameObject;
                action.Invoke(go);
               
                PrefabUtility.SaveAsPrefabAsset(go,AssetDatabase.GetAssetPath(gos[i]));
                Object.DestroyImmediate(go);
            }
            AssetDatabase.Refresh();
        }
        
        /// <summary>
        /// 批量生成子文件所有预制体
        /// </summary>
        /// <param name="savepath">保存路径</param>
        /// <param name="sourcepath">源路径</param>
        /// <param name="patten">匹配模式(后缀名)</param>
        public static void BatchGenerate(string savepath,string sourcepath,string patten, GameObject template,Action<GameObject> action)
        {
            var allSubFile= FileKit.GetAllSubFile<GameObject>(sourcepath,patten);
            var instantiate = UnityEditor.Editor.Instantiate(template);
            foreach (var gameObject in allSubFile)
            {
                action.Invoke(gameObject);
                PrefabUtility.SaveAsPrefabAsset(instantiate,savepath+instantiate.name+".prefab");
            }
            UnityEditor.Editor.DestroyImmediate(instantiate);
            AssetDatabase.Refresh();
        }
        [MenuItem("GameObject/替换预制体", false, -2)]
        public static void BatchReplace()
        {
            var gos=Selection.activeGameObject;
            
            Dictionary<string, GameObject> finddic = ResourcesKit.GetFindDic("t:prefab");
            GameObject temppar = new GameObject();
            
            gos.transform.ForEachChild((t) =>
            {
                // go.transform.SetParent(temppar.transform);
                // var temp=go.CreateObject("Empty");
                // temp.transform.position = dec.transform.position;
                // GameObject s=null;
                //     var name = t.gameObject.GetRawName().TrimEnd();
                //
                // if (finddic.ContainsKey(name))
                // {
                //     s= finddic[name];
                //     GameObject prefab= (GameObject)PrefabUtility.InstantiatePrefab(s);
                //     prefab.transform.SetParent(temp.transform);
                //     prefab.transform.position = t.transform.position;
                // }
                //     Debug.Log(name+"---------"+s);
                //     temp.name = dec.name;
                //     
                // Object.DestroyImmediate(dec.gameObject);
                // PrefabUtility.SaveAsPrefabAsset(go,AssetDatabase.GetAssetPath(tempGO))
                
            });
            Object.DestroyImmediate(temppar.gameObject);
        }
        
        [MenuItem("Assets/预制体/移除预制体的空脚本", false, 12)]
        public static void RemoveSelectDirNullComponent()
        {
            // Debug.Log(Selection.activeContext);
            // Debug.Log(FResources.GetAssetPath(Selection.activeObject));
            BatchOperation(Selection.activeObject.GetAssetPath(), e =>
            {
                //SelectionUtility.RemoveComponent(e);
            });
        }
    }
}