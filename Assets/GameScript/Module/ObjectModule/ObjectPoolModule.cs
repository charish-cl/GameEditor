using System;

namespace TEngine
{
    public class ObjectPoolModule:Module
    {
      
    }


    public abstract class ObjectPoolImp : ModuleImp
    {
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
            
            //轮询所有对象池，回收对象池中的对象
        }
    }
}