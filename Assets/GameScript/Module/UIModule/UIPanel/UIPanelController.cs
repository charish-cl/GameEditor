using System;
using UnityEngine;

namespace TEngine.UIPanel
{
    public abstract class UIPanelController<TProps>:MonoBehaviour, IUIBaseControl
        where TProps : IUIBaseProp
    {
        public string ScreenId { get; set; }
        
        
        public bool IsVisible { get; }
        
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
    }
}