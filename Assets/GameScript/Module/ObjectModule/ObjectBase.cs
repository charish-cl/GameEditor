using System;

namespace TEngine
{
    public class ObjectBase
    {
        private string name;

        private int priority;

        /// <summary>
        /// 是否锁定
        /// </summary>
        private bool Lock;


        private object Target;
        
        
        public DateTime LastTime;
        
        /// <summary>
        /// 是否可以释放
        /// </summary>
        public virtual bool CanReleaseFlag => true;
        
        
        
    }
}