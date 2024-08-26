using UnityEditor;
using UnityEngine;

namespace TEngine
{
    internal class ResourceManager:ModuleImp,IResourceManager
    {
        // public void LoadAsset(string assetName, System.Action<ObjectBase> callback)
        // {
        //     //TODO: load asset from assetbundle
        //     // ObjectBase asset = AssetDatabase.LoadAssetAtPath(assetName, typeof(ObjectBase));
        //     // callback(asset);
        // }

        internal override void Shutdown()
        {
            
        }

        public void LoadAsset(string assetName, System.Action<Object> callback)
        {
            
        }
    }
}