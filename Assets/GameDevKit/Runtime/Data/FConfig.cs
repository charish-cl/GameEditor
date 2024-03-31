using Sirenix.OdinInspector;
using UnityEngine;

namespace GameDevKit
{
    [CreateAssetMenu]
    public class FConfig:ScriptableObject
    {
        [LabelText("SVN Path")]
        [SerializeField]
        [FolderPath]
        public string SVNProjectPath;
        
        public static FConfig GetConfig()
        {
            return ResourcesKit.LoadAssetAtPath<GameDevKit.FConfig>("Assets/GameDevKit/Config/FConfig.asset") as FConfig;
        }
    }
}