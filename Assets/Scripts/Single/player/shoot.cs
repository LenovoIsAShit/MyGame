using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���,�����ӵ�
/// 
///     ��һ��ֻ�ᷢ��һ��
/// 
/// </summary>


public class shoot : MonoBehaviour
{
    bool shooting;
    //�����

    private void Start()
    {
        shooting = false;
    }

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && shooting == false && UImanager.now == UImanager.state.offUI)
        {
            shooting = true;
            Make_bomb();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && shooting == true)
        {
            shooting = false;
        }
    }

    void Make_bomb()
    {
        GameObject bomb = OP.instance.Get("fire_bomb");
        bomb.GetComponent<bomb_test>().stop = false;
        bomb.transform.position = move.player.transform.position + new Vector3(0f, 0.5f, 0f);
        bomb.GetComponent<bomb_test>().Set_check_state(true);
    }
}
