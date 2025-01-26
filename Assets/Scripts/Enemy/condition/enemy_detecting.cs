using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϊ���ڵ�-����Ƿ������
/// 
/// 
/// ���ڴ��Ѳ��������Ϊ���ı�Ϊ�����λ�ÿ���
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
