using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Common
{

    public enum ECommonPanelViewEnum
    {
        MainView = 1,
        PopView = 2,
    }

    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class CommonPanelBase:BasePanel
    {
        public const string PkgName = "Common";
        public const string ResName = "CommonPanel";
        
        public override EWindowOption WindowOption => EWindowOption.None;
        public override EPanelLayer Layer => EPanelLayer.Panel;
        public override EPanelOption PanelOption => EPanelOption.None;
        public override EPanelStackOption StackOption => EPanelStackOption.VisibleTween;
        public override int Priority => 0;
        protected UIEventP0 u_EventCloseEvent { get; private set; }
        protected UIEventHandleP0 u_EventCloseEventHandle { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_EventCloseEvent = EventTable.FindEvent<UIEventP0>("u_EventCloseEvent");
            u_EventCloseEventHandle = u_EventCloseEvent.Add(OnEventCloseEventAction);

        }

        protected sealed override void UnUIBind()
        {
            u_EventCloseEvent.Remove(u_EventCloseEventHandle);

        }
     
        protected virtual void OnEventCloseEventAction(){}
   
   
    }
}