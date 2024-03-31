using UnityEngine;

namespace GameDevKit
{
    //包含一个GameObject和数量,bool位置是否随机
    [System.Serializable]
    public class FactoryElement
    {
        public GameObject prefab;
        public int count;
        public bool isRandomPosition;
    }
}