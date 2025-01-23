using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行为树节点-发现敌人后，根据A*算法得到的路径，找到玩家
/// 
/// 
/// </summary>

public class move_to_palyer : Action
{
    public SharedGameObjectList path;
    //路径
    public SharedInt path_now;
    //第几个位置点

    float speed = 0, accelaration = 3f;
    //怪物速度，加速度

    public override TaskStatus OnUpdate()
    {
        if (path_now.Value < path.Value.Count)
        {
            Vector3 pos = path.Value[path_now.Value].transform.position;
            //当前目标点
            SpeedJudge();
            //更新速度
            Vector3 dir = pos - transform.position;
            //方向

            if (dir.sqrMagnitude >= Time.deltaTime * Time.deltaTime * speed * speed)
            {
                transform.position += (dir.normalized * Time.deltaTime * speed);
            }
            else
            {
                transform.position = pos;
                path_now.Value++;
                //下一个目标点

            }
            return TaskStatus.Running;
            //没走完
        }
        else
        {
            return TaskStatus.Success;
            //走完了
        }
    }

    void SpeedJudge()
    {
        if (speed < accelaration)
        {
            speed += accelaration * Time.deltaTime;
        }
        else
        {
            speed = accelaration;
        }
    }
}
