using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ƽű�
///     ���ƹ�������ɺ���Ϊ������
///     
/// 
///     ������Enemy_Controller
/// </summary>
public class EnemyController : MonoBehaviour
{
    static int max_num , now_num;
    //������������Ŀǰ��������

    public static GameObject[] enemies;
    //������
    public static bool[] alive;
    //��i�������Ƿ����

    void Awake()
    {
        Set_max_num(0);

        Start_make_enemy();
    }

    void LateUpdate()
    {
        Set_die();
    }

    void Start_make_enemy()
    {
        enemies = new GameObject[max_num];
        alive = new bool[max_num];

        for (; now_num < max_num; now_num++)
        {
            enemies[now_num] = Instantiate(Resources.Load<GameObject>("prefabs/Monster/Monster"));
            Vector3 pos = enemy_pos_get.Get_random_pos(now_num+1111);
            pos.Set(pos.x, 209.0265f, pos.z);

            enemies[now_num].transform.position = pos;
            alive[now_num] = true;
        }
    }

    public static void Set_max_num(int num)
    {
        now_num = 0;
        max_num = num;
    }

    public static List<GameObject> Get_enemies()
    {
        List<GameObject> res=new List<GameObject>();
        for(int i = 0; i < enemies.Length; i++)
        {
            if (alive[i] == true)
            {
                res.Add(enemies[i]);
            }
        }
        return res;
    }
    //�������еĴ�������

    public static void Set_die()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            if (alive[i] == true)
            {
                var cmp = enemies[i].GetComponent<Blood_system>();
                if (cmp.Get_hp() == 0)
                {
                    alive[i] = false;
                }
            }
        }

    }//���μ�������
}
