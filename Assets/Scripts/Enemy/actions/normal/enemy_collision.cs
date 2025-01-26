using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人行为树节点-碰撞检测
/// 
/// 
/// </summary>

public class enemy_collision : Action
{
    public override TaskStatus OnUpdate()
    {
        if (collide.iscollided == false)
        {
            return TaskStatus.Running;
        }
        else
        {
            collide.iscollided = false;
            return TaskStatus.Success;
        }
    }
}
