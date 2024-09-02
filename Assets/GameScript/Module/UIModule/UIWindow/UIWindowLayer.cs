using System;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine.UIWindow
{
    /// <summary>
    /// 这里感觉就是框架的精华所在，队列
    /// </summary>
    public class UIWindowLayer: UILayer<IUIWindow>
    {
        private Queue<UIWindowRecord> windowQueue;//用于显示之前的窗口
        private Stack<UIWindowRecord> windowHistory;//用于显示之后的窗口


        private HashSet<IUIBaseControl> translationScreenSet ;//用于过渡动画的窗口集合
        
        //切换界面的时候要触发的事件
        public event Action OnAddTranslationScreen;
        public event Action OnRemoveTranslationScreen;
        
        
        public IUIWindow CurrentWindow { get; private set; }

        public void Init()
        {
            windowQueue = new Queue<UIWindowRecord>();
            windowHistory = new Stack<UIWindowRecord>();
            translationScreenSet = new HashSet<IUIBaseControl>();
        }
        
        
        public void AddTranslationScreen(IUIBaseControl control)
        {
            translationScreenSet.Add(control);
            OnAddTranslationScreen?.Invoke();
        }
        public void RemoveTranslationScreen(IUIBaseControl control)
        {
            translationScreenSet.Remove(control);
            OnRemoveTranslationScreen?.Invoke();
        }


        // private void ShowUIImp(Type type, bool isAsync, params System.Object[] userDatas)
        // {
        //     string windowName = type.FullName;
        //
        //     // 如果窗口已经存在
        //     if (IsContains(windowName))
        //     {
        //         UIWindow window = GetWindow(windowName);
        //         Pop(window); //弹出窗口
        //         Push(window); //重新压入
        //         window.TryInvoke(OnWindowPrepare, userDatas);
        //     }
        //     else
        //     {
        //         UIWindow window = CreateInstance(type);
        //         Push(window); //首次压入
        //         window.InternalLoad(window.AssetName, OnWindowPrepare, isAsync, userDatas).Forget();
        //     }
        // }
        //
        // private async UniTask<UIWindow> ShowUIAwaitImp(Type type, bool isAsync, params System.Object[] userDatas)
        // {
        //     string windowName = type.FullName;
        //
        //     // 如果窗口已经存在
        //     if (IsContains(windowName))
        //     {
        //         UIWindow window = GetWindow(windowName);
        //         Pop(window); //弹出窗口
        //         Push(window); //重新压入
        //         window.TryInvoke(OnWindowPrepare, userDatas);
        //         return window;
        //     }
        //     else
        //     {
        //         UIWindow window = CreateInstance(type);
        //         Push(window); //首次压入
        //         window.InternalLoad(window.AssetName, OnWindowPrepare, isAsync, userDatas).Forget();
        //         float time = 0f;
        //         while (!window.IsLoadDone)
        //         {
        //             time += Time.time;
        //             if (time > 60f)
        //             {
        //                 break;
        //             }
        //             await UniTask.Yield();
        //         }
        //         return window;
        //     }
        // }

        public override void Show(IUIWindow screen)
        {
            Show<IUIWindowProp>(screen, null);
        }
        public override void Show<TProps>(IUIWindow screen, TProps properties)
        {
            IUIWindowProp windowProp = (IUIWindowProp)properties;
            if (ShouldEnqueue(screen, windowProp))
            {
                EnqueueWindow(screen, windowProp);
            }
            else
            {
                DoShow(screen, windowProp);
            }
            
        }

        private void DoShow(IUIWindow screen, IUIWindowProp windowProp)
        {
            DoShow(new UIWindowRecord(screen, windowProp));
        }
        private void DoShow(UIWindowRecord windowEntry) {
            if (CurrentWindow == windowEntry.UIWindow) {
                Debug.LogWarning(
                    string.Format(
                        "[WindowUILayer] The requested WindowId ({0}) is already open! This will add a duplicate to the " +
                        "history and might cause inconsistent behaviour. It is recommended that if you need to open the same" +
                        "screen multiple times (eg: when implementing a warning message pop-up), it closes itself upon the player input" +
                        "that triggers the continuation of the flow."
                        , CurrentWindow.ScreenId));
            }
            //先把当前窗口压入历史 传进来的windowEntry也不是小窗口
            else if (CurrentWindow != null
                     && CurrentWindow.HideOnForegroundLost
                     && !windowEntry.UIWindowProp.IsPopup) {
                CurrentWindow.Hide();
            }

            windowHistory.Push(windowEntry);
            AddTranslationScreen(windowEntry.UIWindow);

            if (windowEntry.UIWindow.IsPopup) {
                //打开暗黑层
                // priorityParaLayer.DarkenBG();
            }

            windowEntry.Show();

            CurrentWindow = windowEntry.UIWindow;
        }
        public void EnqueueWindow<TProp>(IUIWindow controller, TProp windowProp) where TProp : IUIBaseProp
        {
            windowQueue.Enqueue(new UIWindowRecord(controller,(IUIWindowProp)windowProp));
        }
        private bool ShouldEnqueue(IUIWindow controller, IUIWindowProp windowProp) {
            //队列里没元素，直接显示就行了，不用队列了
            if (CurrentWindow == null && windowQueue.Count == 0) {
                return false;
            }
            
            if (windowProp != null && windowProp.SuppressPrefabProperties) {
                return windowProp.WindowQueueLayer != WindowLayer.ForceForeground;
            }
            //看是不是直接打开的
            if (controller.WindowLayer != WindowLayer.ForceForeground) {
                return true;
            }

            return false;
        }
        public void ShowPreviousInHistory()
        {
            if (windowHistory.Count > 0)
            {
                UIWindowRecord record = windowQueue.Dequeue();
                DoShow(record);
            }
        }
        public void ShowNextInqQueue()
        {
            if (windowQueue.Count > 0)
            {
                UIWindowRecord record = windowHistory.Pop();
                DoShow(record);
            }
        }
        /// <summary>
        /// 隐藏后，如果有队列，显示下一个，如果没有队列，显示上一个
        /// </summary>
        /// <param name="screen"></param>
        public override void Hide(IUIWindow screen)
        {
            screen.Hide();
            if (CurrentWindow == screen)
            {
                windowHistory.Pop();
                AddTranslationScreen(screen);
                screen.Hide();
                CurrentWindow = null;
                //先看队列，再看history
                if (windowQueue.Count > 0)
                {
                    ShowNextInqQueue();
                }
                else if (windowHistory.Count > 0)
                {
                    ShowPreviousInHistory();
                }
           
            }
            else
            {
                Debug.LogWarning(
                    string.Format(
                        "[WindowUILayer] The requested WindowId ({0}) is not the current window! This might cause inconsistent " +
                        "behaviour. It is recommended that if you need to hide a screen, it should be the current one."
                        , screen.ScreenId));
            }
        }

        public void ShowWindowById(string windowId, IUIWindowProp uiWindowProp)
        {
            if (!registeredScreens.TryGetValue(windowId,out IUIWindow window))
            {
                throw new System.Exception("[UIWindowLayer] windowId not found");
            }
            
            Show(window, uiWindowProp);
        }
        
    }
}