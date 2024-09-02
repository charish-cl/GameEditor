using System.Collections.Generic;

namespace TEngine
{
    public class ResourceModule: Module
    {
        public HashSet<string> LoadingAsset;
        
        public Dictionary<string, AssetObject> AssetDict;
    }
}