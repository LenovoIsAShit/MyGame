using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 进入房间：
///     将ui上选择的房间ip传给client类
///     然后加载client和clientcontroller
///     
/// 此脚本挂载于Multi_Ui
/// </summary>

public class enter_room : MonoBehaviour
{
    public static GameObject cli;

    public static GameObject cli_controller;

    public static string room_ip;

    Transform roomsselect;
    //实际的UI对象

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    void Awake()
    {
        room_ip = "null";

        roomsselect = transform.GetChild(0);

        Button btn = roomsselect.GetChild(5).GetChild(0).GetComponent<Button>();
        btn.onClick.AddListener(Attempt);
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void Attempt()
    {
        if (enter_room.room_ip.Equals("null"))
        {
            Display_cancel_frame();
        }
        else
        {
            if (Check())
            {
                Enter();
            }
        }
    }
    //尝试
    
    bool Check()
    {
        Socket qs = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        string ip = enter_room.room_ip;
        int port = 6666;
        IPAddress ipa = IPAddress.Parse(ip);
        IPEndPoint ipe = new IPEndPoint(ipa, port);

        qs.Connect(ipe);
        //阻断式

        byte[] data = new byte[1024];
        int len = qs.Receive(data);
        string res= Encoding.UTF8.GetString(data, 0, len);

        qs.Close();

        if (res[0] == '0') return false;
        else
        {
            client.host_ip = enter_room.room_ip;
            client.port = int.Parse(res);
            client.index = client.port - 8080+1;
            return true;
        }
    }
    //查看是否可以连接
    void Enter()
    {
        Image loading = roomsselect.GetChild(7).GetComponent<Image>();
        loading.enabled = true;

        cli = Instantiate(Resources.Load<GameObject>("prefabs/Multi/ingame/Client"));
        cli_controller = Instantiate(Resources.Load<GameObject>("prefabs/Multi/ingame/ClientController"));

        roomsselect.gameObject.SetActive(false);
    }
    //进入房间
    void Display_cancel_frame()
    {
        roomsselect.GetChild(9).gameObject.SetActive(true);
    }
    //如果未选择房间，显示取消提示框


}
