using System;
using System.Runtime.CompilerServices;

namespace TEngine
{
    public struct LoadAssetAwaiter : INotifyCompletion
    {
        // 返回LoadAssetAwaiter
        public LoadAssetAwaiter GetAwaiter()
        {
            return this;
        }

        // IsCompleted初始为false, 表示还未完成
        public bool IsCompleted { get; set; }

        public AssetObject Result { get; set; }
        
        // GetResult在异步操作完成后调用
        public AssetObject GetResult()
        {
            return Result;   
        }

        // 设置异步操作完成后执行的回调
        public async void OnCompleted(Action continuation)
        {
            continuation?.Invoke(); // 调用回调方法，通知await操作继续
        }
    }
}