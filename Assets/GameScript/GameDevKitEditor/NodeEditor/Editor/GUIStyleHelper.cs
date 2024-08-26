using UnityEngine;

namespace GameDevKit.NodeEditor
{
    public static class GUIStyleHelper
    {
        public static void SetNormalBackground(this GUIStyle style, Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            style.normal.background = texture;
        }
        // SetActiveBackground方法,如上
        
        public static void SetActiveBackground(this GUIStyle style, Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            style.active.background = texture;
        }
        
        // SetHoverBackground
        public static void SetHoverBackground(this GUIStyle style, Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            style.hover.background = texture;
        }
        
        //Rect按照scale进行缩放
        public static Rect ScaleSizeBy(this Rect rect, float scale)
        {
            return new Rect(rect.x, rect.y, rect.width * scale, rect.height * scale);
        }
    }
}