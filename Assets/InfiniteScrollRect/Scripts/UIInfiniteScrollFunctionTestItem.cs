using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfiniteScrollFunctionTestItem : MonoBehaviour
{
    public Button BtnDel;
    public UIInfiniteScrollFunctionTest test;
    private void Start()
    {
        BtnDel.onClick.AddListener(BtnDelClick);
    }

    private void BtnDelClick()
    {
        int index = int.Parse(gameObject.name);
        test.DelItem(index);
    }
}
