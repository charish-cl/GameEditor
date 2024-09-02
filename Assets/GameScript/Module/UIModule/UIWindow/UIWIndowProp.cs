using UnityEngine;

namespace TEngine
{
    //这里继承的目的是，让子类继承的时候不再需要重复写一遍WindowQueueLayer、HideOnForegroundLost、IsPopup、SuppressPrefabProperties这四个属性
    public class UIWIndowProp:IUIWindowProp
    {
        [SerializeField] 
        protected bool hideOnForegroundLost = true;

        [SerializeField] 
        protected WindowLayer windowQueuePriority = WindowLayer.ForceForeground;

        [SerializeField]
        protected bool isPopup = false;
        
        /// <summary>  
        /// 如果另一个窗口已经打开，此窗口应该如何表现？  
        /// </summary>  
        /// <value>Force Foreground 会立即打开它，Enqueue 会将它排队，以便在当前窗口关闭后立即打开。</value>
        public WindowLayer WindowQueueLayer {
            get { return windowQueuePriority; }
            set { windowQueuePriority = value; }
        }
        

        /// <summary>
        /// 当其他窗口被置前的时候，自己是否隐藏
        /// </summary>
        public bool HideOnForegroundLost {
            get { return hideOnForegroundLost; }
            set { hideOnForegroundLost = value; }
        }

        /// <summary>
        ///当在Open()调用中传递属性时，是否应覆盖在viewPrefab中配置的属性
        /// </summary>
        public bool SuppressPrefabProperties { get; set; }

        /// <summary>
        /// 弹出窗口在它们后面显示一个黑色背景，并在所有其他窗口的前面显示
        /// </summary>
        public bool IsPopup {
            get { return isPopup; }
            set { isPopup = value; }
        }
    }
}