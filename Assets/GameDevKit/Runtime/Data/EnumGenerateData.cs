using System.Collections.Generic;
using System.Linq;
using GameDevKit.GameLogic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameDevKit
{
    [CreateAssetMenu]
    public class EnumGenerateData:ScriptableObject
    {
        [FolderPath]
        public string outpath = "Assets/Script/Generate/";
        public List<string> lis = new List<string>();
        
        
        /// <summary>
        /// 获取无后缀的名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetRawNonSuffix(string name)
        {
          return  name.Split('.')[0];
        }
        [Button]
        public void Generate()
        {
            foreach ( var li in lis )
            {
                var files = FileKit.GetAllSubFile(li);
                if ( files.Count==0 )
                {
                    continue;
                }
                
                var directoryName = files[0].Split('/').LastItem(1);
                var filename = $"Enum{directoryName}";
                var list = files.Select(e => GetRawNonSuffix(e.Split('/').LastItem(0))
                ).ToList();
                
                
                FCodeBuilder builder = new FCodeBuilder();
                builder.AddEnumFromLis(filename,list);
                FileKit.CreateScriptAndRefresh(outpath,filename,builder.ToString());
            }
        }
    }
}