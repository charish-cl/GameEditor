using System;
using System.Collections.Generic;
using Object = System.Object;

namespace GameDevKit
{
    /// <summary>
    /// 想了想感觉还是ET那种类继承的方式比较好
    /// </summary>
    public class BaseAttribute: Attribute
    {
        public Type AttributeType { get; }

        public BaseAttribute()
        {
            this.AttributeType = this.GetType();
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EventAttribute:BaseAttribute
    {
        
    }
    [Event]
    public abstract class AEvent<T>
    {
        public abstract void Handle(T arg);
    }
    /// <summary>
    /// simple事件框架
    /// </summary>
    public class FEvent
    {
        public static Dictionary<string, List<Action<Object>>> allEvent=new Dictionary<string, List<Action<object>>>();
        
        public static void Register(string key,Action<Object> action)
        {
            if (!allEvent.ContainsKey(key))
            {
                allEvent.Add(key,new List<Action<object>>());
            }
            allEvent[key].Add(action);
        }
        public static void UnRegister(string key,Action<Object> action)
        {
            if (allEvent.ContainsKey(key))
            {
                if(allEvent[key].Contains(action))
                     allEvent[key].Remove(action);
            }
        }
        public static void Fire(string key,Object arg)
        {
            if (allEvent.ContainsKey(key))
            {
                var actions = allEvent[key];
                foreach (var action in actions)
                {
                    action.Invoke(arg);
                }
            }
            else
            {
                allEvent.Add(key,new List<Action<object>>());
                var actions = allEvent[key];
                foreach (var action in actions)
                {
                    action.Invoke(arg);
                }
            }
    
        }
    }
}