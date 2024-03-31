using System.Collections.Generic;
using UnityEngine;

namespace GameDevKit
{
    public class FObjectPool:MonoSingleton<FObjectPool>
    {
        /// <summary>
        ///对象池
        /// </summary>
        Dictionary<string, List<GameObject>> pool;
        
        void CreatPool()
        {
            pool = new Dictionary<string, List<GameObject>>();
        }
        /// <summary>
        /// 包含了实例化的方法
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public GameObject Get(GameObject go)
        {
            return GetPooledObject(go.name, go);
        }
        GameObject GetPooledObject(string key,GameObject go)
        {
            if (pool is null) 
                CreatPool();
            if (!pool.ContainsKey(key))
            {
                pool.Add(key, new List<GameObject>());
            }
            else
            {
                for (int i = 0; i < pool[key].Count; i++)
                {
                    if (!pool[key][i].activeInHierarchy)
                    {
                        pool[key][i].SetActive(true);
                        return pool[key][i];
                    }
                }
            }
            return CreatObject(key, go);
        }
        private GameObject CreatObject(string key, GameObject go)
        {
            //池子有键值但无可用的物品
            GameObject obj = GameObject.Instantiate(go);
            pool[key].Add(obj);
            return obj;
        }

        public void Clear()
        {
            
            foreach (var keyValuePair in pool)
            {
                foreach (var o in keyValuePair.Value)
                {
                    Destroy(o);
                }
            }

            pool = new Dictionary<string, List<GameObject>>();
        }
    }
}