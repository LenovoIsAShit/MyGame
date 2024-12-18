using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// 客户端脚本
///     每隔固定时间给server服务端发送帧信息
///     
/// 挂载于预制件cient游戏对象
/// 
/// </summary>
public class client : MonoBehaviour
{
    public static string host_ip;
    //主机ip

    public static int max_player_num;
    //与服务端约定的socekt开启的最大数量（除了自己）

    public static int index;
    //第几号玩家
    //在客服端发来的帧数据中，玩家号就是玩家数据索引号

    public static int port;
    //服务端预留端口

    public static Socket online;
    //连接


    public static byte[] res;
    //服务端发来的帧数据
    public static byte[] data;
    //发送给服务端的帧数据

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    void Awake()
    {
        Init();

        Connect_to_server();

    }

    void FixedUpdate()
    {
        Get_data();
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    void Connect_to_server()
    {
        online = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipa = IPAddress.Parse(host_ip);
        IPEndPoint ipe = new IPEndPoint(ipa, port);

        online.BeginConnect(ipe, new AsyncCallback(Connect_recall), online);
    }
    //连接server


    //////////////// 每帧发送给server本地状态数据 //////////////////
    void Get_data()
    {
        string datas = "";
        if (Input.GetKey(KeyCode.W)) datas += "1";
        else datas += "0";

        if (Input.GetKey(KeyCode.S)) datas += "1";
        else datas += "0";

        if (Input.GetKey(KeyCode.A)) datas += "1";
        else datas += "0";

        if (Input.GetKey(KeyCode.D)) datas += "1";
        else datas += "0";

        if (Input.GetKey(KeyCode.Space)) datas += "1";
        else datas += "0";

        if (Input.GetKey(KeyCode.R)) datas += "1";
        else datas += "0";

        datas += "*";
        //按键信息

        Local_camera lc = GameObject.FindGameObjectWithTag("SingleCamera").GetComponent<Local_camera>();
        Vector3 move_vec = new Vector3(0f, 0f, 0f);
        if (Input.GetKey(KeyCode.W)) move_vec += lc.Get_Forward();
        if (Input.GetKey(KeyCode.S)) move_vec -= lc.Get_Forward();
        if (Input.GetKey(KeyCode.D)) move_vec += lc.Get_Right();
        if (Input.GetKey(KeyCode.A)) move_vec -= lc.Get_Right();
        move_vec = move_vec.normalized;
        string m = move_vec.x.ToString() + "|" + move_vec.y.ToString() + "|" + move_vec.z.ToString();
        datas += m;

        datas += "*";
        //移动方向

        float dis = move.speed;
        datas+=dis.ToString();
        //移动距离

        datas += "*";

        data = Encoding.UTF8.GetBytes(datas);
    }

    void Connect_recall(IAsyncResult iar)
    {
        Socket online = iar.AsyncState as Socket;
        
        online.EndConnect(iar);

        online.BeginReceive(res, 0, res.Length, SocketFlags.None, new AsyncCallback(Receive_recall), online);

        string temp = "<" + Encoding.UTF8.GetString(data) + ">";
        byte[] send_data = Encoding.UTF8.GetBytes(temp);
        //封装协议包
        online.BeginSend(send_data, 0, send_data.Length, SocketFlags.None, new AsyncCallback(Send_recall), online);
    }

    void Send_recall(IAsyncResult iar)
    {
        Socket online = iar.AsyncState as Socket;
        int countr=online.EndSend(iar);

        Thread.Sleep(20);

        string temp = "<" + Encoding.UTF8.GetString(data) + ">";
        byte[] send_data = Encoding.UTF8.GetBytes(temp);
        //封装协议包

        online.BeginSend(send_data, 0, send_data.Length, SocketFlags.None, new AsyncCallback(Send_recall), online);
    }

    void Receive_recall(IAsyncResult iar)
    {
        Socket online = iar.AsyncState as Socket;
        int count = online.EndReceive(iar);
        online.BeginReceive(res, 0, res.Length, SocketFlags.None, new AsyncCallback(Receive_recall), online);
    }


    void Init()
    {
        max_player_num = 4;
        res = new byte[1024];
        res[0] = 2;//未接受数据标志
        data = Encoding.UTF8.GetBytes("000000*0|0|0*0*");
        //,表示包头.表示包尾

    }
    //初始化

    public static void Close_all_socket()
    {
        online.Close();
    }
}
