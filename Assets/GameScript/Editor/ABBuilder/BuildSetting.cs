using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TEngine
{
    [CreateAssetMenu(fileName = "BuilderSetting", menuName = "TEngine/BuilderSetting", order = 0)]
    public class BuildSetting:SerializedScriptableObject
    {
        [TableList]
        public List<BuildItem> BuildItems = new List<BuildItem>();
    }
}