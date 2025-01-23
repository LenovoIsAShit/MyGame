using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϊ���ڵ�-���ֵ��˺󣬸���A*�㷨�õ���·�����ҵ����
/// 
/// 
/// </summary>

public class move_to_palyer : Action
{
    public SharedGameObjectList path;
    //·��
    public SharedInt path_now;
    //�ڼ���λ�õ�

    float speed = 0, accelaration = 3f;
    //�����ٶȣ����ٶ�

    public override TaskStatus OnUpdate()
    {
        if (path_now.Value < path.Value.Count)
        {
            Vector3 pos = path.Value[path_now.Value].transform.position;
            //��ǰĿ���
            SpeedJudge();
            //�����ٶ�
            Vector3 dir = pos - transform.position;
            //����

            if (dir.sqrMagnitude >= Time.deltaTime * Time.deltaTime * speed * speed)
            {
                transform.position += (dir.normalized * Time.deltaTime * speed);
            }
            else
            {
                transform.position = pos;
                path_now.Value++;
                //��һ��Ŀ���

            }
            return TaskStatus.Running;
            //û����
        }
        else
        {
            return TaskStatus.Success;
            //������
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
