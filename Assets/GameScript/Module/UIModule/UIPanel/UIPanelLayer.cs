using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{   /// <summary>
    /// 这个Layer层是控制面板的
    /// 面板是界面的一种，没有历史记录，没有队列,
    /// 就是简单的显示在界面中
    /// 比如说体力槽，小地图这种常驻的
    /// </summary>
    public class UIPanelLayer: UILayer<IUIPanel>
    {
        public UIGroupList groupList;
        public override void ReparenUIBase(IUIBaseControl controller, Transform screenTransform)
        {
            var panel = controller as IUIPanel;
            if (panel != null)
            {
                //这里因为UIPanel分层了，所以需要重新设置一下层级关系
                ReparentUIPanel(panel, screenTransform);
                return;
            }
            else
            {
                base.ReparenUIBase(controller, screenTransform);
            }

        }

        private void ReparentUIPanel(IUIPanel panel, Transform screenTransform)
        {
            //这里需要重新设置一下层级关系
            if (!groupList.ParaLayerLookup.TryGetValue(panel.PanelPriority,out var layer))
            {
                screenTransform.SetParent(screenTransform, false);
            }
            throw new System.Exception("UIPanelLayer.ReparentUIPanel: 未找到层级");
        }

        /// <summary>
        /// Panel面板是否存在且可见？
        /// </summary>
        /// <param name="panelId"></param>
        /// <returns></returns>
        public bool IsPanelVisible(string panelId)
        {
            return registeredScreens.ContainsKey(panelId)&&registeredScreens[panelId].IsVisible; 
        }
        
        public override void Show(IUIPanel screen)
        {
            screen.Show();
        }

        public override void Show<TProps>(IUIPanel screen, TProps properties)
        {
            screen.Show(properties);
        }

        public override void Hide(IUIPanel screen)
        {
            screen.Hide();
        }
    }
}