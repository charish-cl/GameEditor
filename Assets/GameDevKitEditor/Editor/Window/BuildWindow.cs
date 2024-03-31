using GameDevKit;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GameDevKitEditor
{
    [TreeWindow("打包工具")]
    public class BuildWindow:OdinEditorWindow
    {
        [LabelText("是否全屏")]
        public bool isfullscreen;

        [LabelText("输出路径")]
        [FolderPath]
        public string outpath;
        
        [Button("Debug打包")]
        public static void DebugBuild()
        {
            string path = "游戏/dungeon.exe";
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = path;
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.AllowDebugging;
            buildPlayerOptions.options = BuildOptions.Development;
            //添加场景
            //  buildPlayerOptions.scenes = GetScenes();
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                FileKit.OpenDirectory(path.Split('/')[0]);
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }
            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
            
        }
    }
}