using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace TEngine
{
    internal class EditorResourceManager:ModuleImp,IResourceManager
    {
        // public void LoadAsset(string assetName, System.Action<ObjectBase> callback)
        // {
        // File.ReadAllText("Assets/GameScript/Module/ResourceModule/ResourceModule.cs");
        //     //TODO: load asset from assetbundle
        //     // ObjectBase asset = AssetDatabase.LoadAssetAtPath(assetName, typeof(ObjectBase));
        //     // callback(asset);
        // }

        internal override void Shutdown()
        {
          
        }

        public HashSet<string> m_LoadingAsset;
        public Dictionary<string, AssetObject> m_AssetDict;
        public ObjectPool<AssetObject> m_AssetPool;
        public ObjectPool<BundleObject> m_ResourcePool;

        public void Init( )
        {
            m_LoadingAsset = new HashSet<string>();
            m_AssetDict = new Dictionary<string, AssetObject>();

            var poolModule = GameModule.Get<ObjectPoolModule>();
            m_AssetPool = poolModule.CreateObjectPool<AssetObject>();
            m_ResourcePool = poolModule.CreateObjectPool<BundleObject>();
        }

        public AssetObject LoadAsset(string assetName, Action<AssetObject> callback)
        {
            //看看池子里面有没有这个资源
            AssetObject assetObj = null;
            if (m_AssetDict.ContainsKey(assetName))
            {
                assetObj =  m_AssetPool.Spawn(assetName);
                callback(assetObj);
                return assetObj;
            }

            //如果没有，看看有没有正在加载
            if (m_LoadingAsset.Contains(assetName))
            {
                //如果正在加载，就等待回调
                return null;
            }

            //如果没有正在加载，就开始加载
#if UNITY_EDITOR
           var  asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(assetName);
           
           
#endif
            return null;
        }

        public AssetObject LoadAsset(string assetName)
        {
            throw new NotImplementedException();
        }

        public AssetObject LoadAssetAsync(string assetName)
        {
            throw new NotImplementedException();
        }

        public BundleObject LoadBundle(string bundleName)
        {
            throw new NotImplementedException();
        }

        public BundleObject LoadBundleAsync(string bundleName)
        {
            throw new NotImplementedException();
        }

        public void UnloadAsset(string assetName)
        {
            throw new NotImplementedException();
        }

        public void UnloadAsset(object asset)
        {
            throw new NotImplementedException();
        }

        public void UnloadBundle(string bundleName)
        {
            throw new NotImplementedException();
        }

        public void UnloadBundle(object bundle)
        {
            throw new NotImplementedException();
        }

        public void UnloadUnusedAssets()
        {
            throw new NotImplementedException();
        }
    }
}