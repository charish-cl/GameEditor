using UnityEngine;

namespace GameDevKit.Utility
{
    //圆类
    public class Circle
    {
        public Vector3 center;
        public float radius;

        public Circle(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }
}