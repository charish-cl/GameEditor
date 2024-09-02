using System;

namespace TEngine
{
    public abstract class ObjectBase
    {
        private string name;

        private int priority;

        /// <summary>
        /// 是否锁定
        /// </summary>
        private bool _Lock;


        private object target;
        
        
        private DateTime lastUseTime;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Priority
        {
            get => priority;
            set => priority = value;
        }

        public bool Lock
        {
            get => _Lock;
            set => _Lock = value;
        }

        public object Target
        {
            get => target;
            set => target = value;
        }

        public DateTime LastUseTime
        {
            get => lastUseTime;
            set => lastUseTime = value;
        }

        /// <summary>
        /// 是否可以释放
        /// </summary>
        public virtual bool CanReleaseFlag => true;

        /// 生成，回收，释放
        public virtual void OnSpawn(){}
        public virtual void OnUnSpawn(){}
        public abstract void OnRelease();



    }
}