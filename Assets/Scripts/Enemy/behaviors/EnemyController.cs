using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 怪物控制脚本
///     控制怪物的生成和行为树挂载
///     
/// 
///     挂载于Enemy_Controller
/// </summary>
public class EnemyController : MonoBehaviour
{
    static int max_num , now_num;
    //最大敌人数量，目前敌人数量

    public static GameObject[] enemies;
    //敌人们
    public static bool[] alive;
    //第i个敌人是否活着

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
    //返回现有的存活机器人

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

    }//依次检查机器人
}
