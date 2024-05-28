using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitCubic.UI
{
    public class TestGridScrollView:MonoBehaviour
    {
        public GridInfiniteScrollView scrollView;
        private void Start()
        {
            scrollView.UpdateHeightFunc += UpdateHeightFuncItem;
            scrollView.UpdateDataFunc += OnFillItem;
            
            
            scrollView.PushItem(100);
        }
        void OnFillItem(int index, GameObject go)
        {
           go.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
            go.name = index.ToString();
        }
  
        int UpdateHeightFuncItem(int index)
        {
            return 100;
        }
    }
}