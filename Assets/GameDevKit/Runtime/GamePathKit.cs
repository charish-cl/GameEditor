using System;
using System.IO;

namespace GameDevKit
{
    /// <summary>
    /// 游戏资源路径kit
    /// </summary>
    public  static class GamePathKit
    {
        public  static string GetFullPath(this FileInfo file)
        {
            return  $"{file.DirectoryName}\\{file.Name}";
        }
        /// <summary>
        /// Unity不支持相对路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public  static string GetAssetPath(this FileInfo file)
        {
            var path = file.GetFullPath();
            return  path.Substring(path.IndexOf("Assets", StringComparison.Ordinal));
        }
        
        public  static string GetResourecesPath(this string path)
        {
            path=path.Substring(path.IndexOf("Resources", StringComparison.Ordinal));
            return path.Replace("Resources/", "");
        }
    }
}