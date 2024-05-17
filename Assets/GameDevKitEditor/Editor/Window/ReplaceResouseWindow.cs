using System.Diagnostics;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace GameDevKitEditor
{
    [TreeWindow("替换资源工具")]
    public class ReplaceResouseWindow : OdinEditorWindow
    {
        public Object findObj;
        public Object newObj;


        // public bool checkPrefab = true;
        // public bool checkScene = true;
        // public bool checkMaterial = true;
        //
        //

        [LabelText("是否计算耗时")] public bool IsCalculateTime;


        string filter = "";
        string oldfilter = "";
        string[] guids;
        bool isfirst = true;
        int len = 0;
        [LabelText("查询字符")] public string searchStr = "";

        [LabelText("查询的预制体路径,作为缓存使用")] [Searchable]
        public List<string> prefabpathlist = new List<string>();

        [LabelText("查询结果")] public List<Object> result = new List<Object>();


        /// <summary>
        ///    查找图片预制件引用  根据文字搜索prefab
        /// </summary>
        ///
        [Button("查找文件引用  根据文字搜索prefab", ButtonHeight = 30)]
        public void FindReference()
        {
            // 资源排查情况
            // 1 直接转移文件到子common  实现复制资源到新目录
            // 3 整个prefab的替换 替换原来的prefab为新的通用prefab 注意：数据绑定要拷贝过来  文本绑定需要拷贝 情况比较复杂需要特别查看

            result.Clear();
            string assetGuid = "";
            // 循环读取目录查找每个文件
            Debug.Log("查找" + searchStr);
            if (findObj == null)
            {
                // 字符查询
                if (searchStr == "")
                {
                    return;
                }
            }
            else
            {
                string assetPath = AssetDatabase.GetAssetPath(findObj);
                assetGuid = AssetDatabase.AssetPathToGUID(assetPath);
            }

            Stopwatch sw = new Stopwatch();
            if (IsCalculateTime)
            {
                sw.Start();
            }

            filter = "";
            if (true)
            {
                filter += "t:Prefab ";
                filter += "t:SpriteData ";
            }

            if (filter != oldfilter)
            {
                isfirst = true;
            }

            if (isfirst)
            {
                oldfilter = filter;
                guids = AssetDatabase.FindAssets(filter, new[] { "Assets" });
                prefabpathlist.Clear();
                len = guids.Length;
                Debug.Log("查找文件总数:" + len);
                for (int i = 0; i < len; i++)
                {
                    string filePath = AssetDatabase.GUIDToAssetPath(guids[i]);
                    prefabpathlist.Add(filePath);
                }

                isfirst = false;
            }

            len = guids.Length;
            Debug.Log("数量:" + len);
            ArrayList filelist = new ArrayList();
            //使用多线程提高查询效率
            float findex = 1.0f;
            string matchName = searchStr;
            // 汉字进行unicode编码
            if (!string.IsNullOrEmpty(searchStr))
            {
                for (int i = 0; i < searchStr.Length; i++)
                {
                    if ((int)searchStr[i] > 127)
                    {
                        matchName = matchName.Replace(searchStr[i].ToString(),
                            String2Unicode(searchStr[i].ToString()));
                    }
                }
            }

            Debug.Log("search>>" + matchName);
            Parallel.ForEach(prefabpathlist, filePath =>
            {
                // 检查是否包含guid
                findex = findex + 1;
                try
                {
                    // 某些文件读取会抛出异常
                    //测试数据流读取的时间 11s
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        int nBytes = (int)fs.Length; //计算流的长度
                        byte[] byteArray = new byte[nBytes]; //初始化用于MemoryStream的Buffer
                        int nBytesRead = fs.Read(byteArray, 0, nBytes); //将File里的内容一次性的全部读到byteArray中去
                        string str = System.Text.Encoding.Default.GetString(byteArray);
                        bool ishave = false;
                        if (!string.IsNullOrEmpty(assetGuid))
                        {
                            if (str.Contains(assetGuid))
                            {
                                ishave = true;
                            }
                        }

                        // 是否存在文本
                        if (!string.IsNullOrEmpty(searchStr))
                        {
                            if (str.Contains(matchName))
                            {
                                ishave = true;
                            }
                        }

                        if (ishave)
                        {
                            filelist.Add(filePath);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning(filePath + "\n" + e.ToString());
                }
            });
            Debug.Log($"引用对象数量：{filelist.Count}");
            // 查找完毕添加显示
            foreach (string i in filelist)
            {
                Object fileObject = AssetDatabase.LoadAssetAtPath(i, typeof(Object));
                result.Add(fileObject);
            }

            if (IsCalculateTime)
            {
                sw.Stop();

                TimeSpan ts2 = sw.Elapsed;

                Debug.Log("查询总共花费s." + ts2.TotalMilliseconds / 1000);
            }
        }

        [Button("批量替换图片资源")]
        public void Replace()
        {
            // 2 查询调用的prefab 更改图片资源为新的图片 实现替换功能
            if (EditorUtility.DisplayDialog("提示", "确定要批量替换掉当前资源吗", "确定"))
            {
                if (findObj == null)
                {
                    EditorUtility.DisplayDialog("提示", "目标对象为空", "Ok");
                    return;
                }

                if (newObj == null)
                {
                    EditorUtility.DisplayDialog("提示", "替换新对象为空", "Ok");
                    return;
                }

                if (result.Count < 1)
                {
                    EditorUtility.DisplayDialog("提示", "没有可替换引用组件", "Ok");
                    return;
                }

                string oldGuid = string.Empty;
                string newGuid = string.Empty;
                if (findObj)
                {
                    oldGuid = findObj is Texture2D ||
                              findObj is Sprite
                        ? GetTexture2DAndSprideStr(findObj)
                        : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(findObj));
                }

                if (newObj)
                {
                    newGuid = newObj is Texture2D ||
                              newObj is Sprite
                        ? GetTexture2DAndSprideStr(newObj)
                        : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(newObj));
                }

                Debug.Log("替换prefab图片资源");
                //1 先搜索引用到的prefab 
                //2 循环替换preab中 替换文件的guid
                EditorApplication.update = delegate()
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        Object destObj = result[i];
                        FindAssets(oldGuid, newGuid, destObj);
                    }
                };
            }
        }

        /// <summary>
        /// 取得替换资源内容 fileid guid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string GetTexture2DAndSprideStr(Object obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            string guid = AssetDatabase.AssetPathToGUID(path);
            string fileID = string.Empty;
            if (obj is Texture2D) fileID = "21300000";
            else if (obj is Sprite)
            {
                if (AssetDatabase.LoadAllAssetsAtPath(path).Length <= 2) fileID = "21300000";
                else
                {
                    string fileName = obj.name;
                    string matchName = fileName;
                    bool isHaveChinese = false;
                    for (int i = 0; i < fileName.Length; i++)
                    {
                        //有没有中文
                        if ((int)fileName[i] > 127)
                        {
                            if (!isHaveChinese) isHaveChinese = true;
                            matchName = matchName.Replace(fileName[i].ToString(),
                                String2Unicode(fileName[i].ToString()));
                        }
                    }


                    string metaPath = path + ".meta";
                    string[] metaTxt = File.ReadAllLines(metaPath);
                    if (isHaveChinese) matchName = "\"" + matchName + "\"";
                    string matchContent = string.Format(": {0}", matchName);
                    for (int i = 0; i < metaTxt.Length; i++)
                    {
                        if (metaTxt[i].Contains(matchContent))
                        {
#if UNITY_2019
                                fileID = metaTxt[i-1].Split(':')[1].Trim();
#else
                            fileID = metaTxt[i].Split(':')[0].Trim();
#endif
                            break;
                        }
                    }
                }
            }
            else return null;

            return string.IsNullOrEmpty(fileID)
                ? string.Empty
                : string.Format("fileID: {0}, guid: {1}", fileID, guid);
        }

        /// <summary>
        /// 这个是用来替换Prefab之类的文件中的Guid的
        /// </summary>
        /// <param name="oldGuid"></param>
        /// <param name="newGuid"></param>
        /// <param name="destObj"></param>
        void FindAssets(string oldGuid, string newGuid, Object destObj)
        {
            // UnityEngine.Debug.LogWarning("oldGuid--->" + oldGuid);
            // UnityEngine.Debug.LogWarning("newGuid--->" + newGuid);
            string assetPath = AssetDatabase.GetAssetPath(destObj);
            //string file = System.IO.Directory.GetFiles(assetPath);
            string objTxt = File.ReadAllText(assetPath);
            if (Regex.IsMatch(objTxt, oldGuid))
            {
                objTxt = objTxt.Replace(oldGuid, newGuid);
                File.WriteAllText(assetPath, objTxt);

                var metaPath = AssetDatabase.GetAssetPath(destObj) + ".meta";
                Debug.Log(metaPath);
                AssetDatabase.DeleteAsset(metaPath);
            }
            
        }

        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public string String2Unicode(string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0').ToUpper(),
                    bytes[i].ToString("x").PadLeft(2, '0').ToUpper());
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 替换资源
        /// </summary>
        /// <param name="sourceObj">源Object</param>
        /// <param name="targetObj">目标Object</param>
        public bool ReplaceResource(Object sourceObj, Object targetObj)
        {
            findObj = sourceObj;
            newObj = targetObj;
            FindReference();

            if (result.Count == 0)
            {
                Debug.Log($"没有资源引用 {sourceObj.name}");
                return false;
            }

            string oldGuid = sourceObj is Texture2D ||
                             sourceObj is Sprite
                ? GetTexture2DAndSprideStr(sourceObj)
                : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(sourceObj));

            string newGuid = targetObj is Texture2D ||
                             targetObj is Sprite
                ? GetTexture2DAndSprideStr(targetObj)
                : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(targetObj));

            EditorApplication.update = delegate()
            {
                for (int i = 0; i < result.Count; i++)
                {
                    Object destObj = result[i];
                    FindAssets(oldGuid, newGuid, destObj);
                    EditorUtility.SetDirty(destObj);
                }
            };
            AssetDatabase.Refresh();
            return true;
        }

        public bool CheckIsUnUse(Object sourceObj)
        {
            findObj = sourceObj;
            FindReference();

            return result.Count == 0;
        }

        public List<Object> AssetDataBaseGetAllFolderAsset(string directoryPath)
        {
            // 获取目录下的所有资源GUID
            string[] guids = AssetDatabase.FindAssets("", new string[] { directoryPath });

            // 创建列表来存储所有资源
            List<Object> assetsList = new List<Object>();

            // 遍历所有资源GUID，并加载资源添加到列表中
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
                if (asset != null)
                {
                    assetsList.Add(asset);
                }
            }

            return assetsList;
        }

        public string GetCurrentAssetDirectory()
        {
            // 获取当前选中的资源
            var GUIDs = Selection.assetGUIDs;
            foreach (var guid in GUIDs)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                return path;
                //输出结果为：Assets/测试文件.png
            }

            return null;
        }

        [TabGroup("合并资源")] [Searchable] public List<List<Texture2D>> NeedMergeResources = new List<List<Texture2D>>();
        [TabGroup("合并资源")] [LabelText("选中目录")] public string DirectoryPath;

        [TabGroup("合并资源")]
        [Button("获取当前打开文件夹下的相似资源", ButtonHeight = 50)]
        public void GetSimilarAsset()
        {
            var path = GetCurrentAssetDirectory();


            if (DirectoryPath != null)
            {
                path = DirectoryPath;
            }

            var assets = AssetDataBaseGetAllFolderAsset(path)
                .Where(e => e.GetType() == typeof(Texture2D))
                .Select(e => e as Texture2D).ToList();

            NeedMergeResources?.Clear();
            NeedMergeResources = new List<List<Texture2D>>();
            ImageComparer.imageCache.Clear();
            foreach (var texture2D in assets)
            {
                ImageComparer.AddImage(texture2D);
            }

            Dictionary<string, List<Texture2D>> groupedAssets = new Dictionary<string, List<Texture2D>>();

            groupedAssets = ImageComparer.imageCache;
            // 将分组的资源添加到NeedMergeResources中
            foreach (var group in groupedAssets)
            {
                //只有一个不算是冗余资源
                if (group.Value.Count == 1)
                {
                    continue;
                }

                NeedMergeResources.Add(group.Value);
            }
        }

        [TabGroup("合并资源")]
        [Button("替换NeedMergeResources中的依赖对象", ButtonHeight = 50)]
        public void ReplaceNeedMergeResources()
        {
            foreach (var resources in NeedMergeResources)
            {
                var sprites = resources.Select(e =>
                    AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(e))).ToArray();
                //查找所有依赖，替换为第一张sprite
                foreach (var s in sprites)
                {
                    ReplaceResource(s, sprites[0]);
              
                }

                //删除只保留第一个
                for (var i = sprites.Length - 1; i > 0; i--)
                {
                     AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(sprites[i]));
                }
            }
            AssetDatabase.Refresh();
        }
        
        [TabGroup("清除资源")]
        public List<Object> ClearResourse = new List<Object>(); // 依赖于资源的物体列表
   
        [TabGroup("清除资源")]
        [Button("获取资源")]
        public void GetUnUseResources()
        {
            var path = GetCurrentAssetDirectory();
            
            var assets = AssetDataBaseGetAllFolderAsset(path)
                .Where(e=>e.GetType()==typeof(Texture2D))
                .Select(e=>e as Texture2D).ToList();
            foreach (var o in assets)
            {
                if (CheckIsUnUse(o))
                {
                    ClearResourse.Add(o);
                }
            }
        
        }
        [TabGroup("清除资源")]
        [Button("清除无用的")]
        public void ClearUnUseResources()
        {
            foreach (var o in ClearResourse)
            {
                var path = AssetDatabase.GetAssetPath(o);
                AssetDatabase.DeleteAsset(path);
                
            }
            AssetDatabase.Refresh();
        }
    }
}