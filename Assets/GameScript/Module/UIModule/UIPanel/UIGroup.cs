using System;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public enum PanelLayer
    {
        None = 0,
        Prioritary = 1,
        Tutorial = 2,
        Blocker = 3,
    }
    /// <summary>
    /// 弹窗是不需要分层的，只有Panel需要
    /// </summary>
    [Serializable]
    public class UIGroup
    {
        public Transform Root { get; set; }
        
        public PanelLayer LayerName { get; set; }
        
        public UIGroup(Transform root, PanelLayer layerName)
        {
            Root = root;
            LayerName = layerName;
        }
    }

    public class UIGroupList
    {
        public List<UIGroup> paraLayers { get; set; }
        
        private Dictionary<PanelLayer, Transform> lookup;

        public Dictionary<PanelLayer, Transform> ParaLayerLookup {
            get {
                if (lookup == null || lookup.Count == 0) {
                    CacheLookup();
                }

                return lookup;
            }
        }

        private void CacheLookup()
        {
            lookup = new Dictionary<PanelLayer, Transform>();
            for (int i = 0; i < paraLayers.Count; i++) {
                lookup.Add(paraLayers[i].LayerName, paraLayers[i].Root);
            }
        }

        public UIGroupList(List<UIGroup> paraLayers)
        {
            this.paraLayers = paraLayers;
        }
    }
}
