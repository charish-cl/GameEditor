using GameDevKit;
using UnityEditor;
using UnityEngine;

namespace GameDevKitEditor
{
    public static class SelectionUtility
    {
        /// <summary>
        /// 聚焦物体
        /// </summary>
        /// <param name="gameObject"></param>
        public static void LockSelectedPoint(GameObject gameObject)
        {
            
            //类似你在层级窗口中点击了物体,他会自动展开层级
            EditorGUIUtility.PingObject(gameObject);
            //聚焦物体
            Selection.activeGameObject = gameObject;

            SceneView.lastActiveSceneView.FrameSelected();
        }
        
        [MenuItem("FTool/移除场景中所有的空组件")]
        public static void RemoveNullComponent()
        {
            var gos = SceneAsset.FindObjectsOfType<GameObject>();
            for ( int i = 0 ; i < gos.Length ; i ++ )
            {
                RemoveComponent(gos[i]);
            }
        }
        [MenuItem("FTool/移除选中物体以下层级的空组件")]
        public static void SelectionGameObj()
        {
            GameObject[] gos = Selection.gameObjects;
            for ( int i = 0 ; i < gos.Length ; i ++ )
            {
                RemoveComponent(gos[i]);
            }
        }
        [MenuItem("Assets/获取资源类型", false, 12)]
        public static void GetAssetType()
        {
           Debug.Log(Selection.activeObject.GetType());
        }
        [MenuItem("Assets/加载该资源代码(编辑器)", false, 12)]
        public static void LoadSelectResource()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string s = $"var data = FResources.LoadAssetAtPath<{Selection.activeObject.GetType()}>(\"{path}\");";
            SystemHelper.Copy(s);
        }
        [MenuItem("Assets/加载该资源代码", false, 12)]
        public static void LoadSelectRes()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            path = path.Split('.')[0];
            path =  path.Replace("Assets/Resources/", "");
            string s = $"var data = FResources.ResLoad<{Selection.activeObject.GetType()}>(\"{path}\");";
            SystemHelper.Copy(s);
        }
        [MenuItem("Assets/代码生成/添加路径到生成的枚举中", false, 12)]
        public static void AddPath()
        {
            var path = GetSelectionPath();
            var data = ResourcesKit.LoadAssetAtPath<GameDevKit.EnumGenerateData>(
                "Assets/GameDevKit/Config/EnumGenerate/Enum Generate Data.asset");
            foreach ( var dataLi in data.lis )
            {
                if ( dataLi.Contains(path) )
                    return;
            }
            data.lis.Add(path);
            
            EditorUtility.SetDirty(data);
        }
        
        public static string GetSelectionPath()
        {
            return AssetDatabase.GetAssetPath(Selection.activeObject);
        }
        public static void SetSelectionDirty()
        {
            EditorUtility.SetDirty(Selection.activeGameObject);
        }
        public static void RemoveComponent(GameObject component)
        {
            Transform[] transforms = component.GetComponentsInChildren<Transform>(true); //获取全部层级
            for ( int j = 0 ; j < transforms.Length ; j ++ )
            {
                GameObject obj = transforms[j].gameObject;
                if ( GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj) != 0 )
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
                    Debug.Log(obj.name + "已经移除空脚本");
                    EditorUtility.SetDirty(obj);
                }
            }
            AssetDatabase.Refresh();
        }
      
    }
}