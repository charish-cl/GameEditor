using System.Linq;
using GameDevKit;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;
using UnityEngine;

namespace GameDevKitEditor
{
    public class SceneRightClickMenu : Editor
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;
            if (e != null && e.button == 1 && e.type == EventType.MouseDown)
            {
                var menu = new GenericMenu();
                var methods = TypeCache.GetMethodsWithAttribute<SceneRightClickMenuItemAttribute>();
                foreach (var method in methods)
                {
                    var attribute = method.GetCustomAttributes(typeof(SceneRightClickMenuItemAttribute), true).First();
                    if (attribute == null)
                    {
                        continue;
                    }

                    var menuItem = attribute as SceneRightClickMenuItemAttribute;
                    menu.AddItem(new GUIContent(menuItem.MenuItemName), false, () => method.InvokeMethod());
                }

                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    var scene = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i).Split('/');
                    menu.AddItem(new GUIContent("进入_" + scene[scene.Length - 1]), false, OnLoadScene,
                        UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
                }

                menu.ShowAsContext();
                e.type = EventType.MouseUp;
            }
        }

        private static void OnLoadScene(object userdata)
        {
            EditorSceneManager.SaveOpenScenes();
            string scenename = (string)userdata;
            EditorSceneManager.OpenScene(scenename);
        }
    }
}