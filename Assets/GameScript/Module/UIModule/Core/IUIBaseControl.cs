using System;

namespace TEngine
{
    public interface IUIBaseControl
    {
        string ScreenId { get; set; }
        bool IsVisible { get; }

        void Show(IUIBaseProp props = null);
        void Hide(bool animate = true);

        /// <summary>
        /// 进入动画完成
        /// </summary>
        Action<IUIBaseControl> InTransitionFinished { get; set; }
        
        /// <summary>
        /// 弹出动画完成
        /// </summary>
        Action<IUIBaseControl> OutTransitionFinished { get; set; }
        Action<IUIBaseControl> CloseRequest { get; set; }
        Action<IUIBaseControl> ScreenDestroyed { get; set; }
    }


    public interface IUIWindow : IUIBaseControl
    {
        /// <summary>
        /// 被其他页面覆盖是否自身隐藏
        /// </summary>
        bool HideOnForegroundLost { get; }
        
        /// <summary>
        /// 是否是弹窗
        /// </summary>
        bool IsPopup { get; }
        
        WindowLayer WindowLayer { get; }
    }
    /// <summary>
    /// 枚举类型，用于定义窗口在打开时、在历史记录和队列中的行为
    /// </summary>
    public enum WindowLayer {
        //关闭其他所有的
        ForceForeground = 0,
        
        //老老实实放到队列里的
        Enqueue = 1,
    }
    public interface IUIPanel : IUIBaseControl
    {
        PanelLayer PanelPriority { get; }
    }
    
  
}