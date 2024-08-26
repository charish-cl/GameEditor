namespace TEngine
{
    public interface IResourceManager
    {
        public void LoadAsset(string assetName, System.Action<UnityEngine.Object> callback);
    }
}