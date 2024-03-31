using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameDevKit
{
    //TODO；这里面的代码很是需要改下！好多重复功能的，还有根本没什么用的
    public static class FileKit
    {
        public enum PathType
        {
            /// <summary>
            /// 获取到本地工程的绝对路径
            /// </summary>
            ProjectAbosolutePath,
            /// <summary>
            ///  Assets资源文件夹的绝对路径
            /// </summary>
            Application_dataPath,
            /// <summary>
            ///  持久性的数据存储路径，在不同平台路径不同，但都存在，绝对路径
            /// </summary>
            Application_persistentDataPath,
            /// <summary>
            /// Assets资源文件夹下StreamingAssets文件夹目录的绝对路径
            /// </summary>
            Application_streamingAssetsPath,
            /// <summary>
            /// 游戏运行时的缓存目录，也是绝对路径
            /// </summary>
            Application_temporaryCachePath
        }

        [Serializable]
        public struct UnityFileInfo
        {
            
            public string assetpath; //Asset=/路径,包含name

            private string resourcepath;
            public string Resourcepath //Resources路径,包含name
            {
                get
                {
                    if ( !assetpath.Contains("Resources") )
                    {
                        Debug.Log("不包含Resources路径");
                        return null;
                    }
                    else
                    {
                        resourcepath = FileKit.GetResoucesPath(assetpath,IsDir);
                    }
                    return resourcepath;
                }
            }
            
            public string name;
            public string TopDir; //当前目录名
            public bool IsDir; //是否是目录
            public Object obj;
            public List<UnityFileInfo> subFileInfos; //子文件
        }

        /// <summary>
        /// 获取上一级目录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetTopDirName(FileInfo info)
        {
            return info.Directory.Name;
        }

        /// <summary>
        /// 获取Asset的同级目录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAssetDir(string name)
        {
            return Application.dataPath + $"/../{name}";
        }
        /// <summary>
        /// 判断文件是不是目录
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool IsDir(string filepath)
        {
            return Directory.Exists(filepath);
        }

        public static bool FileExit(string filePath)
        {
            return !File.Exists(filePath);
        }
        
       
        public static void GetAllSubUnityFileInfos(ref List<UnityFileInfo> unityFileInfos,
            string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.Log("不存在该路径" + path);
                return;
            }

            DirectoryInfo directory = new DirectoryInfo(path);
            //文件夹下一层的所有子文件
            //SearchOption.TopDirectoryOnly：这个选项只取下一层的子文件
            //SearchOption.AllDirectories：这个选项会取其下所有的子文件
            var directories = directory.GetDirectories();
            //添加所有目录
            foreach (var directoryInfo in directories)
            {
                UnityFileInfo unityFileInfo = new UnityFileInfo()
                {
                    name = directoryInfo.Name,
                    assetpath = GetRelativePath(directoryInfo.FullName),
                    IsDir = true,
                    subFileInfos = new List<UnityFileInfo>()
                };
                GetAllSubUnityFileInfos(ref unityFileInfo.subFileInfos, directoryInfo.FullName);
                unityFileInfos.Add(unityFileInfo);
            }

            var files = directory.GetFiles();
            //添加所有文件
            foreach (var fileInfo in files)
            {
                if (fileInfo.Name.EndsWith(".meta"))
                {
                    continue;
                }

                var assetpath = GetRelativePath(fileInfo.DirectoryName) + "/" +
                                fileInfo.Name;
                var go = ResourcesKit.LoadAssetAtPath(assetpath);
                UnityFileInfo unityFileInfo = new UnityFileInfo()
                {
                    name = go.name,
                    assetpath = assetpath,
                    IsDir = false,
                    TopDir = GetTopDirName(fileInfo),
                    obj = go,
                };
                unityFileInfos.Add(unityFileInfo);
            }
        }

        static void Test2(List<FileKit.UnityFileInfo> unityFileInfo)
        {
            if (unityFileInfo == null) return;
            foreach (var fileInfo in unityFileInfo)
            {
                if (fileInfo.IsDir)
                {
                    Debug.Log("目录" + fileInfo.assetpath);
                    Test2(fileInfo.subFileInfos);
                }
                else
                {
                    Debug.Log("文件" + fileInfo.assetpath);
                }
            }
        }

        public static void OpenDirectory(string path, bool isFile = false)
        {
            if (string.IsNullOrEmpty(path)) return;
            path = path.Replace("/", "\\");
            if (isFile)
            {
                if (!File.Exists(path))
                {
                    Debug.LogError("No File: " + path);
                    return;
                }

                path = string.Format("/Select, {0}", path);
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    Debug.LogError("No Directory: " + path);
                    return;
                }
            }

            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        public static string GetProjectPath(PathType pathType)
        {
            switch (pathType)
            {
                case PathType.ProjectAbosolutePath:
                    return System.Environment.CurrentDirectory;
                case PathType.Application_dataPath:
                    return Application.dataPath;
                case PathType.Application_persistentDataPath:
                    return Application.persistentDataPath;
                case PathType.Application_streamingAssetsPath:
                    return Application.streamingAssetsPath;
                case PathType.Application_temporaryCachePath:
                    return Application.temporaryCachePath;
            }

            return null;
        }


        /// <summary>
        /// 获取该路径下所有文件的路径
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllSubFile(string fullPath,SearchOption option =SearchOption.AllDirectories)
        {
            List<string> fileInfos = new List<string>();
            if ( Directory.Exists(fullPath) )
            {
                DirectoryInfo direction = new DirectoryInfo(fullPath);
                FileInfo[] files = direction.GetFiles("*", option);
                if ( files.Length == 0 )
                {
                    Debug.Log("不存在文件在该目录下" + fullPath);
                }
                foreach (var t in files)
                {
                    if ( t.Name.EndsWith(".meta") )
                    {
                        continue;
                    }
                    else
                    {
                        fileInfos.Add(GetRelativePath(t.DirectoryName) + "/" + t.Name);
                    }
                }
            }
            else
            {
                Debug.Log("不存在该目录" + fullPath);
            }
            return fileInfos;
        }
        public static List<T> GetAllSubFile<T>(string path, string patten) where T : Object
        {
            if (!Directory.Exists(path))
            {
                Debug.Log("不存在该路径" + path);
                return null;
            }

            DirectoryInfo direction = new DirectoryInfo(path);
            //文件夹下一层的所有子文件
            //SearchOption.TopDirectoryOnly：这个选项只取下一层的子文件
            //SearchOption.AllDirectories：这个选项会取其下所有的子文件
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            List<T> list = new List<T>();
            foreach (var fileInfo in files)
            {
                if (fileInfo.Name.EndsWith(".meta") || !fileInfo.Name.EndsWith(patten))
                {
                    continue;
                }
#if UNITY_EDITOR
                var go = AssetDatabase.LoadAssetAtPath<T>(GetRelativePath(fileInfo.DirectoryName) + "/" +
                                                          fileInfo.Name);
                if (go != null)
                {
                    list.Add(go);
                }
#endif
            }

            return list;
        }

        public static string GetRelativePath(string path)
        {
            path = path.Substring(path.IndexOf("Assets"));
            path = path.Replace('\\', '/');
            return path;
        }
        /// <summary>
        /// 获取Resouces目录下的路径，无后缀
        /// </summary>
        /// <returns></returns>
        public static string GetResoucesPath(string path,bool isDir)
        {
           
            string length = "Assets/Resources/";
            path = path.Substring(length.Length);
            if ( isDir )
            {
                return path;
            }
            path=  path.Substring(0,path.IndexOf("."));
            return path;
            //Resources/Sprite/#1 - Transparent Icons_3
        }

        /// <summary>
        /// 获取资源目录下的所有路径，返回一个字典
        /// </summary>
        /// <param name="unityFileInfo"></param>
        /// <param name="paths"></param>
        public static void GetAllPaths(List<FileKit.UnityFileInfo> unityFileInfo, ref Dictionary<string, string> paths)
        {
            if ( unityFileInfo == null ) return;
            foreach ( var fileInfo in unityFileInfo )
            {
                if ( fileInfo.IsDir )
                {
                    //记录下路径
                    var path_key=fileInfo.Resourcepath.Replace('/', '_');
                    paths.Add(path_key, fileInfo.Resourcepath);
                    GetAllPaths(fileInfo.subFileInfos, ref paths);
                }
            }
          
        }

        public static void CreateScriptAndRefresh(string path, string filename, string res)
        {
#if UNITY_EDITOR
            CreatDirectoryIfEmpty(path);
            File.WriteAllText(path + $"{filename}.cs", res, Encoding.UTF8);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }  
        public static void CreateAndRefresh(string path, string filename, string res)
        {
#if UNITY_EDITOR
            CreatDirectoryIfEmpty(path);

            File.WriteAllText(path + $"{filename}", res, Encoding.UTF8);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        
        
        /// <summary>
        /// 文件的创建，写入
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="name">文件名</param>
        /// <param name="info">信息</param>
        public static void Createfile(string path, string name, string info)
        {
            StreamWriter sw; //流信息
            FileInfo t = new FileInfo(path + "//" + name);
            if (!t.Exists)
            {
                //判断文件是否存在
                sw = t.CreateText(); //不存在，创建
            }
            else
            {
                sw = t.AppendText(); //存在，则打开
            }

            sw.WriteLine(info); //以行的形式写入信息
            sw.Close(); //关闭流
            sw.Dispose(); //销毁流
        }

        /// <summary>
        /// 文件的读取
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="name">文件名</param>
        /// <returns>文件数据</returns>
        public static ArrayList LoadFile(string path, string name)
        {
            StreamReader sr = null; //文件流
            try
            {
                //通过路径和文件名读取文件
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception ex) //需要引入命名空间 using System
            {
                Debug.LogError(ex.Message);
                return null;
            }

            string line;
            ArrayList arrlist = new ArrayList(); //需要引入命名空间 using System.Collections
            while ((line = sr.ReadLine()) != null)
            {
                //读取每一行加入到ArrayList中
                arrlist.Add(line);
            }

            sr.Close();
            sr.Dispose();
            return arrlist;
        }

        public static List<T> LoadAllFile<T>(string fullPath, SearchOption option = SearchOption.AllDirectories)
            where T : Object
        {
            List<T> list = new List<T>();
            //获取指定路径下面的所有资源文件
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo direction = new DirectoryInfo(fullPath);
                FileInfo[] files = direction.GetFiles("*", option);
                if ( files.Length == 0 )
                {
                    Debug.Log("不存在文件在该目录下" + fullPath);
                }
                foreach (var t in files)
                {
                    if (t.Name.EndsWith(".meta"))
                    {
                        continue;
                    }
#if UNITY_EDITOR
                    var path = t.GetAssetPath();
                    var go = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (go != null)
                    {
                        list.Add(go);
                    }
                    else
                    {
                        Debug.Log($"AssetDatabase.LoadAssetAtPath 加载失败！类型{typeof(T)}，文件路径 {path}");
                    }
#endif
                }
            }
            else
            {
                Debug.Log("不存在该目录" + fullPath);
            }
            return list;
        }
        public static void CreatDirectoryIfEmpty(string exportPath)
        {
            if (! Directory.Exists(exportPath) )
            {
                Directory.CreateDirectory(exportPath);
            }
        }

     
    }
}