namespace TEngine
{
    public interface IUIBaseProp
    {
        
    }

    public interface IUIPanelProp : IUIBaseProp
    {
        PanelPriority Priority { get; set; }
    }
    
    public interface IUIWindowProp : IUIBaseProp
    {
        WindowPriority WindowQueuePriority { get; set; }
        bool HideOnForegroundLost { get; set; }
        bool IsPopup { get; set; }
        bool SuppressPrefabProperties { get; set; }
    }
    
}