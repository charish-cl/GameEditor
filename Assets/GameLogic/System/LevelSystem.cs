using TEngine;
using UnityEngine;

namespace GameLogic
{
    [Update]
    public class LevelSystem:BaseLogicSys<LevelSystem>
    {

        public override void OnStart()
        {
            base.OnStart();
            
            CreateTank(MapConfig.Instance.PlayerTank,true);
            
            //TODO: create other tanks

            for (int i = 0; i < MapConfig.Instance.EnemyTankCount; i++)
            {
                //随即上下左右偏移
                var pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                var enemyTank = CreateTank(MapConfig.Instance.EnermyTank,false);
                enemyTank.transform.position = pos;
            }
        }

        public TankEntity PlayerTank { get; set; }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            Log.Info("LevelSystem OnUpdate");
            
            //控制玩家tank的移动;
            PlayerTank.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * PlayerTank.Speed,Input.GetAxis("Vertical") * Time.deltaTime * PlayerTank.Speed,0);
            //转向只有上下左右，向右为正
            
            PlayerTank.transform.Rotate(0, 0,Input.GetAxis("Horizontal") * Time.deltaTime *10);
        }

        public TankEntity CreateTank(MapConfig.TankInfo tankInfo,bool isPlayer)
        {
            Log.Info("CreateTank");
            var tank = GameObject.Instantiate(tankInfo.TankPrefab, tankInfo.SpawnPoint);
            TankEntity tankEntity = null;
            //TODO: create tank
            if (isPlayer)
            {
                tankEntity = tank.AddComponent<TankEntity>();
                
                PlayerTank = tankEntity;
                PlayerTank.Init(tankInfo);
                
            }
            else
            {
                tankEntity= tank.AddComponent<TankEntity>();
                //TODO: create enemy tank
            }
            return tankEntity;
        }
        
        
        
    }
}