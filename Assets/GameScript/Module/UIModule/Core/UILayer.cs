using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public abstract class UILayer<TUIBase> : MonoBehaviour where TUIBase : IUIBaseControl {
        protected Dictionary<string, TUIBase> registeredScreens;
          /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="screen">界面类型参数</param>
        public abstract void Show(TUIBase screen);

        /// <summary>
        /// 显示一个界面，带一些参数
        /// </summary>
        /// <param name="screen">界面类型参数</param>
        /// <param name="properties">属性参数</param>
        /// <typeparam name="TProps">属性类型</typeparam>
        public abstract void Show<TProps>(TUIBase screen, TProps properties) where TProps : IUIBaseProp;

        /// <summary>
        /// 隐藏界面
        /// </summary>
        /// <param name="screen">界面类型参数</param>
        public abstract void Hide(TUIBase screen);

        /// <summary>
        /// 初始化Layer层
        /// </summary>
        public virtual void Initialize() {
            registeredScreens = new Dictionary<string, TUIBase>();
        }

        /// <summary>
        /// 传进来的界面当做层的子节点
        /// </summary>
        /// <param name="controller">界面的controller</param>
        /// <param name="screenTransform">界面节点</param>
        public virtual void ReparenTUIBase(IUIBaseControl controller, Transform screenTransform) {
            screenTransform.SetParent(transform, false);
        }

        /// <summary>
        /// 注册界面的controller带上明确的界面id
        /// </summary>
        /// <param name="screenId">界面id</param>
        /// <param name="controller">界面controller</param>
        public void RegisterScreen(string screenId, TUIBase controller) {
            if (!registeredScreens.ContainsKey(screenId)) {
                AddUI(screenId, controller);
            }
            else {
                Debug.LogError("[AUILayerController] Screen controller already registered for id: " + screenId);
            }
        }

        /// <summary>
        /// 根据id取消注册界面的controller
        /// </summary>
        /// <param name="screenId">界面id</param>
        /// <param name="controller">被取消的界面controller</param>
        public void UnregisterScreen(string screenId, TUIBase controller) {
            if (registeredScreens.ContainsKey(screenId)) {
                RemoveUI(screenId, controller);
            }
            else {
                Debug.LogError("[AUILayerController] Screen controller not registered for id: " + screenId);
            }
        }

        /// <summary>
        /// 根据id去找界面的controller,并且显示出来
        /// </summary>
        /// <param name="screenId">界面Id</param>
        public void ShowById(string screenId) {
            TUIBase ctl;
            if (registeredScreens.TryGetValue(screenId, out ctl)) {
                Show(ctl);
            }
            else {
                Debug.LogError("[AUILayerController] Screen ID " + screenId + " not registered to this layer!");
            }
        }

        /// <summary>
        /// 根据界面id显示具体的controller,带上具体的属性参数
        /// </summary>
        /// <param name="screenId">界面id</param>
        /// <param name="properties">属性参数</param>
        /// <typeparam name="TProps">属性类型</typeparam>
        public void ShoById<TProps>(string screenId, TProps properties) where TProps : IUIBaseProp {
            TUIBase ctl;
            if (registeredScreens.TryGetValue(screenId, out ctl)) {
                Show(ctl, properties);
            }
            else {
                Debug.LogError("[AUILayerController] Screen ID " + screenId + " not registered!");
            }
        }

        /// <summary>
        /// 根据id隐藏界面
        /// </summary>
        /// <param name="screenId">界面id</param>
        public void HideById(string screenId) {
            TUIBase ctl;
            if (registeredScreens.TryGetValue(screenId, out ctl)) {
                Hide(ctl);
            }
            else {
                Debug.LogError("[AUILayerController] Could not hide Screen ID " + screenId + " as it is not registered to this layer!");
            }
        }

        /// <summary>
        /// 根据id看是否注册了
        /// </summary>
        /// <param name="screenId">界面id</param>
        public bool IsRegistered(string screenId) {
            return registeredScreens.ContainsKey(screenId);
        }
        
        /// <summary>
        /// 隐藏所有界面
        /// </summary>
        /// <param name="shouldAnimateWhenHiding">隐藏的时候是否需要动画</param>
        public virtual void HideAll(bool shouldAnimateWhenHiding = true) {
            foreach (var kv in registeredScreens) {
                kv.Value.Hide(shouldAnimateWhenHiding);
            }
        }
        //TODO: 这里可以配合预加载,游戏初始化的时候预加载指定页面
        protected virtual void AddUI(string screenId, TUIBase controller) {
            controller.ScreenId = screenId;
            registeredScreens.Add(screenId, controller);
            controller.ScreenDestroyed += OnScreenDestroyed;
        }

        protected virtual void RemoveUI(string screenId, TUIBase controller) {
            controller.ScreenDestroyed -= OnScreenDestroyed;
            registeredScreens.Remove(screenId);
        }

        private void OnScreenDestroyed(IUIBaseControl screen) {
            if (!string.IsNullOrEmpty(screen.ScreenId)
                && registeredScreens.ContainsKey(screen.ScreenId)) {
                UnregisterScreen(screen.ScreenId, (TUIBase) screen);
            }
        }
    }
}