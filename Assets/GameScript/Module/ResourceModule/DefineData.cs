using System;
using System.Collections.Generic;

namespace TEngine
{
    //这两个保存到配置里的类
    [Serializable]
    public class DefineData
    {
        public string ABName;
        public List<string> AssetList;
    }

    [Serializable]
    public class ResItem
    {
        public string ResName;

        /// <summary>
        /// 所属的AB包名
        /// </summary>
        public string ABName;

        public List<string> DependABList;
    }

    [Serializable]
    public class ABItemLis
    {
        public List<DefineData> mABList = new List<DefineData>();
    }

    [Serializable]
    public class ResItemLis
    {
        public List<ResItem> mResList = new List<ResItem>();
    }
}