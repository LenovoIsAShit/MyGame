using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人碰撞组件，用于碰撞检测
/// 
/// 挂载于Monster
/// </summary>
public class collide : MonoBehaviour
{
    public static  bool iscollided;
    //碰撞标志

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
