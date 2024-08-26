
namespace GameDevKitEditor
{
    using UnityEditor;
    using UnityEngine;
    public class EditorIconViewer : EditorWindow
    {
        private Vector2 _scrollPosition;
        [MenuItem("Window/Editor Icon Viewer")]
        private static void ShowWindow()
        {
            var window = GetWindow<EditorIconViewer>("Editor Icon Viewer");
            window.Show();
        }
        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            var textures = Resources.FindObjectsOfTypeAll<Texture2D>();
            foreach (var texture in textures)
            {
                if (texture.name.StartsWith("sv_") || texture.name.StartsWith("d_") || texture.name.StartsWith("builtin_"))
                {
                    continue;
                }
                var rect = GUILayoutUtility.GetRect(16, 16);
                GUI.DrawTexture(rect, texture);
                GUILayout.Label(texture.name);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}