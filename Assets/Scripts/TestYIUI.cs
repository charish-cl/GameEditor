using System;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using YIUI.Common;
using YIUIFramework;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class TestYIUI:MonoBehaviour
    {
        private async void Start()
        {
           YIUILoadDI.LoadAssetFunc = LoadAssetFunc;
           YIUILoadDI.LoadAssetAsyncFunc = LoadAssetAsyFunc;
           SingletonMgr.Initialize();
           await PanelMgr.Inst.ManagerAsyncInit();
           PanelMgr.Inst.OpenPanel<CommonPanel>();
        }

        private async UniTask<(Object, int)> LoadAssetAsyFunc(string arg1, string arg2, Type arg3)
        {
           await UniTask.CompletedTask;

           return LoadAssetFunc(arg1, arg2, arg3);
        }

        private (Object, int) LoadAssetFunc(string arg1, string arg2, Type arg3)
        {
            var path = $"Assets/GameRes/YIUI/{arg1}/Prefabs/{arg2}.prefab"; 
            return (AssetDatabase.LoadAssetAtPath<Object>(path),1);
        }
    }
}