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
    public static EnemyController enemy_Controller;


    static int max_num , now_num;
    //������������Ŀǰ��������
    public static List<GameObject> enemies;
    //������

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
    //�������еĴ�������



    IEnumerator Make_enemy()
    {
        for (; now_num < max_num; now_num++)
        {
            yield return new WaitForSeconds(0.5f);;

            GameObject obj = Init_Monster(OP.instance.Get("Monster"));

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

}
