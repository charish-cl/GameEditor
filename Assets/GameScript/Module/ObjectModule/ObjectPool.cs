using System;

namespace TEngine
{
    public class ObjectPool<T> :IObjectPool<T> where T : ObjectBase
    {
        
        public string Name { get; }
        public string FullName { get; }
        public Type ObjectType { get; }
        public int Count { get; }
        public int CanReleaseCount { get; }
        public bool AllowMultiSpawn { get; }
        public float AutoReleaseInterval { get; set; }
        public int Capacity { get; set; }
        public float ExpireTime { get; set; }
        public int Priority { get; set; }
        
        
        public GameFrameworkMultiDictionary<string, Object<T>> mPool = new GameFrameworkMultiDictionary<string, Object<T>>();
        
        public void Init()
        {
          
        }
        public void Update()
        {
            GameTime.StartFrame();
        }
        public void Register(T obj, bool spawned)
        {
            
        }

        public bool CanSpawn()
        {
            throw new NotImplementedException();
        }

        public bool CanSpawn(string name)
        {
            throw new NotImplementedException();
        }

        public T Spawn()
        {
            throw new NotImplementedException();
        }

        public T Spawn(string name)
        {
            throw new NotImplementedException();
        }

        public void Unspawn(T obj)
        {
            throw new NotImplementedException();
        }

        public void Unspawn(object target)
        {
            throw new NotImplementedException();
        }

        public void SetLocked(T obj, bool locked)
        {
            throw new NotImplementedException();
        }

        public void SetLocked(object target, bool locked)
        {
            throw new NotImplementedException();
        }

        public void SetPriority(T obj, int priority)
        {
            throw new NotImplementedException();
        }

        public void SetPriority(object target, int priority)
        {
            throw new NotImplementedException();
        }

        public bool ReleaseObject(T obj)
        {
            throw new NotImplementedException();
        }

        public bool ReleaseObject(object target)
        {
            throw new NotImplementedException();
        }

        public void Release()
        {
            throw new NotImplementedException();
        }

        public void Release(int toReleaseCount)
        {
            throw new NotImplementedException();
        }

        public void ReleaseAllUnused()
        {
            throw new NotImplementedException();
        }
    }
}