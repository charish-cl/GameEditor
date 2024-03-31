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
        public string text = "你好";
       
        [Button]
        public static void Test()
        {
            // 工程路径
            string projectPath = Application.dataPath;
            //Packages路径
            string packagesPath = Application.dataPath + "/../Packages";
            
            Debug.Log(projectPath);
            Debug.Log(packagesPath);
        }
        
        [Button("查看场景物体是否有z轴不为0的")]
        public static void CheckZ_IsZero()
        {
            foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
            {
                //在built setting中被勾选的scene            
                if (S.enabled)
                {
                    //得到场景的名称
                    string name = S.path;

                    //打开这个场景
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(name);

                    // 遍历场景中的GameObject
                    foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
                    {
                        if (obj.transform.position.z != 0)
                        {
                            EditorGUIUtility.PingObject(obj);
                            Debug.Log(obj.name);
                        }
                    }
                }
            }

        }
    }
}