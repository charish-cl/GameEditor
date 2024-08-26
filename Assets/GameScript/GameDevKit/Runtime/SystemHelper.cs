using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameDevKit
{
    //TODO；意义不明确，只有RunProcess真正与System相关的方法
    public class SystemHelper
    {
         
        /// <summary>
        /// 通过系统默认的程序直接打开文件
        /// </summary>
        /// <param name="fullNamePath">文件的完整路径</param>
        public void OpenFile(string fullNamePath)
        {
            System.Diagnostics.Process.Start(fullNamePath);
        }
        public static void OpenFolder(string folder)
        {
            Process.Start("Explorer.exe", folder.Replace('/', '\\'));
        }
        public static void RunProcess(string path, string arguments)
        {
            Process.Start(path, arguments);
        }
        public static void Copy(string s)
        {
            GUIUtility.systemCopyBuffer = s;
            Debug.Log($"复制成功 {s}");
        }
        /// <summary>
        /// 获取剪切板内容
        /// </summary>
        /// <param name="s"></param>
        public static List<string> GetPaste()
        {
           return GUIUtility.systemCopyBuffer.Split(',').ToList();
        }
      
        //数字转中文
        public static string NumberToChinese(int num)
        {
            string[] chineseNum = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            string[] chineseUnit = new string[] { "", "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百", "千", "万", "十", "百", "千" };
            string result = "";
            int unit = 0;
            while (num > 0)
            {
                int n = num % 10;
                if (n != 0)
                {
                    result = chineseNum[n] + chineseUnit[unit] + result;
                }
                else
                {
                    if (unit == 0 || chineseUnit[unit] == "万" || chineseUnit[unit] == "亿")
                    {
                        result = chineseNum[n] + result;
                    }
                    else if (chineseUnit[unit] != chineseUnit[unit - 1])
                    {
                        result = chineseNum[n] + result;
                    }
                }
                num = num / 10;
                unit++;
            }
            return result;
        }
        
        
    }
}