using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameDevKit.Utility
{
    //简单的跟随脚本,包含一个跟随目标和一个跟随速度,在Update中改变位置,使其跟随目标,如果跟随目标为null,则不跟随,并且在OnDestroy中取消订阅
    //包含一个Tag,在Start中根据Tag查找目标,并且订阅,用Odin的ValueDropDown来显示所有的Tag,并且可以选择
    public class SimpleFollow : SimpleUnit
    {
        [TabGroup("Tag")]
        [ValueDropdown("GetTags")]
        public string targetTag;

        private Transform _target;
        private void Start()
        {
            _target = GameObject.FindGameObjectWithTag(targetTag).transform;
        }

        private void Update()
        {
            if (_target == null) return;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            _target = null;
        }
       
    }
   
    
    
  
}