using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// 游戏主机-网络层脚本
///     接发帧信息
/// 
/// 
/// 挂载于预制件server对象
/// </summary>


public class server : MonoBehaviour
{
    public static int max_player_num;
    //socekt开启的最大数量（除了自己）

    public static int now_player_num;
    //目前玩家人数

    public static Socket[] sk_list;
    //服务端socket
    public static Socket room_ask;
    //房间询问，是否能加入

    public static bool[] active_list;
    //每个player是否激活
    public static byte[] data;
    //每帧发送给clietn的信息

    public static List<byte[]> res;
    //储存每个玩家（除了主机）在此帧的行为


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    void Awake()
    {
        Init();

        Open_Sockets();

        Open_roomnum_ask();

        refresh.is_host = true;
        //开启主机
    }

    void  LateUpdate()
    {
        Update_client_datas();
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    void Update_client_datas()
    {
        string str = "";

        str+=(0).ToString();
        str += "*";
        GameObject p1 = GameObject.FindGameObjectWithTag("SinglePlayer");
        string pos_p1 = "";
        Vector3 p_p1 = p1.transform.position;
        pos_p1+= (p_p1.x.ToString() + "|" + p_p1.y.ToString() + "|" + p_p1.z.ToString());
        str += pos_p1;
        str += "*";
        //主机位置

        string frd_p1 = "";
        Vector3 nfrd_p1 = p1.transform.forward;
        frd_p1 += (nfrd_p1.x.ToString() + "|" + nfrd_p1.y.ToString() + "|" + nfrd_p1.z.ToString());
        str += frd_p1;
        str += "*";
        //主机朝向

        if (Input.GetKey(KeyCode.W)) str += "1";
        else str += "0";
        if (Input.GetKey(KeyCode.A)) str += "1";
        else str += "0";
        if (Input.GetKey(KeyCode.S)) str += "1";
        else str += "0";
        if (Input.GetKey(KeyCode.D)) str += "1";
        else str += "0";
        if (Input.GetKey(KeyCode.Space)) str += "1";
        else str += "0";
        if (Input.GetKey(KeyCode.R)) str += "1";
        else str += "0";
        //主机按键

        str += ",";
        //主机默认第一个
        
        for(int i = 0; i < max_player_num; i++)
        {
            str += (i + 1).ToString();
            str += "*";
            //玩家号

            if (active_list[i] == true)
            {
                string pos = "";
                Vector3 npos = controller.gamers[i].transform.position;
                pos += (npos.x.ToString() + "|" + npos.y.ToString() + "|" + npos.z.ToString());
                str += pos;
                str += "*";
                //位置信息

                string frd = "";
                Vector3 nfrd = controller.gamers[i].transform.forward;
                frd += (nfrd.x.ToString() + "|" + nfrd.y.ToString() + "|" + nfrd.z.ToString());
                str += frd;
                str += "*";
                //朝向信息

                str += Get_model_state(i);
                //抽取按键状态

                str += "*"; 
            }
            else
            {
                str += "=";
                //表示此未激活
            }
            str += ",";
        }

        data = Encoding.UTF8.GetBytes(str);
    }
    //向每个client传送已经改变完的数据 


    void Open_Sockets()
    {
        for (int i = 0; i < max_player_num; i++)
        {
            sk_list[i] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string local_ip = refresh.local_ip;
            int port = 8080 + i;
            IPAddress ipa = IPAddress.Parse(local_ip);
            IPEndPoint ipe = new IPEndPoint(ipa, port);
            sk_list[i].Bind(ipe);
            //初始化

            sk_list[i].Listen(1);
            //开始监听

            sk_list[i].BeginAccept(new AsyncCallback(Accept_recall), sk_list[i]);
            //必须异步接受
        }
    }
    //打开需要数量的socekt，并开始异步监听
    void Accept_recall(IAsyncResult iar)
    {
        Socket main = iar.AsyncState as Socket;
        //主socket
        Socket cli =  main.EndAccept(iar);
        //客户端cli

        IPEndPoint ipe = (IPEndPoint)main.LocalEndPoint;
        IPEndPoint cli_ipe = (IPEndPoint)cli.RemoteEndPoint;
        int index = ipe.Port - 8080;

        cli.BeginReceive(res[index], 0, res[index].Length, SocketFlags.None, new AsyncCallback(Receive_recall), cli);
        //异步接受消息

        string temp = "<" + Encoding.UTF8.GetString(data) + ">";
        byte[] send_data = Encoding.UTF8.GetBytes(temp);
        //封装协议包

        cli.BeginSend(send_data, 0, send_data.Length, SocketFlags.None, new AsyncCallback(Send_recall), cli);
        //异步发送
    }
    void Receive_recall(IAsyncResult iar)
    {
        Socket main =iar.AsyncState as Socket;
        int count = main.EndReceive(iar);

        IPEndPoint ipe = (IPEndPoint)main.LocalEndPoint;
        int index = ipe.Port - 8080;

        main.BeginReceive(res[index], 0, res[index].Length, SocketFlags.None, new AsyncCallback(Receive_recall), main);
    }
    void Send_recall(IAsyncResult iar)
    {
        Socket cli = iar.AsyncState as Socket;
        int count = cli.EndSend(iar);

        Thread.Sleep(20);

        string temp = "<" + Encoding.UTF8.GetString(data) + ">";
        byte[] send_data = Encoding.UTF8.GetBytes(temp);
        //封装协议包

        cli.BeginSend(send_data, 0, send_data.Length, SocketFlags.None, new AsyncCallback(Send_recall), cli);
    }



    void Open_roomnum_ask()
    {
        room_ask = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        string local_ip = refresh.local_ip;
        int port = 6666;
        IPAddress ipa = IPAddress.Parse(local_ip);
        IPEndPoint ipe = new IPEndPoint(ipa, port);
        room_ask.Bind(ipe);

        room_ask.Listen(1);

        room_ask.BeginAccept(Room_ask, room_ask);
    }
    //此端口返回0或者端口号：
    //          前者表示人员已满，无法加入；
    //          后者表示可加入，和可以连接的端口号
    void Room_ask(IAsyncResult iar)
    {
        Socket main = iar.AsyncState as Socket;
        //主socket
        Socket client = main.EndAccept(iar);
        //服务端socket

        byte[] data;
        if (now_player_num < max_player_num)
        {
            int pos = Serch_respos();
            //从左到右找返回第一个空位
            active_list[pos] = true;
            //预留空位
            now_player_num++;
            //空位减少

            data =Encoding.UTF8.GetBytes((8080+pos).ToString());
            //有空位则返回空位
        }
        else
        {
            data = Encoding.UTF8.GetBytes((0).ToString());
            //没有空位则返回0 
        }
        client.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(Room_ask_recall), client);


        main.BeginAccept(Room_ask, main);
    }
    void Room_ask_recall(IAsyncResult iar)
    {
        Socket main = iar.AsyncState as Socket;
        Socket client = main.EndAccept(iar);

        client.Close();
    }



    int Serch_respos()
    {
        for(int i = 0; i < max_player_num; i++)
        {
            if (active_list[i] == false) return i;
        }
        return 0;
    }
    //返回第一个有空的位置  

    void Init()
    {
        now_player_num = 0;
        res = new List<byte[]>();
        sk_list = new Socket[max_player_num];
        active_list = new bool[max_player_num];

        string temp = "";
        for(int i=0; i<max_player_num; i++)temp += (i.ToString() + "*=,");
        data= Encoding.UTF8.GetBytes(temp);

        int sz = Encoding.UTF8.GetBytes(new string('2', 100)).Length;
        for (int i = 0; i < max_player_num; i++)
        {
            res.Add(new byte[sz]);
            res[i][0] = 2;//未赋值标志
            active_list[i] = false;
        }
    }

    string Get_model_state(int i)
    {
        if (res[i][0] == 2) return "000000";//未赋值标志
        else
        {
            string act = Encoding.UTF8.GetString(server.res[i]);
            string[] loc_str = act.Split(">");
            int index = 0;
            if (server.res[i][0] != '<') index = 1;
            string[] real_str = loc_str[index].Split("*");
            string ans = "";
            for (int s = 1; s <= 6; s++)
            {
                ans += real_str[0][s];
            }

            return ans;
        }
    }
    //抽取res[i]中的按键状态，即第i位玩家的按键信息

    public static void Close_all_socket()
    {
        for(int i=0;i< sk_list.Length; i++)
        {
            sk_list[i].Close();
        }
        room_ask.Close();
    }
    //关闭所有socekt
}
