using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ű�
/// 
/// 
/// </summary>

public class Attack_dir : MonoBehaviour
{
    void LateUpdate()
    {
        Set_dir();
    }

    public Vector3 Get_dir()
    {
        var cmp = Local_camera.cme.GetComponent<Camera>();
        Vector3 mousedir = (cmp.ScreenToWorldPoint(Input.mousePosition) - Local_camera.cme.transform.position).normalized*Local_camera.cme.transform.position.y;
        Vector3 playerdir = move.player.transform.position - Local_camera.cme.transform.position;
        Vector3 pointdir = (mousedir - playerdir).normalized;
        pointdir.Set(pointdir.x, 0f, pointdir.z);
        return pointdir;
    }
    //��ȡ���ָ�����߷���

    void Set_dir()
    {
        transform.GetChild(2).transform.rotation = Quaternion.LookRotation(Get_dir());
    }
    //�޸ķ���
}
