using System.Collections.Generic;

namespace TEngine.UIWindow
{
    public class UIWindowLayer: UILayer<IUIWindow>
    {
        private Queue<UIWindowRecord> windowQueue;
        private Stack<UIWindowRecord> windowHistory;
        
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
           screen.Show();
           if (screen.HideOnForegroundLost)
           {
               
           }
        }

        public override void Show<TProps>(IUIWindow screen, TProps properties)
        {
            screen.Show(properties);
        }

        public override void Hide(IUIWindow screen)
        {
            screen.Hide();
           
           
        }
    }
}