using System;
using GameDevKit;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevKitEditor
{
    [TreeWindow("UI工具")]
    public class UIWindow:OdinEditorWindow
    {
        private static UIWindow _window;

        
        [FolderPath()]
        [LabelText("UIForm当前UIForm模板")]
        public string uiFormPath = "Assets/GameDevKit/Demo/Script/UI";
        [MenuItem("FTool/UIWindow")]
        public static void Open()
        {
            _window ??= GetWindow<UIWindow>();
            _window.Show();
        }
        
        [Button("设置子类Image不遮挡",ButtonSizes.Large)]
        public static void SetDisMask()
        {
            GameObject go = Selection.activeObject as GameObject;
            var images =go.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                image.maskable = false;
                image.raycastTarget = true;
            } 
            var texts =go.GetComponentsInChildren<Text>();
            foreach (var image in texts)
            {
                image.maskable = false;
                image.raycastTarget = true;
            }
        }
        [Button("设置子类Image NativeSize",ButtonSizes.Large)]
        public static void SetRawSize()
        {
            GameObject go = Selection.activeObject as GameObject;
            var images =go.GetComponentsInChildren<Image>();
           for (var i = 0; i < images.Length; i++)
           {
               images[i].SetNativeSize();
           }
        }     
        [Button("获取Ui位置信息",ButtonSizes.Large)]
        public static void getPivotPos()
        {
            GameObject go = Selection.activeObject as GameObject;
            Debug.Log("pivot---"+go.GetComponent<RectTransform>().pivot);
            Debug.Log("anchoredPosition---"+go.GetComponent<RectTransform>().anchoredPosition);
        }     
        [Button("设置子类Image原始大小",ButtonSizes.Large)]
        public static void SetRawWithHeight()
        {
            GameObject go = Selection.activeObject as GameObject;
     
            var images =go.GetComponentsInChildren<Image>();
           for (var i = 0; i < images.Length; i++)
           {
              var sprite = images[i].sprite;
              float height= sprite.rect.height;
              float width=sprite.rect.width;
              images[i].GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
              images[i].GetComponent<RectTransform>().localScale = Vector3.one;
           }
        }
        [Button("设置子类字体大小",ButtonSizes.Large)]
        public static void SetFontSize(int fontsize=10)
        {
            GameObject go = Selection.activeObject as GameObject;
            var texts =go.GetComponentsInChildren<Text>();
            for (var i = 0; i < texts.Length; i++)
            {
                texts[i].fontSize = fontsize;
            }
        }
        [Button("子物体自适应父类Rect",ButtonSizes.Large)]
        public static void GetParenRect()
        {
            GameObject go = Selection.activeObject as GameObject;
            var texts =go.GetComponentsInChildren<Text>();
            for (var i = 0; i < texts.Length; i++)
            {
                texts[i].fontSize = 10;
            }
        }
        [Button("添加到引用中",ButtonSizes.Large)]
        public static void AddReference()
        {
            var gameObjects = Selection.gameObjects ;
            var fMonoReferenceHolder = gameObjects[0].transform.GetComponentInParent<ReferenceHelper>();

            foreach (var gameObject in gameObjects)
            {
                fMonoReferenceHolder.Add(new ReferenceHelper.MonoReference()
                {
                    go = gameObject,
                    name = gameObject.name.SplitAfter('_')
                });
            }
        }
        [GUIColor(0, 1, 0)]
        [Button("生成UIForm代码",ButtonSizes.Large,ButtonStyle.Box)]
        public void GenerateUIFormCode()
        {
            var gameObject = Selection.activeGameObject ;
            //看有没有FMonoReferenceHolder组件，如果没有就添加一个，弹出提示
            var fMonoReferenceHolder = gameObject.TryGetOrAddComponent<ReferenceHelper>();
            fMonoReferenceHolder.CreateNewCode();
            
            
            SystemHelper.GetPaste();
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}