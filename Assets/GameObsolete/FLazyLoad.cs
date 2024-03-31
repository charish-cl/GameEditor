namespace GameDevKit.Obosolete
{
    /// <summary>
    /// 梦里写的代码
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FLazyLoad<T>
    {
        private T item;
        public delegate T Event_Get();
        public Event_Get GetFunc;
        public T Item
        {
            get
            {
                if (item == null)
                {
                    item = GetFunc.Invoke();
                }
                return item;
            }
        }
    }
}