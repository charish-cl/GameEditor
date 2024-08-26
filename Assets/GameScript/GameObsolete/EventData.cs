using System;
using System.Collections.Generic;
using GameDevKit.GameLogic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameDevKit.Obosolete
{
    /// <summary>
    /// 这个当时是用来生成事件代码的
    /// </summary>
    [CreateAssetMenu]
    public class EventData:ScriptableObject
    {
        [TableList(DrawScrollView = true, MinScrollViewHeight = 500)] [LabelText("事件列表")]
        public List<EventUnitData> EventDic;
        [FolderPath]
        public string outpath = "Assets/Script/Generate/";
        public string Name = "EventID";
        
        [Serializable]
        public struct EventUnitData
        {
            public string EventName;
            public List<string> paramLis;

            public override string ToString()
            {
                return $"public static readonly int {EventName} = StringId.StringToHash(\"{EventName}\");";
            }

            [Button("触发")]
            void CopySend()
            {
                var eventClass = ReflectionHelper.GetClass<EventData>().Name;
                SystemHelper.Copy($"GameEventMgr.Instance.Send({eventClass}.{EventName},);");
             
            }

            [Button("注册")]
            void CopyRegister()
            {
                var eventClass = ReflectionHelper.GetClass<EventData>().Name;
                SystemHelper.Copy($"GameEventMgr.Instance.AddEventListener<{paramLis.Split(",")}>({eventClass}.{EventName},On{EventName});");
            }
        }
        [Button]
        public void GenerateCode()
        {
            FCodeBuilder builder = new FCodeBuilder();
            builder.AddNameSpace("TEngine");
            builder.AddClass($"{Name}");
            foreach (var eventUnitData in EventDic)
            {
                builder.AppendLine(eventUnitData.ToString());
            }
            FileKit.CreateScriptAndRefresh(outpath,Name,builder.ToString());
        }
        
    }
}

