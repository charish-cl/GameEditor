using System.Linq;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using UnityEngine;

namespace GameDevKit.Utility
{
    
    public class SimpleUnit : MonoBehaviour
    {
        //运动相关
        [TabGroup("Move")]
        public float speed = 5;
        
        //tag相关

        #region Tag
        //自己的Tag,OnValueChanged会在值改变时调用
        [OnValueChanged("OnSelfTagChanged")]
        [TabGroup("Tag")]
        [ValueDropdown("GetTags")]
        public string selfTag;
#if UNITY_EDITOR
        
        [TabGroup("Tag")]
        protected string[] GetTags()
        {
            return InternalEditorUtility.tags.Distinct().ToArray();
        }
        [TabGroup("Tag")]
        [Button("Add Tag", ButtonSizes.Large)]
        protected void AddTag(string tag)
        {
            InternalEditorUtility.AddTag(tag);
        }
        
        //自己的Tag改变时调用
        protected void OnSelfTagChanged()
        {
            gameObject.tag = selfTag;
        }
     
#endif
       
       
        #endregion
    }
}