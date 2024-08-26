using System;
using UnityEngine;

namespace GameDevKit
{
    //TODO：感觉没多大用，不同项目输入其实 GetMousePosInWord 这种应该在MapSystem相关的类里，输入只管输入就好了
    public class FInput:Singleton<FInput>
    {
        
        public  Vector3 GetMousePosInWord()
        {
            return FCamera.Main.ScreenToWorldPoint(Input.mousePosition);
        }
        public  Vector3 GetMousePosInWordByRay()
        {
            var ray= FCamera.Main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray,out raycastHit))
            {
                return raycastHit.point;
            }
            return Vector3.zero;
        }
        public void OnMouseDown(int i, Action action)
        {
            if(Input.GetMouseButtonDown(i)){action?.Invoke();}
        }

        public void SetMoveByBaseInput(Transform transform,float speed)
        {
            var dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            transform.Translate(dir*Time.deltaTime*speed);
        }

        public bool OnMouseClickExit()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }
    
    
}