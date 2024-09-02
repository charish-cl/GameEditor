using System.Collections.Generic;

namespace TEngine
{
    public class BundleObject:ObjectBase
    {
        public List<BundleObject> depenceBundleObjects;
        
        public List<AssetObject> assetObjects;
        
        
        public override void OnRelease()
        {
            
        }
    }
}