using TEngine.UIPanel;
using TEngine.UIWindow;

namespace TEngine
{
    public sealed class UIModule: Module
    {
        public  IUIHelper UIHelper { get; set; }
        public IResourceManager ResourceManager { get; set; }
        
        public UIPanelLayer UIPanelLayer { get; set; }
        public UIWindowLayer UIWindowLayer { get; set; }
        
        
        protected override void Awake()
        {
            base.Awake();
            ResourceManager = ModuleImpSystem.GetModule<IResourceManager>();
        }

        public void OpenWindow<T>(string windowId,IUIWindowProp uiWindowProp)
        {
            UIWindowLayer.ShowWindowById(windowId,uiWindowProp);
        }

        public void RegisterUI(string screenId, IUIBaseControl uiBaseControl)
        {
            var window = uiBaseControl as IUIWindow;
            if (window!= null)
            {
                UIWindowLayer.RegisterScreen(screenId, window);
            }
         
        }
    }

    internal class UIModuleImp : ModuleImp
    {
        
        // private void InternalOpenUIForm(int serialId, string uiFormAssetName, UIGroup uiGroup, object uiFormInstance, bool pauseCoveredUIForm, bool isNewInstance, float duration, object userData)
        // {
        //     try
        //     {
        //         IUIForm uiForm = m_UIFormHelper.CreateUIForm(uiFormInstance, uiGroup, userData);
        //         if (uiForm == null)
        //         {
        //             throw new GameFrameworkException("Can not create UI form in UI form helper.");
        //         }
        //
        //         uiForm.OnInit(serialId, uiFormAssetName, uiGroup, pauseCoveredUIForm, isNewInstance, userData);
        //         uiGroup.AddUIForm(uiForm);
        //         uiForm.OnOpen(userData);
        //         uiGroup.Refresh();
        //
        //         if (m_OpenUIFormSuccessEventHandler != null)
        //         {
        //             OpenUIFormSuccessEventArgs openUIFormSuccessEventArgs = OpenUIFormSuccessEventArgs.Create(uiForm, duration, userData);
        //             m_OpenUIFormSuccessEventHandler(this, openUIFormSuccessEventArgs);
        //             ReferencePool.Release(openUIFormSuccessEventArgs);
        //         }
        //     }
        //     catch (Exception exception)
        //     {
        //         if (m_OpenUIFormFailureEventHandler != null)
        //         {
        //             OpenUIFormFailureEventArgs openUIFormFailureEventArgs = OpenUIFormFailureEventArgs.Create(serialId, uiFormAssetName, uiGroup.Name, pauseCoveredUIForm, exception.ToString(), userData);
        //             m_OpenUIFormFailureEventHandler(this, openUIFormFailureEventArgs);
        //             ReferencePool.Release(openUIFormFailureEventArgs);
        //             return;
        //         }
        //
        //         throw;
        //     }
        // }
        internal override void Shutdown()
        {
            
        }
    }
}