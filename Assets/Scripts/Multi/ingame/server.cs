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
/// ��Ϸ����-�����ű�
///     �ӷ�֡��Ϣ
/// 
/// 
/// ������Ԥ�Ƽ�server����
/// </summary>


public class server : MonoBehaviour
{
    public static int max_player_num;
    //socekt��������������������Լ���

    public static int now_player_num;
    //Ŀǰ�������

    public static Socket[] sk_list;
    //�����socket
    public static Socket room_ask;
    //����ѯ�ʣ��Ƿ��ܼ���

    public static bool[] active_list;
    //ÿ��player�Ƿ񼤻�
    public static byte[] data;
    //ÿ֡���͸�clietn����Ϣ

    public static List<byte[]> res;
    //����ÿ����ң������������ڴ�֡����Ϊ


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    void Awake()
    {
        Init();

        Open_Sockets();

        Open_roomnum_ask();

        refresh.is_host = true;
        //��������
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
        //����λ��

        string frd_p1 = "";
        Vector3 nfrd_p1 = p1.transform.forward;
        frd_p1 += (nfrd_p1.x.ToString() + "|" + nfrd_p1.y.ToString() + "|" + nfrd_p1.z.ToString());
        str += frd_p1;
        str += "*";
        //��������

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
        //��������

        str += ",";
        //����Ĭ�ϵ�һ��
        
        for(int i = 0; i < max_player_num; i++)
        {
            str += (i + 1).ToString();
            str += "*";
            //��Һ�

            if (active_list[i] == true)
            {
                string pos = "";
                Vector3 npos = controller.gamers[i].transform.position;
                pos += (npos.x.ToString() + "|" + npos.y.ToString() + "|" + npos.z.ToString());
                str += pos;
                str += "*";
                //λ����Ϣ

                string frd = "";
                Vector3 nfrd = controller.gamers[i].transform.forward;
                frd += (nfrd.x.ToString() + "|" + nfrd.y.ToString() + "|" + nfrd.z.ToString());
                str += frd;
                str += "*";
                //������Ϣ

                str += Get_model_state(i);
                //��ȡ����״̬

                str += "*"; 
            }
            else
            {
                str += "=";
                //��ʾ��δ����
            }
            str += ",";
        }

        data = Encoding.UTF8.GetBytes(str);
    }
    //��ÿ��client�����Ѿ��ı�������� 


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
            //��ʼ��

            sk_list[i].Listen(1);
            //��ʼ����

            sk_list[i].BeginAccept(new AsyncCallback(Accept_recall), sk_list[i]);
            //�����첽����
        }
    }
    //����Ҫ������socekt������ʼ�첽����
    void Accept_recall(IAsyncResult iar)
    {
        Socket main = iar.AsyncState as Socket;
        //��socket
        Socket cli =  main.EndAccept(iar);
        //�ͻ���cli

        IPEndPoint ipe = (IPEndPoint)main.LocalEndPoint;
        IPEndPoint cli_ipe = (IPEndPoint)cli.RemoteEndPoint;
        int index = ipe.Port - 8080;

        cli.BeginReceive(res[index], 0, res[index].Length, SocketFlags.None, new AsyncCallback(Receive_recall), cli);
        //�첽������Ϣ

        string temp = "<" + Encoding.UTF8.GetString(data) + ">";
        byte[] send_data = Encoding.UTF8.GetBytes(temp);
        //��װЭ���

        cli.BeginSend(send_data, 0, send_data.Length, SocketFlags.None, new AsyncCallback(Send_recall), cli);
        //�첽����
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
        //��װЭ���

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
    //�˶˿ڷ���0���߶˿ںţ�
    //          ǰ�߱�ʾ��Ա�������޷����룻
    //          ���߱�ʾ�ɼ��룬�Ϳ������ӵĶ˿ں�
    void Room_ask(IAsyncResult iar)
    {
        Socket main = iar.AsyncState as Socket;
        //��socket
        Socket client = main.EndAccept(iar);
        //�����socket

        byte[] data;
        if (now_player_num < max_player_num)
        {
            int pos = Serch_respos();
            //�������ҷ��ص�һ����λ
            active_list[pos] = true;
            //Ԥ����λ
            now_player_num++;
            //��λ����

            data =Encoding.UTF8.GetBytes((8080+pos).ToString());
            //�п�λ�򷵻ؿ�λ
        }
        else
        {
            data = Encoding.UTF8.GetBytes((0).ToString());
            //û�п�λ�򷵻�0 
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
    //���ص�һ���пյ�λ��  

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
            res[i][0] = 2;//δ��ֵ��־
            active_list[i] = false;
        }
    }

    string Get_model_state(int i)
    {
        if (res[i][0] == 2) return "000000";//δ��ֵ��־
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
    //��ȡres[i]�еİ���״̬������iλ��ҵİ�����Ϣ

    public static void Close_all_socket()
    {
        for(int i=0;i< sk_list.Length; i++)
        {
            sk_list[i].Close();
        }
        room_ask.Close();
    }
    //�ر�����socekt
}
