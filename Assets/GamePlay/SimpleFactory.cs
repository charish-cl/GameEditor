using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameDevKit.Utility
{
    //通用工厂类,它包含一个List<FactoryElement> ,start方法会遍历这个List,并且实例化每个FactoryElement中的GameObject
    //如果FactoryElement中的isRandomPosition为true,则会随机位置,它有一个属性Vector2表示随机位置范围
    public class SimpleFactory : MonoBehaviour
    {
        [TableList]
        public List<FactoryElement> factoryElements;
        public Vector2 randomPositionRange = new Vector2(10, 10);
        public void Start()
        {
            foreach (var factoryElement in factoryElements)
            {
                for (int i = 0; i < factoryElement.count; i++)
                {
                    var go = Instantiate(factoryElement.prefab);
                    if (factoryElement.isRandomPosition)
                    {
                        go.transform.position = new Vector3(Random.Range(-randomPositionRange.x, randomPositionRange.x),
                            Random.Range(-randomPositionRange.y, randomPositionRange.y), 0);
                    }
                }
            }
        }
    }
  
   
}