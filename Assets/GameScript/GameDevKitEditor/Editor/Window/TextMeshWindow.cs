// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using Sirenix.OdinInspector.Editor;
// using TMPro;
// using UnityEditor;
// using UnityEngine;
//
// namespace GameDevKitEditor
// {
//     [TreeWindow("UI工具/字体工具")]
//     public class TextMeshWindow : OdinEditorWindow
//     {
//         [TableList] public List<TextMeshData> textMeshList;
//
//         [Button("获取所有字体", ButtonSizes.Large)]
//         public void GetAllTextMesh()
//         {
//             // Initialize list
//             textMeshList = new List<TextMeshData>();
//             // Find all TextMesh components under the Canvas object
//             Canvas canvas = FindObjectOfType<Canvas>();
//             if (canvas != null)
//             {
//                 TextMeshProUGUI[] textMeshes = canvas.GetComponentsInChildren<TextMeshProUGUI>();
//                 foreach (TextMeshProUGUI textMesh in textMeshes)
//                 {
//                     textMeshList.Add(new TextMeshData(textMesh));
//                 }
//             }
//         }
//
//         [Button("保存", ButtonSizes.Large)]
//         public void Save()
//         {
//             foreach (TextMeshData data in textMeshList)
//             {
//                 data.textMesh.text = data.text;
//                 data.textMesh.gameObject.name = data.name;
//             }
//         }
//
//         [Button("设置TextMeshProUGUI排列方式", ButtonSizes.Large)]
//         public void SetTextMeshProUGUIAlignment()
//         {
//             foreach (TextMeshData data in textMeshList)
//             {
//                 SetTextMeshProUGUIAlignment(data.textMesh);
//             }
//         }
//
//         private void SetTextMeshProUGUIAlignment(TextMeshProUGUI textMesh)
//         {
//             if (textMesh != null)
//             {
//                 textMesh.alignment = TextAlignmentOptions.Left;
//                 textMesh.enableAutoSizing = true;
//                 textMesh.enableWordWrapping = true;
//                 textMesh.overflowMode = TextOverflowModes.Overflow;
//                 // textMesh.verti = VerticalWrapMode.Overflow;
//                 // textMesh.horizontalOverflow = HorizontalWrapMode.Wrap;
//                 textMesh.lineSpacing = 1f; // 设置行间距
//             }
//         }
//
//         [Button("获取TextMeshProUGUI全局设置", ButtonSizes.Large)]
//         public void GetGlobalTextMeshSettings()
//         {
//             TMP_Settings settings = TMP_Settings.GetSettings();
//             EditorGUIUtility.PingObject(settings);
//             // 其他全局属性...
//         }
//
//         // Custom class to store TextMeshProUGUI data
//         public class TextMeshData
//         {
//             [TableColumnWidth(250)] public string path;
//             [TableColumnWidth(150)] public string name;
//             [TableColumnWidth(200)] public string text;
//             [TableColumnWidth(200)] public TextMeshProUGUI textMesh;
//
//             public TextMeshData(TextMeshProUGUI textMesh)
//             {
//                 this.textMesh = textMesh;
//                 this.text = textMesh.text;
//                 this.name = textMesh.gameObject.name;
//                 this.path = GetPath(textMesh.gameObject);
//             }
//
//             private string GetPath(GameObject obj)
//             {
//                 string path = obj.name;
//                 while (obj.transform.parent != null)
//                 {
//                     obj = obj.transform.parent.gameObject;
//                     path = obj.name + "/" + path;
//                 }
//
//                 return path;
//             }
//         }
//
//         // Automatically select the TextMeshProUGUI object in the hierarchy when its text is changed
//         [OnValueChanged(nameof(SelectTextMesh))]
//         public string SelectedText;
//
//         private void SelectTextMesh()
//         {
//             foreach (TextMeshData data in textMeshList)
//             {
//                 if (data.text == SelectedText)
//                 {
//                     Selection.activeGameObject = data.textMesh.gameObject;
//                     break;
//                 }
//             }
//         }
//     }
// }