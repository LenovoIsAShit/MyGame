using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BehaviorDesigner.Runtime;

/// <summary>
/// 怪物控制脚本
///     控制怪物的生成和行为树挂载
///     
///     
///     挂载于Enemy_Controller
/// </summary>
public class EnemyController : MonoBehaviour
{
    public static EnemyController enemy_Controller;


    static int max_num , now_num;
    //最大敌人数量，目前敌人数量
    public static List<GameObject> enemies;
    //敌人们

    void Awake()
    {
        enemies = new List<GameObject>();

        enemy_Controller=GetComponent<EnemyController>();
    }

    private void Start()
    {

    }

    void LateUpdate()
    {

    }

    public void Start_make_enemy()
    {

        StartCoroutine(Make_enemy());
    }

    public static void Set_max_num(int num)
    {
        now_num = 0;
        //max_num = num;
        max_num = num;
    }

    public static List<GameObject> Get_enemies()
    {
        List<GameObject> res=new List<GameObject>();
        for(int i = 0; i < enemies.Count; i++)
        {
            res.Add(enemies[i]);
        }
        return res;
    }
    //返回现有的存活机器人



    IEnumerator Make_enemy()
    {
        float time = 0.5f;
        Display_pos.Set_time(time);

        int t = now_num;
        for (; now_num < max_num; now_num++)
        {
            GameObject obj = Init_Monster(OP.instance.Get("Monster"));
            obj.SetActive(false);
            obj.GetComponent<detect_system>().Reset_detected();

            if (!Check_pos(obj))
            {
                continue;
            }

            GameObject obj2 = OP.instance.Get("Display_pos");
            obj2.transform.position = new Vector3(obj.transform.position.x, 208.2f, obj.transform.position.z);
            obj2.GetComponent<Display_pos>().play();

            yield return new WaitForSeconds(time);
            
            obj.SetActive(true);

            obj.transform
                .DOMoveY(50, 0.3f)
                .From(true)
                .SetAutoKill(true);

            enemies.Add(obj);
            
        }
        yield break;
    }

    public void Reset_nownum(int n)
    {
        now_num = n;
    }

    public int Get_nownum()
    {
        return now_num;
    }

    public void Decrease_nownum()
    {
        now_num--;
    }

    GameObject Init_Monster(GameObject obj)
    {
        Vector3 pos = enemy_pos_get.Get_random_pos(now_num + 1111);
        pos.Set(pos.x, 209.0265f, pos.z);
        obj.transform.position = pos;

        obj.GetComponent<Blood_system>().Reset_hp();

        

        return obj;
    }

    bool Check_pos(GameObject obj)
    {
        if (!grid.Edge_check(obj.transform.position))
        {
            OP.instance.Del(obj);
            now_num--;
            return false;
        }
        return true;
    }
}
