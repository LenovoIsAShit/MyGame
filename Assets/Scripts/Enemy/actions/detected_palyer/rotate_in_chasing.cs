using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行为树节点-追逐时旋转
/// 
/// 
/// 
/// </summary>


public class rotate_in_chasing : Action
{
    public SharedGameObjectList path;
    //路径
    public SharedInt path_now;
    //第几个位置点

    public override TaskStatus OnUpdate()
    {
        Vector3 dir = move.player.transform.position - transform.position;
        dir.Set(dir.x, 0f, dir.z);
        dir=dir.normalized;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 0.05f);

        return TaskStatus.Running;
    }
}
