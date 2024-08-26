using System.Collections.Generic;
using System.Linq;
using GameDevKit;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace GameDevKitEditor
{
    [TreeWindow("场景工具")]
    public class SceneWindow : OdinEditorWindow
    {
        [TableList] public List<SceneView> settingsScenes;

        [Searchable] public List<SceneAsset> serchScenes;

        public class SceneView
        {
            public SceneAsset scene;
            public string path;

            [Button("删除场景")]
            public void Delete()
            {
                scene.DestroyAsset();
            }
        }

        protected override void OnEnable()
        {
            settingsScenes = new List<SceneView>();
            settingsScenes = GetAllScene().Select(e =>
                new SceneView()
                {
                    scene = ResourcesKit.LoadAssetAtPath<SceneAsset>(e.path),
                    path = e.path
                }
            ).ToList();

            serchScenes = GetAllScene()
                .Select(e => ResourcesKit.LoadAssetAtPath<SceneAsset>(e.path))
                .ToList();
        }

        static EditorBuildSettingsScene[] GetAllScene()
        {
            string[] finddic = ResourcesKit.GetFindPath("t:scene");
            // 定义 场景数组
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[finddic.Length];
            for (var i1 = 0; i1 < finddic.Length; i1++)
            {
                string scenePath = finddic[i1];
                // 通过scene路径初始化
                scenes[i1] = new EditorBuildSettingsScene(scenePath, true);
            }

            return scenes;
        }

        /// <summary>
        /// 把所有的Scene添加到build里
        /// </summary>
        [MenuItem("FTool/Scene/AddAllScene")]
        [Button]
        static void AddAllScene()
        {
            // 设置 scene 数组
            EditorBuildSettings.scenes = GetAllScene();
        }

        [Button]
        [MenuItem("FTool/Scene/ClearAllScene")]
        static void ClearScene()
        {
            // 定义 场景数组
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[0];
            // 设置 scene 数组
            EditorBuildSettings.scenes = scenes;
        }
    }
}