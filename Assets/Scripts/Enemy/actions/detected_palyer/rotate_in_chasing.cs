using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϊ���ڵ�-׷��ʱ��ת
/// 
/// 
/// 
/// </summary>


public class rotate_in_chasing : Action
{
    public SharedGameObjectList path;
    //·��
    public SharedInt path_now;
    //�ڼ���λ�õ�

    public override TaskStatus OnUpdate()
    {
        Vector3 dir = move.player.transform.position - transform.position;
        dir.Set(dir.x, 0f, dir.z);
        dir=dir.normalized;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 0.05f);

        return TaskStatus.Running;
    }
}
