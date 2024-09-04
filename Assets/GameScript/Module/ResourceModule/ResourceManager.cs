using System;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public class ResourceManager:ModuleImp,IResourceManager
    {
           // public void LoadAsset(string assetName, System.Action<ObjectBase> callback)
        // {
        // File.ReadAllText("Assets/GameScript/Module/ResourceModule/ResourceModule.cs");
        //     //TODO: load asset from assetbundle
        //     // ObjectBase asset = AssetDatabase.LoadAssetAtPath(assetName, typeof(ObjectBase));
        //     // callback(asset);
        // }

        public HashSet<string> m_LoadingAsset;
        public Dictionary<string, AssetObject> m_AssetDict;
        public Dictionary<string, BundleObject> m_BundleDict;
        
        //资源依赖
        Dictionary<string, List<string>> ResourceDependencyDic = new Dictionary<string, List<string>>();
        
        public ObjectPool<AssetObject> m_AssetPool;
        public ObjectPool<BundleObject> m_ResourcePool;


        public List<BundleObject> m_UnloadBundleList;
        public List<AssetObject> m_UnloadAssetList;
        
        /// <summary>
        /// bundle依赖管理信息
        /// </summary>
        AssetBundleManifest m_AssetBundleManifest;

        string manifestfilePath;

        public void Init( ) 
        {
            m_LoadingAsset = new HashSet<string>();
            m_AssetDict = new Dictionary<string, AssetObject>();
            
            m_UnloadBundleList = new List<BundleObject>();
            m_UnloadAssetList = new List<AssetObject>();
            
            var poolModule = GameModule.Get<ObjectPoolModule>();
            m_AssetPool = poolModule.CreateObjectPool<AssetObject>();
            m_ResourcePool = poolModule.CreateObjectPool<BundleObject>();
        }

        public void Initialize()
        {
            AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile(manifestfilePath);
            UnityEngine.Object[] objs = manifestAssetBundle.LoadAllAssets();

            if (objs.Length == 0)
            {
                throw new Exception($"{nameof(ResourceManager)}.{nameof(Initialize)}() AssetBundleManifest load fail.");
            }

            m_AssetBundleManifest = objs[0] as AssetBundleManifest;
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
            //看看池子里面有没有这个资源
            if (m_BundleDict.TryGetValue(bundleName,out var bundleObj))
            {
                return m_ResourcePool.Spawn(bundleName); 
            }
            //如果没有，看看有没有正在加载
            if (m_LoadingAsset.Contains(bundleName))
            {

            }

            var dependencies = m_AssetBundleManifest.GetDirectDependencies(bundleName);
            
            
            var bundleObject = new BundleObject();
           
            return null;
        }

        public BundleObject LoadBundleAsync(string bundleName)
        {
            throw new NotImplementedException();
        }

        public void UnloadAsset(string assetName)
        {
            
        }

        public void UnloadAsset(object asset)
        {
            m_UnloadAssetList.Add(asset as AssetObject);
        }

        public void UnloadBundle(string bundleName)
        {
            
        }

        public void UnloadBundle(object bundle)
        {
            m_UnloadBundleList.Add(bundle as BundleObject);
        }

        public void UnloadUnusedAssets()
        {
            throw new NotImplementedException();
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);


            if (m_UnloadAssetList.Count > 0)
            {
                foreach (var asset in m_UnloadAssetList)
                {
                    m_AssetPool.UnSpawn(asset);
                    
                    //依赖也要卸载
                    foreach (var dep in asset.Dependencies)
                    {
                        m_AssetPool.UnSpawn(dep);
                    }
                }
            }

            if (m_UnloadBundleList.Count>0)
            {

                foreach (var bundle in m_UnloadBundleList)
                {
                    m_ResourcePool.UnSpawn(bundle);
                    //依赖也要卸载

                    foreach (var asset in bundle.Dependencies)
                    {
                        m_ResourcePool.UnSpawn(asset);
                    }
                }
            }
        }

        internal override void Shutdown()
        {
            
        }
    }
}