using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 敌人行为树节点-获得新目标巡逻点
/// </summary>

public class enemy_pos_get : Action
{
    public static Transform edgepoints= GameObject.FindGameObjectWithTag("EdgePoints").transform;
    //四个边界点的父节点

    public SharedVector3 pos;

    public override TaskStatus OnUpdate()
    {
        pos.Value = Get_random_pos(1);

        return TaskStatus.Success;
    }

    public static Vector3 Get_random_pos(int k)
    {
        float minx, minz, maxx, maxz;

        Vector3 p1 = edgepoints.TransformPoint(edgepoints.GetChild(0).transform.localPosition);
        Vector3 p2 = edgepoints.TransformPoint(edgepoints.GetChild(1).transform.localPosition);
        Vector3 p3 = edgepoints.TransformPoint(edgepoints.GetChild(2).transform.localPosition);
        Vector3 p4 = edgepoints.TransformPoint(edgepoints.GetChild(3).transform.localPosition);

        minx = Mathf.Min(Mathf.Min(p3.x, p4.x), Mathf.Min(p1.x, p2.x));
        maxx = Mathf.Max(Mathf.Max(p3.x, p4.x), Mathf.Max(p1.x, p2.x));
        minz = Mathf.Min(Mathf.Min(p3.z, p4.z), Mathf.Min(p1.z, p2.z));
        maxz = Mathf.Max(Mathf.Max(p3.z, p4.z), Mathf.Max(p1.z, p2.z));

        maxx -= minx;
        var rd = new System.Random(System.DateTime.Now.Millisecond*136516*k);
        var resx = ((float)rd.NextDouble()) * maxx;//获得随机数
        resx += minx;

        maxz -= minz;
        rd = new System.Random(System.DateTime.Now.Millisecond*k);
        var resz = ((float)rd.NextDouble()) * maxz;//获得随机数
        resz += minz;

        /*
        if (resx >= maxx) resx = maxx - 1;
        if (resx <= minx) resx = maxx + 1;
        if (resz >= maxz) resz = maxz - 1;
        if (resz <= minz) resz = minz + 1;
        */

        Vector3 vec = new Vector3(resx, 0f, resz);
        return vec;
    }
    //获得随机点
}
