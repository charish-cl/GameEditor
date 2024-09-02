using System;
using UnityEngine;

namespace TEngine.UIPanel
{
    /// <summary>
    /// TODO:时间注册那里用我写的来处理吧
    /// </summary>
    /// <typeparam name="TProps"></typeparam>
    public abstract class UIPanelController<TProps>:MonoBehaviour, IUIBaseControl
        where TProps : IUIBaseProp
    {
        public string ScreenId { get; set; }
        public bool IsVisible { get; }
        
        [Header("Screen Animations")] 
        [Tooltip("界面显示的动画")] 
        [SerializeField]
        private AniComponent animIn;

        [Tooltip("界面隐藏的动画")] 
        [SerializeField]
        private AniComponent animOut;
        
        public void Show(IUIBaseProp props = null)
        {
            
        }

        public void Hide(bool animate = true)
        {
          
        }

        public Action<IUIBaseControl> InTransitionFinished { get; set; }
        public Action<IUIBaseControl> OutTransitionFinished { get; set; }
        public Action<IUIBaseControl> CloseRequest { get; set; }
        public Action<IUIBaseControl> ScreenDestroyed { get; set; }
        
        /// <summary>
        /// 属性参数设置到界面的时候触发，在SetProperties之后触发，比较安全的能取到值
        /// </summary>
        protected virtual void OnPropertiesSet()
        {
            
        }

        public virtual void OnHide()
        {
            
        }
    }
}