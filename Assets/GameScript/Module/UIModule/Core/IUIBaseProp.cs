namespace TEngine
{
    public interface IUIBaseProp
    {
        
    }

    public interface IUIPanelProp : IUIBaseProp
    {
        PanelLayer Priority { get; set; }
    }
    
    public interface IUIWindowProp : IUIBaseProp
    {
        WindowLayer WindowQueueLayer { get; set; }
        bool HideOnForegroundLost { get; set; }
        bool IsPopup { get; set; }
        bool SuppressPrefabProperties { get; set; }
    }
    
}