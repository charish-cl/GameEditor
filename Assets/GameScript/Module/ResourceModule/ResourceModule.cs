using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TEngine
{
    public class ResourceModule: Module
    {
        private string RESOURCE_ASSET_NAME="Bundle";
        private string BUNDLE_ASSET_NAME="Resource";
        private string PrefixPath { get; set; }
        private string Platform { get; set; }

        private void Start()
        {
            PrefixPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../AssetBundle")).Replace("\\", "/");
            PrefixPath += $"/{Platform}";
        }

        public void LoadConfig()
        {
            string manifestBunldeFile="Assets/AssetBundles/manifest.assetbundle";
            AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile(manifestBunldeFile, 0, 0);

            //想了下感觉也没要
            // TextAsset textAsset = manifestAssetBundle.LoadAsset<TextAsset>("AssetBundleManifest");
            // TextAsset resourceTextAsset = manifestAssetBundle.LoadAsset(RESOURCE_ASSET_NAME) as TextAsset;
            // TextAsset bundleTextAsset = manifestAssetBundle.LoadAsset(BUNDLE_ASSET_NAME) as TextAsset;
            
            //
            // resourceTextAsset.text
        }
        private string GetFileUrl(string assetUrl)
        {
            return $"{PrefixPath}/{assetUrl}";
        }

        private string GetPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                default:
                    throw new System.Exception($"未支持的平台:{Application.platform}");
            }
        }

    }
}