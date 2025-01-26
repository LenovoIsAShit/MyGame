using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �����
/// 
///         �涨ʵ��������Ϊ��
///                 Ԥ�����+"="+������
/// 
/// </summary>
public class OP : MonoBehaviour
{
    public static OP Instance {get {return instance; } }
    public static OP instance;
    //����

    Dictionary<string, List<GameObject>> pool;
    Dictionary<string, GameObject> prefab;
    public Dictionary<string, List<GameObject>> Pool
    {
        get
        {
            if (pool == null)pool= new Dictionary<string, List<GameObject>>();
            return pool;
        }
    }
    public Dictionary<string, GameObject> Prefab
    {
        get
        {
            if (prefab == null) prefab = new Dictionary<string, GameObject>();
            return prefab;
        }
    }
    //����غ�Ԥ���


    /// <summary>
    /// ����������
    /// </summary>
    public GameObject Get(string objname)
    {
        if (Pool.ContainsKey(objname) && Pool[objname].Count > 0)
        {
            GameObject obj=null;
            obj = Pool[objname][0];
            obj.SetActive(true);
            obj.name = objname + "=" + Pool[objname].Count.ToString();
            Pool[objname].Remove(obj);
            //������
            switch (objname)
            {
                case "fire_bomb":
                    obj.GetComponent<bomb_test>().Copy(obj.GetComponent<bomb_test>());
                    return obj;
                default:
                    return obj;
            }//���⴦��

        }//������ҳ������������0

        string path = "prefabs/All/" + objname;

        if (!Prefab.ContainsKey(objname))
        {
            switch (objname)
            {
                case "fire_bomb":
                    GameObject obj = bomb_testFactory.bomb_fac.Create();
                    obj.GetComponent<bomb_test>().Set_check_state(false);
                    obj.GetComponent<bomb_test>().stop = true;
                    Prefab.Add(objname, obj);
                    break;

                default:
                    Prefab.Add(objname, Instantiate(Prefab[path]));
                    break;
            }
        }//���û��Ԥ���

        switch (objname)
        {
            case "fire_bomb":
                GameObject t = Instantiate(Prefab[objname]);
                t.name = objname + "=" + "0";
                t.GetComponent<bomb_test>().Copy(Prefab[objname].GetComponent<bomb_test>());
                return t;

            default:
                GameObject obj2 = Instantiate(Prefab[objname]);
                obj2.SetActive(true);
                obj2.name = objname + "=" + "0";
                return obj2;
        }



    }
    //��ȡ��ͨ����
    public void Del(GameObject obj)
    {
        string[] strs = obj.name.Split("=");
        string prefab_name = strs[0];

        if (Pool.ContainsKey(prefab_name))
        {
            Pool[prefab_name].Add(obj);
            obj.SetActive(false);
        }
        else
        {
            Pool.Add(prefab_name, new List<GameObject>() { obj});
        }
    }
    //ж�ض���





    /// <summary>
    /// �������ں���
    /// </summary>
    void Cleanup()
    {
        foreach(var p in pool)
        {
            foreach(var p_item in p.Value)
            {
                Destroy(p_item);
            }
            p.Value.Clear();
        }
        Pool.Clear();

        foreach (var p in Prefab)
        {
            Destroy(p.Value);
        }
        Prefab.Clear();

    }
    void OnDisable()
    {
        //Cleanup();
    }
    void OnApplicationQuit()
    {
        Cleanup();
    }
    private void Awake()
    {
        instance = gameObject.GetComponent<OP>();
    }
}