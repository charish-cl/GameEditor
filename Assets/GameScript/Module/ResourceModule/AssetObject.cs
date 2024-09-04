using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public class AssetObject:ObjectBase
    {
        
        public List<AssetObject> Dependencies = new List<AssetObject>();
        
        
        public override void OnRelease()
        {
            
        }
    }
}