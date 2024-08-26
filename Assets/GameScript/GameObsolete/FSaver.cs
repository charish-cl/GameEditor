using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameDevKit
{
    //参考JEngine的实现,现在看来感觉不是很优雅
    public class FSaver
    {
        private static string FSaverKey = "FSaver";
        private static void AddJSaverKeys(string key)
        {
            var s = PlayerPrefs.GetString(FSaverKey, key);
            if (!s.Split(',').ToList().Contains(key))
            {
                var sb = new StringBuilder(s);
                sb.Append($",{key}");
                s = sb.ToString();
            }
            PlayerPrefs.SetString(FSaverKey, s);
        }
        public static bool HasData(string dataName)
        {
            return PlayerPrefs.HasKey(dataName);
        }
        /// <summary>
        /// Delete specific data
        /// </summary>
        /// <param name="dataName"></param>
        public static void DeleteData(string dataName)
        {
            PlayerPrefs.DeleteKey(dataName);
        }
        /// <summary>
        /// Delete all data
        /// </summary>
        public static void DeleteFAll()
        {
            var s = PlayerPrefs.GetString(FSaverKey);
            if (!String.IsNullOrEmpty(s))
            {
                var keys = s.Split(',');
                foreach(var key in keys)
                {
                    PlayerPrefs.DeleteKey(key);
                }
            }
        }      
        public static  void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void GetAndAddString(string key, string data)
        {
            if(!PlayerPrefs.HasKey(key)) 
                PlayerPrefs.SetString(key,"");
            string s=PlayerPrefs.GetString(key);
                
            foreach (var s1 in s.Split(','))
            {
                if(s1==data) return; 
            }
            PlayerPrefs.SetString(key,$"{s},{data}");
        }

        public static List<string> GetAndSpiltString(string key)
        {
            if(!PlayerPrefs.HasKey(key)) 
                PlayerPrefs.SetString(key,"");
            string s=PlayerPrefs.GetString(key);
           
            return s.Split(',').Where(e=>!string.IsNullOrWhiteSpace(e)).ToList();
        }
        public static  void SetString(string key,string data)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                AddJSaverKeys(key);
            }
            PlayerPrefs.SetString(key,data);
        }

        public static void SetLisStr(string key,List<string> list)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var s in list)
            {
                builder.Append(s);
                builder.Append(",");
            }
            PlayerPrefs.SetString(key,builder.ToString());
        }
        public static  void SetBool(string key,bool data)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                AddJSaverKeys(key);
            }
            PlayerPrefs.SetString(key,data.ToString());
        }
        public static  bool GetBool(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogError($"不存在key为 {key} 的数据");
                return false;
            }
            return  bool.Parse(PlayerPrefs.GetString(key)) ;
        }  
        public static string GetString(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogError($"不存在key为 {key} 的数据");
                return null;
            }
            return PlayerPrefs.GetString(key);
        }
    }
}