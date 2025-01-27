using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 游戏控制层：
///     server根据每帧client发过来的信息，来更新对应玩家状态
///     
/// 挂载于预制件controller对象
/// </summary>


public class controller : MonoBehaviour
{
    public static List<GameObject> gamers;
    //其他玩家的trans组件

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    void Awake()
    {
        Init();

    }


    void LateUpdate()
    {
        for(int i = 0; i < server.max_player_num; i++)
        {
            if (server.active_list[i])
            {
                gamers[i].SetActive(true);
                Update_gamer(i);
            }
            else
            {
                gamers[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void Update_gamer(int i)
    {
        string act = Encoding.UTF8.GetString(server.res[i]);
        //获得此帧数据 
        
        if (server.res[i][0] !=2)
        {

            string[] str= act.Split(">");
            int index = 0;
            if (server.res[i][0] != '<')index = 1;

            string[] real_str = str[index].Split("*");
            Change_model(gamers[i].transform.GetChild(0).gameObject, real_str[0]);

            Change_pos(gamers[i], real_str[1], real_str[2]);
        }

    }
    //改变其他player位置信息

    void Change_model(GameObject model,string act)
    {
        Animator ani = model.GetComponent<Animator>();
        bool pW = act[1] == '1' ? true : false;
        bool pA = act[2] == '1' ? true : false;
        bool pS = act[3] == '1' ? true : false;
        bool pD = act[4] == '1' ? true : false;
        bool space = act[5] == '1' ? true : false;
        bool pR = act[6] == '1' ? true : false;

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
    //改变模型

    void Change_pos(GameObject trs,string vec,string dis)
    {
        string[] xyz = vec.Split("|");
        Vector3 nvec = new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));

        float ndis = float.Parse(dis);

        if(nvec.x!=0||nvec.y!=0||nvec.z!=0)trs.transform.rotation = Quaternion.LookRotation(nvec);
        //朝向
        trs.transform.position += (nvec.normalized * ndis * Time.deltaTime);
        //位置
    }
    //改变位置

    void Init()
    {
        gamers = new List<GameObject>();
        for (int i = 0; i < server.max_player_num; i++)
        {
            gamers.Add(Instantiate(Resources.Load<GameObject>("prefabs/Model/gameri")));
            gamers[i].SetActive(false);
        }
        //初始化gamers
    }
}
