namespace GameEditor
{
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;

    namespace GameEditor
    {
        public class TortoiseGitTool : OdinEditorWindow
        {
            private static string ProjectPath => Application.dataPath + "/../";

        
            private static void OpenWindow()
            {
                GetWindow<TortoiseGitTool>().Show();
            }

            [MenuItem("Git工具/Commit _F5")]
            public static void Commit()
            {
                System.Diagnostics.Process.Start("TortoiseGitProc.exe", $"/command:commit /path:{ProjectPath}");
            }
            [MenuItem("Git工具/Push _F6")]
            public static void Push()
            {
                System.Diagnostics.Process.Start("TortoiseGitProc.exe", $"/command:push /path:{ProjectPath}");
            }

            [MenuItem("Git工具/Pull _F7")]
            public static void Pull()
            {
                System.Diagnostics.Process.Start("TortoiseGitProc.exe", $"/command:pull /path:{ProjectPath}");
            }

            public  static string remoteBranch = "origin/oneelk";
            //Merge from remote  branch to local branch
            [MenuItem("Git工具/Merge _F8")]
            public static void Merge()
            {
                System.Diagnostics.Process.Start("TortoiseGitProc.exe", $"/command:merge /fromurl:{remoteBranch} /path:{ProjectPath}");
            }
        
            [MenuItem("Git工具/打开日志 _F9",false,5)]
            static void SVNLog(){
                System.Diagnostics.Process.Start ("TortoiseGitProc.exe", $"/command:log /path:{ProjectPath}");
            }
            public static void MergeAndDeleteLocalBranch()
            {
                //执行命令行
            
            }
        }
    }
}