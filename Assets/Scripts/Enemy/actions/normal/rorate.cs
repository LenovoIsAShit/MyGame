using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 敌人行为树节点-转向
/// 
/// 
/// </summary>

public class rorate : Action
{
    public SharedVector3 pos;
    public override TaskStatus OnUpdate()
    {
        Vector3 tp = transform.position;
        tp.Set(tp.x, 0f, tp.z);

        Vector3 dir = pos.Value - tp;


        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 0.02f);
            return TaskStatus.Running;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(dir);
            return TaskStatus.Success;
        }
    }
}
