using UnityEngine;

namespace GameDevKit
{
    //TODO：这个跟FInput一样没必要为一个很小的功能写这个类
    public  static class FCamera
    {
        static Camera mainCamera;

        public static Camera Main
        {
            get
            {
                if ( mainCamera == null )
                {
                    mainCamera = Camera.main;
                }

                return mainCamera;
            }
        }

        public static Vector3 WorldToScreenPoint(Vector3 position)
        {
            return Main.WorldToScreenPoint(position);
        }
        
        public static bool IsInView(this Camera camera,Vector3 worldPos)
        {
            Transform camTransform =camera.transform;
            Vector2 viewPos = camera.WorldToViewportPoint(worldPos);
            Vector3 dir = (worldPos - camTransform.position).normalized;
            float dot = Vector3.Dot(camTransform.forward, dir);     //判断物体是否在相机前面
            
            if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
                return true;
            else
                return false;
        }
        
    }
}