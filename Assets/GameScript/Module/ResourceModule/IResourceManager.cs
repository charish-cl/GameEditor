using System;
using System.Collections.Generic;

namespace TEngine
{
    public interface IResourceManager
    {  
        
        public AssetObject LoadAsset(string assetName);
        
        public AssetObject LoadAssetAsync(string assetName);
        
        public BundleObject LoadBundle(string bundleName);
        
        public BundleObject LoadBundleAsync(string bundleName);
        
        public void UnloadAsset(string assetName);

        public void UnloadAsset(object asset);

        public void UnloadBundle(string bundleName);

        public void UnloadBundle(object bundle);
        public void UnloadUnusedAssets();
    }
}