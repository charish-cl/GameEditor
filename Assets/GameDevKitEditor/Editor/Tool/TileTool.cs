// using System.Collections.Generic;
// using System.IO;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Tilemaps;
//
// namespace Editor.GameDevKit
// {
//     public class TileTool
//     {
//         public static string path = "Assets/RESOURCES/Tile/";
//         
//         [MenuItem("Assets/批量生成瓦片/动画瓦片(选中精灵图集)", false, 12)]
//         public static void GeneratAnimTile()
//         {
//             var temppath = path + "动画瓦片/";
//             if (!Directory.Exists(temppath))
//                 Directory.CreateDirectory(temppath);
//             var gos=Selection.objects;
//             foreach (var o in gos)
//             {
//                 var go= AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(o));
//                 var animatedTile = ScriptableObject.CreateInstance<AnimatedTile>();
//                 
//                 List<Sprite> sprites = new List<Sprite>();
//                 for (var i =1; i < go.Length; i++)
//                 {
//                     sprites.Add(go[i] as Sprite);
//                 }
//                 animatedTile.m_AnimatedSprites = sprites.ToArray();
//                 string assetPath = $"{temppath}/{o.name}.asset";
//                 AssetDatabase.CreateAsset(animatedTile, assetPath);
//             }
//             AssetDatabase.Refresh();
//         }   
//         [MenuItem("Assets/批量生成瓦片/预制体瓦片(选中预制体)", false, 12)]
//         public static void GeneratPrefabTile()
//         {
//             var temppath = path + "预制体瓦片/";
//             if (!Directory.Exists(temppath))
//                 Directory.CreateDirectory(temppath);
//             var gos=Selection.objects;
//             foreach (var o in gos)
//             {
//                 var ruleTile = ScriptableObject.CreateInstance<RuleTile>();
//                 var go = o as GameObject;
//                 ruleTile.m_DefaultGameObject = go;
//                 ruleTile.m_DefaultSprite = go.GetComponent<SpriteRenderer>().sprite;
//                 ruleTile.m_DefaultColliderType = Tile.ColliderType.None;
//                 string assetPath = $"{temppath}/{o.name}.asset";
//                 AssetDatabase.CreateAsset(ruleTile, assetPath);
//             }
//             AssetDatabase.Refresh();
//         }   
//         
//         [MenuItem("Assets/批量生成笔刷/预制体笔刷(选中预制体)", false, 12)]
//         public static void GeneratPrefabBrush()
//         {
//             var temppath = path + "预制体笔刷/";
//             if (!Directory.Exists(temppath))
//                 Directory.CreateDirectory(temppath);
//             var gos=Selection.objects;
//             foreach (var o in gos)
//             {
//                 var prefabBrush = ScriptableObject.CreateInstance<PrefabBrush>();
//                 var go = o as GameObject;
//                 prefabBrush.m_Prefab = go;
//                 string assetPath = $"{temppath}/{o.name}.asset";
//                 AssetDatabase.CreateAsset(prefabBrush, assetPath);
//             }
//             AssetDatabase.Refresh();
//         }   
//         
//         
//         public static string GetSelectedPathOrFallback()
//         {
//             string path = "Assets";
//
//             foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
//             {
//                 path = AssetDatabase.GetAssetPath(obj);
//                 if (!string.IsNullOrEmpty(path) && File.Exists(path))
//                 {
//                     //如果是目录获得目录名，如果是文件获得文件所在的目录名
//                     path = Path.GetDirectoryName(path);
//                     break;
//                 }
//             }
//             return path;
//         }
//
//     }
// }