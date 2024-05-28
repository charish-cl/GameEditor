using UnityEngine;
using UnityEngine.UI;

namespace BitCubic.UI
{
    public static class UIRectTransformExtend
    {
        public static bool Overlaps(this RectTransform a, RectTransform b)
        {
            return a.WorldRect().Overlaps(b.WorldRect());
        }
        public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
        {
            return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
        }

        public static Rect WorldRect(this RectTransform rectTransform)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
            float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

            Vector3 position = rectTransform.position;
            return new Rect(position.x + rectTransformWidth * rectTransform.pivot.x, position.y - rectTransformHeight * rectTransform.pivot.y, rectTransformWidth, rectTransformHeight);
        }

        public static void ForceRebuildLayoutImmediate(this RectTransform rect)
        {
            LayoutGroup[] groups = rect.GetComponentsInChildren<LayoutGroup>();
            for (int i = groups.Length - 1; i >= 0; i--)
            {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(groups[i].GetComponent<RectTransform>());
            }
        }
    }
}