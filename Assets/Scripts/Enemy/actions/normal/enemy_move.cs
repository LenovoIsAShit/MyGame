using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BehaviorDesigner.Runtime.BehaviorManager;

/// <summary>
/// �����ƶ��ڵ�-��Ŀ��Ѳ�ߵ�ǰ��
/// 
/// 
/// </summary>

public class enemy_move : Action
{
    float speed=0,accelaration=3f;
    //�����ٶȣ����ٶ�

    public SharedVector3 pos;


    public override TaskStatus OnUpdate()
    {
        Vector3 tp = transform.position;
        tp.Set(tp.x, 0f, tp.z);

        SpeedJudge();

        if ( (tp- pos.Value).sqrMagnitude>= speed * Time.deltaTime* speed * Time.deltaTime)
        {
            Vector3 dir = (pos.Value - tp).normalized;
            transform.position += (speed * Time.deltaTime * dir);

            return TaskStatus.Running;
        }
        else
        {
            Vector3 vec = transform.position;
            vec.Set(pos.Value.x, vec.y, pos.Value.z);
            transform.position = vec;
            return TaskStatus.Success;
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
