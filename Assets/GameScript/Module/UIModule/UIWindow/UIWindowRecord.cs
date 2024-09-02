namespace TEngine
{
    /// <summary>
    /// UIWindow窗口记录,保存窗口和参数,方便后续恢复
    /// </summary>
    public struct UIWindowRecord
    {
        public readonly IUIWindow UIWindow { get;  }
        public readonly IUIWindowProp UIWindowProp { get; }

        public UIWindowRecord(IUIWindow uiWindow, IUIWindowProp uiWindowProp)
        {
            UIWindow = uiWindow;
            UIWindowProp = uiWindowProp;
        }

        public UIWindowRecord(IUIWindow uiWindow) : this()
        {
            UIWindow = uiWindow;
        }

        public void Show() {
            UIWindow.Show(UIWindowProp);
        }
    }
}