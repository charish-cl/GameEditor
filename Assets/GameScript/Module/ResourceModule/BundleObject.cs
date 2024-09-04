using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public class BundleObject:ObjectBase
    {
        public AssetBundle bundle;
        
        public List<BundleObject> Dependencies = new List<BundleObject>();
        
        AssetBundleRequest request;
        
        public bool IsLoadDone;
        
        public void LoadBundle(string bundleName)
        {
            bundle = AssetBundle.LoadFromFile(bundleName);
        }
        
        public Object LoadAsset<T>(string AssetName) where T : Object
        {
            var asset = bundle.LoadAsset<T>(AssetName);
            return asset;
        }
        
       
        public void LoadAssetAsync<T>(string AssetName) where T : Object
        {
             request = bundle.LoadAssetAsync<T>(AssetName);
        }


        public bool Update()
        {
            if (IsLoadDone)
            {
                return true;
            }
            
            if (request.isDone)
            {
                for (int i = 0; i < Dependencies.Count; i++)
                {
                    if (!Dependencies[i].IsLoadDone)
                    {
                        return false;
                    }
                }
            }

            IsLoadDone = true;
            
            return true;
        }
        
        public override void OnRelease()
        {
            bundle.Unload(true);
        }
    }
}