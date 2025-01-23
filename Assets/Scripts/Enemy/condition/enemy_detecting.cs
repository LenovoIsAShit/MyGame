using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行为树节点-检测是否发现玩家
/// 
/// 
/// 用于打断巡逻正常行为，改变为向玩家位置靠近
/// </summary>
public class enemy_detecting : Conditional
{
    public SharedBool detected;

    public override TaskStatus OnUpdate()
    {
        if (detected.Value == true)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }
}
