using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ײ�����������ײ���
/// 
/// ������Monster
/// </summary>
public class collide : MonoBehaviour
{
    public static  bool iscollided;
    //��ײ��־

    public void Awake()
    {
        iscollided = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        iscollided = true;

        if (collision.gameObject.tag.Equals("SinglePlayer"))
        {
            var cld = collision.transform.GetComponent<Blood_system>();
            cld.Change_hp(-20);
        }
    }

}
