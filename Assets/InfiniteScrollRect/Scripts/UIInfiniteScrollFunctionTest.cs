using BitCubic.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIInfiniteScrollFunctionTest: MonoBehaviour
{
    [Header("Components Settings")]
    public GameObject Panel;
    public Button BtnPop;
    public Button BtnPush;
    public Button BtnHide;
    public Button BtnStart;
    public Button BtnClear;
    public Toggle TGMoveType1;
    public Toggle TGMoveType2;
    public Toggle TGMoveType3;
    public Text txtCount;
    public RectTransform item;
    [FormerlySerializedAs("Scroll")] public InfiniteScrollView scrollView;

    private Dictionary<int, int> heights = new Dictionary<int, int>();          //index=>height
    private Dictionary<int, string> messages = new Dictionary<int, string>();   //index=>message
    private int count = 0;                                                      //数据数量
    private void Start()
    {
        scrollView.UpdateHeightFunc += UpdateHeightFuncItem;
        scrollView.UpdateDataFunc += OnFillItem;
        BtnPop.onClick.AddListener(BtnPopClick);
        BtnPush.onClick.AddListener(BtnPushClick);
        BtnHide.onClick.AddListener(BtnShowOrHideClick);
        BtnClear.onClick.AddListener(BtnClearClick);
        BtnStart.onClick.AddListener(BtnAutoPushClick);
      
    }

    private bool started = false;
    /// <summary>
    /// 自动增加Item
    /// </summary>
    private void BtnAutoPushClick()
    {
        started = !started;
        if (started)
        {
            InvokeRepeating("AutoPush", 0, 3);
        }
        else
        {
            CancelInvoke();
        }
    }



    private void AutoPush()
    {
        for (int i = 0; i < 10; i++)
        {
            CreateItemData(Random.Range(30, 200));
        }
        scrollView.PushItem(10);
        txtCount.text = count.ToString();
    }

    /// <summary>
    /// 出栈Item点击事件
    /// </summary>
    private void BtnPopClick()
    {
        if (count <= 0) return;
        count -= 1;
        scrollView.PopItem(1);
        heights.Remove(count);
        messages.Remove(count);
        txtCount.text = count.ToString();
    }
   
    /// <summary>
    /// 入栈Item点击事件
    /// </summary>
    private void BtnPushClick()
    {
        CreateItemData(Random.Range(30 , 200));
        scrollView.PushItem(1);
        txtCount.text = count.ToString();
    }
    /// <summary>
    /// Panel显隐点击事件
    /// </summary>
    private void BtnShowOrHideClick()
    {
        Panel.SetActive(!Panel.activeSelf);
    }
    /// <summary>
    /// 清空消息点击事件
    /// </summary>
    private void BtnClearClick()
    {
        scrollView.PopItem(count);
        heights.Clear();
        messages.Clear();
        count = 0;
        txtCount.text = count.ToString();
    }

    StringBuilder sb = new StringBuilder();
    /// <summary>
    /// 生成一条数据
    /// </summary>
    /// <param name="len">数据长度</param>
    private void CreateItemData(int len)
    {
        sb.Clear();
        string str= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        sb = new StringBuilder();
        for (int i = 0; i < len; i++)
        {
            sb.Append(str[Random.Range(0, 62)]);
        }
        item.GetComponentInChildren<Text>().text = sb.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        heights.Add(count, (int)item.sizeDelta.y);
        messages.Add(count, sb.ToString());
        count++;
    }
    /// <summary>
    /// 删除一条数据
    /// </summary>
    /// <param name="index">数据ID</param>
    public void DelItem(int index)
    {
        for (int i = index; i < count-1; i++)
        {
            heights[i] = heights[i + 1];
            messages[i] = messages[i + 1];
        }
        BtnPopClick();
        txtCount.text = count.ToString();
    }
    void OnFillItem(int index, GameObject go)
    {
        go.GetComponentInChildren<Text>().text = messages[index];
        go.name = index.ToString();
    }
  
    int UpdateHeightFuncItem(int index)
    {
        return heights[index];
    }
}
