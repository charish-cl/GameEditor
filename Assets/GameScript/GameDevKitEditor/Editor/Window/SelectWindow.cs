using System;
using System.Linq;
using System.Reflection;
using GameDevKit;
using GameDevKit.GameLogic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameDevKitEditor
{
    //Update方法不会在OdinTree上调用,只能用OnGUI了
    [TreeWindow("选中工具")]
    public class SelectWindow:OdinEditorWindow
    {
        private static SelectWindow _window;
        
        [LabelText("是否在场景中")]
        public bool isSceneobj = false;
        [LabelText("选中对象")]
        public Object obj;
        [LabelText("对象类型")]
        public string objType;

        [LabelText("选中对象数量")]
        public int cnt;
        
     
        [LabelText("选中文件绝对路径")]
        public string absolutpath;
        [LabelText("选中文件路径")]
        public string path;

        [LabelText("Unity的Guid")]
        public string unityguid;
        [LabelText("系统的Guid")]
        public Guid guid;
        

        [OnInspectorGUI]
        private void MyUpdate()
        {
            if (Selection.activeObject == null)
            {
                return;
            }
            isSceneobj = Selection.activeGameObject;
            
            cnt = Selection.count;
            obj = Selection.activeObject;
            objType = Selection.activeObject.GetType().ToString();
            
            if (isSceneobj)
            {
                return;
            }
            unityguid = Selection.assetGUIDs.First();
            guid = new Guid(unityguid);
            path = SelectionUtility.GetSelectionPath();
        }
   
        
        [Button("销毁子物体")]
        public static void DestroyAllChildren()
        {
            Selection.activeGameObject.transform.DestoryChildren();
            Selection.activeObject.SetAssetDirty();
        }
        [MenuItem("FTool/SelectWinodw")]
        public static void Open()
        {
            _window ??= GetWindow<SelectWindow>();
            _window.Show();
        }
        
        
        [Button("获取选中文件相对路径")]
        public static void GetFileRelativePath()
        {
            var GUIDs = Selection.assetGUIDs;
            foreach (var guid in GUIDs)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                Debug.Log(path);
                //输出结果为：Assets/测试文件.png
            }
        }
        [Button("获取选中文件绝对路径")]
        public static void GetFileAbsolutePath()
        {
            var GUIDs = Selection.assetGUIDs;

            foreach (var guid in GUIDs)
            {
                var path = Application.dataPath.Remove(Application.dataPath.Length - 6) +
                           AssetDatabase.GUIDToAssetPath(guid);
                Debug.Log(path);
                ///输出结果为：Volumes/Eevee_4TB/ZKCM/Test/Draw/Assets/测试文件.png
            }
        }
        [OnInspectorGUI]
        private void Space2()
        {
            GUILayout.Space(20);
        }
          
        #region 组件工具
        
        //Unity事件快捷键： Ctrl + Shift + M /Ctrl + Shift + Q  实现
        [MenuItem("FTool/组件工具/全部展开 %#m")]
        [ButtonGroup("组件")]
        [Button("全部展开组件")]
        static void Expansion()
        {
            
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var window = EditorWindow.GetWindow(type);
            FieldInfo info = type.GetField("m_Tracker", BindingFlags.NonPublic | BindingFlags.Instance);
            ActiveEditorTracker tracker = info.GetValue(window) as ActiveEditorTracker;
 
            for (int i = 0; i < tracker.activeEditors.Length; i++)
            {
                // 可以通过名子单独判断组件展开或不展开
                //if (tracker.activeEditors[i].target.GetType().Name != "NewBehaviourScript")
                //{
                //这里1就是展开，0就是合起来
                tracker.SetVisible(i, 1);
                //}
            }
            SelectionUtility.SetSelectionDirty();
        }
        [MenuItem("FTool/组件工具/全部收起组件 %#R")]
        [ButtonGroup("组件")]
        [Button("全部折叠组件")]
        static void Shrinkage()
        {
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var window = EditorWindow.GetWindow(type);
            FieldInfo info = type.GetField("m_Tracker", BindingFlags.NonPublic | BindingFlags.Instance);
            ActiveEditorTracker tracker = info.GetValue(window) as ActiveEditorTracker;
 
            for (int i = 0; i < tracker.activeEditors.Length; i++)
            {
                //这里1就是展开，0就是合起来
                tracker.SetVisible(i, 0);
            }
            SelectionUtility.SetSelectionDirty();
        }
 
        #endregion

        [OnInspectorGUI]
        private void Space1()
        {
            GUILayout.Space(20);
        }
        
        [Button("移除场景中所有的空组件",ButtonSizes.Large)]
        [ButtonGroup("组件2")]
        public static void RemoveNullComponent()
        {
            var gos = SceneAsset.FindObjectsOfType<GameObject>();
            for ( int i = 0 ; i < gos.Length ; i ++ )
            {
                SelectionUtility.RemoveComponent(gos[i]);
            }
        }
        [Button("移除选中物体以下层级的空组件",ButtonSizes.Large)]
        [ButtonGroup("组件2")]
        public static void SelectionGameObj()
        {
            GameObject[] gos = Selection.gameObjects;
            for ( int i = 0 ; i < gos.Length ; i ++ )
            {
                SelectionUtility.RemoveComponent(gos[i]);
            }
        }
        //通过属性unityguid Ping对象
        [Button("Ping对象",ButtonSizes.Large)]
        public  void PingObject()
        {
            
            var path = AssetDatabase.GUIDToAssetPath(unityguid);
            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
            EditorGUIUtility.PingObject(obj);
        }
        
        //系统的Guid转换为Unity的Guid,通过把字符串中的'-'去掉
        [Button("系统的Guid转换为Unity的Guid",ButtonSizes.Large)]
        public  void GuidToUnityGuid()
        {
            var newGuid = guid.ToString().Replace("-","");
                newGuid.Dump();
            var path = AssetDatabase.GUIDToAssetPath(newGuid);
            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
            EditorGUIUtility.PingObject(obj);
        }

    }
}