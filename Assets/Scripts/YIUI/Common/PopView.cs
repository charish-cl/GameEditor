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
    public sealed partial class PopView:PopViewBase
    {

        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"PopView Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"PopView Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"PopView OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"PopView OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"PopView OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"PopView OnOpen");
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