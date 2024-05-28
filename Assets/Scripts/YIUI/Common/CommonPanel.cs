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
    public sealed partial class CommonPanel:CommonPanelBase
    {
    
        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"CommonPanel Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"CommonPanel Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"CommonPanel OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"CommonPanel OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"CommonPanel OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"CommonPanel OnOpen");
            return true;
        }

        protected override async UniTask<bool> OnOpen(ParamVo param)
        {
            return await base.OnOpen(param);
        }
        
        #endregion

        #region Event开始


       
        protected override void OnEventCloseEventAction()
        {
            Close();
        }
        #endregion Event结束

    }
}