using System.Collections.Generic;
using System.IO;
using UnityEngine.Pool;

namespace TEngine
{
    public class ResourceModule: Module
    {
        
        public HashSet<string> LoadingAsset;
        
        public Dictionary<string, AssetObject> AssetDict;

        private IObjectPool<AssetObject> m_AssetPool;
        private IObjectPool<BundleObject> m_ResourcePool;


        public void Init( )
        {
         
            File.ReadAllText("Assets/GameScript/Module/ResourceModule/ResourceModule.cs");

        }
        
    }
}