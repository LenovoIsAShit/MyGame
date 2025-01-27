using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// �ͷ�����ҿ������
///     ����server�����ÿ֡��������Ϣ
/// 
/// ������Ԥ�Ƽ�clientcontroller����
/// </summary>

public class client_controller : MonoBehaviour
{
    public static List<GameObject> gamers;
    //���

    public static bool[] active;
    //����״̬
    public static string[] str;

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void Awake()
    {
        Init();
    }

    void Update()
    {
        Update_res();

        if (client.res[0] != 2)
        {
            Updata_gamers();
        }
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    void Update_res()
    {
        if (client.res[0] != 2)
        {
            string s = Encoding.UTF8.GetString(client.res);
            string[] temp = Encoding.UTF8.GetString(client.res).Split(">");
            
            int index = 0;
            if (client.res[0] != '<')
            {
                //���ݴ���
                index = 1;
            }
            str = temp[index].Split(",");

        }
    }
    //��ʼ��client.res

    void Updata_gamers()
    {
        for (int i = 0; i < str.Length-1; i++)
        {
            string now = str[i];
            if (now == (i.ToString() + "*=") || now == ("<" + i.ToString() + "*="))
            {
                gamers[i].SetActive(false);
                //δ����
                continue;
            }
            else
            {

                string[] actions = now.Split("*");

                gamers[i].SetActive(true);
                //�Ѽ���

                string[] pos = actions[1].Split("|");
                Vector3 npos = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
                gamers[i].transform.position = npos;
                //����

                string[] vec = actions[2].Split("|");
                Vector3 nvec = new Vector3(float.Parse(vec[0]), float.Parse(vec[1]), float.Parse(vec[2]));
                gamers[i].transform.rotation = Quaternion.LookRotation(nvec);
                //����

                string modl = actions[3];
                Change_model(gamers[i].transform.GetChild(0).gameObject, modl);
                //ģ��
            }
        }
    }
    //�������������Ϣ

    void Change_model(GameObject model, string act)
    {
        Animator ani = model.GetComponent<Animator>();
        bool pW = act[0] == '1' ? true : false;
        bool pA = act[1] == '1' ? true : false;
        bool pS = act[2] == '1' ? true : false;
        bool pD = act[3] == '1' ? true : false;
        bool space = act[4] == '1' ? true : false;
        bool pR = act[5] == '1' ? true : false;

        bool running = pW || pS || pA || pD;

        if (pR)
        {
            ani.SetBool("jump", false);
            ani.SetBool("run", false);
            ani.SetBool("dance", true);
        }
        else
        {
            ani.SetBool("dance", false);

            if (running)
            {
                ani.SetBool("run", true);

                if (space) ani.SetBool("rtj", true);
                else ani.SetBool("rtj", false);
            }
            else
            {
                ani.SetBool("run", false);

                if (space) ani.SetBool("jump", true);
                else ani.SetBool("jump", false);
            }
        }
    }
    //����ģ��

    void Init_gamers()
    {
        gamers = new List<GameObject>();
        for (int i = 0; i < client.max_player_num; i++)
        {
            if (i != client.index)
            {
                gamers.Add(Instantiate(Resources.Load<GameObject>("prefabs/Model/gameri")));
            }
            else
            {
                gamers.Add(GameObject.FindGameObjectWithTag("SinglePlayer"));
            }
            gamers[i].SetActive(false);
        }
    }
    //��ʼ�����

    void Init()
    {
        Init_gamers();

        active = new bool[client.max_player_num];
        for (int i = 0; i < client.max_player_num; i++) active[i] = false;

    }
}
