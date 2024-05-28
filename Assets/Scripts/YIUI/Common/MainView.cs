using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Common
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.5.28
    /// </summary>
    public sealed partial class MainView:MainViewBase
    {

        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"MainView Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"MainView Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"MainView OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"MainView OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"MainView OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"MainView OnOpen");
            return true;
        }

        protected override async UniTask<bool> OnOpen(ParamVo param)
        {
            return await base.OnOpen(param);
        }
        
        #endregion

        #region Event开始


        #endregion Event结束

    }
}