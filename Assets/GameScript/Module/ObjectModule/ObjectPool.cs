namespace TEngine
{
    public class ObjectPool<T> where T : ObjectBase
    {
        public GameFrameworkDictionary<string, Object<T>> mPool = new GameFrameworkDictionary<string, Object<T>>();
        public void Init()
        {
          
        }
        public void Update()
        {
            GameTime.StartFrame();
        }
    }
}