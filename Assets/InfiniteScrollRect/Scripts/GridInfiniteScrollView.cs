using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace BitCubic.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    public class GridInfiniteScrollView : SerializedMonoBehaviour
    {
        [Header("Padding Settings")] [OnValueChanged("EditorUpdate")]
        public int TopPadding = 0;

        [OnValueChanged("EditorUpdate")] public int BottomPadding = 0;
        float LeftSpace = 0;
        [OnValueChanged("EditorUpdate")] public Vector2 Space;

        [Header("Item Settings")] public GameObject ItemPrefab;
        [OnValueChanged("EditorUpdate")] public int Columns = 2;
        [OnValueChanged("EditorUpdate")] public int Rows = 2;

        [HideInInspector] public Func<int, int> UpdateHeightFunc; // 获取元素高度的回调：传入序号，返回高度

        [HideInInspector]
        public Action<int, GameObject> UpdateDataFunc = delegate { }; // 填充数据回调：传入序号和Item的GameObject，无返回

        private Dictionary<int, float> _heights;
        private Dictionary<int, Vector2> _positions; // index->position
        private int _count;

        private ScrollRect _scroll;
        private RectTransform _content;
        private GameObject[] _items;
        private RectTransform[] _rects;
        private Vector2 _viewSize;

        float itemWidth;
        float itemHeight;
        [OnValueChanged("EditorSetLayout")] public bool isVertical = true; // 是否是垂直滚动

        public void EditorSetLayout()
        {
            _scroll = GetComponent<ScrollRect>();
            _content = _scroll.content;
            _scroll.vertical = isVertical;
            _scroll.horizontal = !isVertical;

            Debug.Log("231321");
            if (isVertical)
            {
                _content.anchorMin = new Vector2(0, 1);
                _content.anchorMax = new Vector2(1, 1);
                _content.pivot = new Vector2(0, 1);
            }
            else
            {
                _content.anchorMin = new Vector2(0, 0);
                _content.anchorMax = new Vector2(0, 1);
                _content.pivot = new Vector2(0, 1);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _scroll = GetComponent<ScrollRect>();
            _content = _scroll.content;
            var rect = _scroll.viewport.rect;
            _viewSize = new Vector2(rect.width, rect.height);
            _heights = new Dictionary<int, float>();
            _positions = new Dictionary<int, Vector2>();
            _scroll.onValueChanged.AddListener(UpdateItemData);

            itemWidth = ItemPrefab.GetComponent<RectTransform>().rect.width;
            itemHeight = ItemPrefab.GetComponent<RectTransform>().rect.height;

            var itemRect = ItemPrefab.GetComponent<RectTransform>().sizeDelta;
            LeftSpace = (_viewSize.x - itemRect.x * Columns - (Columns - 1) * Space.x) / 2;

            isVertical = _scroll.vertical && !_scroll.horizontal; // 只允许一个方向滚动
        }

        private void InitItems()
        {
            // 计算需要的数量
            _count = (isVertical ? (int)(_viewSize.y / itemHeight * Columns) : (int)(_viewSize.x / itemWidth * Columns)) + 4 * Columns;
            Debug.Log($"初始生成数量 {_count}");

            // 初始化数组
            _items = new GameObject[_count];
            _rects = new RectTransform[_count];

            // 获取现有的子物体并补充不足的物体
            for (int i = 0; i < _count; i++)
            {
                GameObject item;
                RectTransform rect;

                if (i < _content.childCount)
                {
                    item = _content.GetChild(i).gameObject;
                    rect = item.GetComponent<RectTransform>();
                }
                else
                {
                    item = Instantiate(ItemPrefab, _content);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = Vector3.one;
                    rect = item.GetComponent<RectTransform>();
                }

                // 设置左上角对齐
                rect.pivot = new Vector2(0, 1);
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);

                _items[i] = item;
                _rects[i] = rect;
            }
        }


        private void UpdateItemData(Vector2 value)
        {
            if (_items == null) return;

            int visibleMaxIdx = GetMaxIndexInView();
            Debug.Log(visibleMaxIdx);
            for (int i = _items.Length - 1, j = visibleMaxIdx; i >= 0; j--, i--)
            {
                if (j < 0)
                {
                    _items[i].SetActive(false);
                    continue;
                }

                if (!_items[i].activeSelf)
                    _items[i].SetActive(true);

                if (UpdateHeightFunc!=null)
                {
                    UpdateDataFunc(j, _items[i]);
                }
                _rects[i].anchoredPosition = _positions[j];
            }
        }

        public void EditorUpdate()
        {
            PushItem(0);
        }

        public void PushItem(int count)
        {
            if (count < 0) return;
            ReCalcSize(count);
            UpdateItemData(Vector2.zero);
        }

        public void PopItem(int popCount)
        {
            if (popCount <= 0) return;
            ReCalcSize(-popCount);
            UpdateItemData(Vector2.zero);
        }

        private void ReCalcSize(int count)
        {
            _count += count;
            if (_count < 0) _count = 0;
            Vector2 size = CalcSizesAndPositions();
            if (_items == null) InitItems();

            _content.sizeDelta = size;
        }

        private Vector2 TempPosition = Vector2.one;

        private Vector2 CalcSizesAndPositions()
        {
            _heights.Clear();
            _positions.Clear();

            float xPos = 0;
            float yPos = 0;
            int row = 0, colum = 0;
            for (int i = 0; i < _count; i++)
            {
                if (isVertical)
                {
                    row = i / Columns;
                    colum = i % Columns;

                    xPos = LeftSpace + colum * (itemWidth + Space.x);
                    yPos = -(TopPadding + row * (itemHeight + Space.y));
                }

                else
                {
                    row = i % Rows;
                    colum = i / Rows;

                    xPos = LeftSpace + colum * (itemWidth + Space.x);
                    yPos = -(TopPadding + row * (itemHeight + Space.y));
                }

                TempPosition.x = xPos;
                TempPosition.y = yPos;

                _heights[i] = UpdateHeightFunc?.Invoke(i) ?? itemHeight;
                _positions[i] = TempPosition;
            }

            if (isVertical)
            {
                 return new Vector2(_content.sizeDelta.x,
                    Mathf.Abs(_positions[_count - 1].y) + _heights[_count - 1] + BottomPadding);
            }

            return new Vector2(_positions[_count - 1].x + _heights[_count - 1], _content.sizeDelta.y);
        }

        private int GetMaxIndexInView()
        {
            float size = isVertical
                ? (_content.anchoredPosition.y + _viewSize.y)
                : (-_content.anchoredPosition.x + _viewSize.x);

            for (int i = 0; i < _positions.Count; i++)
            {
                if ((isVertical && size <= -_positions[i].y) || (!isVertical && size <= _positions[i].x))
                {
                    return i;
                }
            }

            return _positions.Count - 1;
        }

        [Button("Test Push Items")]
        public void TestPushItems()
        {
            Initialize();
            PushItem(20);
        }
    }
}