using UnityEngine;

namespace GameDevKit.Utility
{
    //简单2d移动，根据输入改变位置
    public class SimpleMove : SimpleUnit
    {
        
        private void Update()
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            transform.Translate(new Vector3(x, y, 0) * speed * Time.deltaTime);
        }
    }
    
}