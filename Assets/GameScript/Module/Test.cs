using System;
using UnityEngine;

namespace TEngine
{
    public class Test:MonoBehaviour
    {
        private void Start()
        {
            GameModule.Timer.AddTimer(OnTimer, 1.0f);
        }

        private void OnTimer(object[] args)
        {
            Debug.Log("Timer");
        }
    }
}