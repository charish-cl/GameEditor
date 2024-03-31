using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PathCreator
{
    public class BulletHell : MonoBehaviour
    {
        private const string group1 = "Default";
        private const string group2 = "路径";
        [TabGroup(group1)]
        [LabelText("初始点")] 
        public Vector3 initalPoint;
        
        [TabGroup(group1)]
        [LabelText("方向")] 
        public Vector3 forward;
        
        [TabGroup(group1)]
        [LabelText("子弹数量")]
        public int num;
        
        [TabGroup(group1)]
        [LabelText("速度")]
        public float speed;
        
        [TabGroup(group1)]
        [LabelText("加速度")]
        public float accelerate_speed;
        
        [TabGroup(group1)]
        [LabelText("角度")]
        [Range(0,360)]
        public float angle;
        
        
        [TabGroup(group1)]
        [LabelText("子弹间隔")]
        public float bulletInterval;
        
        [TabGroup(group2)]
        [LabelText("路径")]
        public Vector3[] paths;

        public GameObject bulletTemplate;
        private async void Start()
        {
            for (int i = 0; i < num; i++)
            {
                var bullet = Instantiate(bulletTemplate);
                bullet.transform.position = initalPoint;
                bullet.transform.rotation =Quaternion.Euler(new Vector3(0,0,i * angle));
                var bulletToken = bullet.transform.GetCancellationTokenOnDestroy();
                FlyBullet(bullet.transform, speed).Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(bulletInterval));
            }
          
        }

        [Button]
        public void SetUp()
        {
            bulletTemplate.transform.forward =new Vector3(0,0, angle);
        }
        private async UniTaskVoid FlyBullet(Transform bulletTransform, float speed)
        {
            float startTime = Time.time;
            Vector3 startPosition = bulletTransform.position;
            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, bulletTransform.GetCancellationTokenOnDestroy());
                var t = (Time.time - startTime);
                bulletTransform.position = startPosition + ((speed * t)+accelerate_speed*Mathf.Pow(t,2)*1/2)*bulletTransform.up;
            }
        }
    }
}