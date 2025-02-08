using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϊ���ڵ�-����A*����õ�·����
/// 
/// 
/// 
/// </summary>



public class enemy_get_path : Action
{
    public SharedGameObjectList path;

    public SharedInt path_now;

    public override TaskStatus OnUpdate()
    {
        for(int i = 0; i < path.Value.Count; i++)
        {
            GameObject t = path.Value[i];
            path.Value[i] = null;
            Object.Destroy(t);
        }
        
        path.Value.Clear();
        path_now.Value = 0;
        //����

        List<Vector3> list = grid.Astar(transform.position, move.player.transform.position);

        for(int i = 0; i < list.Count; i++)
        {
            path.Value.Add(new GameObject());
            path.Value[i].transform.position = list[i];
            //���·����
        }
        //���·����

        return TaskStatus.Success;
    }
}
