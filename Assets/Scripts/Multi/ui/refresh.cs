using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 刷新按钮，点击在UI上加载出局域网所有房间
/// 
/// 挂载于Multi_Ui
/// </summary>

public class refresh : MonoBehaviour
{
    Transform roomsselect;
    //实际的UI对象

    public static string local_ip;
    //本机WLAN网卡ipv4地址

    public static bool is_host;
    //是否作为主机开了房间
    List<string> list;
    //存获得的局域网内在线IP
    List<GameObject> rooms;
    //实例化的房间

    Dictionary<string, bool>is_host_map;
    //某ip是否为主机

    public static Socket host_sk;
    //用于反应是否为主机

    bool pinged;//是否ping完
    bool queried;//是否询问完成
    bool loaded;//是否加载完UI
    bool loading;//是否正在加载 

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    private void Awake()
    {
        roomsselect = transform.GetChild(0);

        list = new List<string>();
        rooms= new List<GameObject>();
        is_host_map = new Dictionary<string, bool>();
        pinged =false;
        queried = false;
        is_host = false;
        loading = false;

        local_ip = Get_Wlan_Ipv4();

        Button rf_refresh = roomsselect.GetChild(3).GetChild(0).GetComponent<Button>();
        rf_refresh.onClick.AddListener(Refresh);
        //挂载监听_刷新

        Button back_mainpage = roomsselect.GetChild(2).GetChild(0).GetComponent<Button>();
        back_mainpage.onClick.AddListener(Quit);
        //挂载监听_退出
    }

    void Start()
    {
        Respond_ishost();
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    void Respond_ishost()
    {
        host_sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        if (local_ip != null)
        {
            string ip = refresh.local_ip;
            int port = 6060;//默认确认主机的端口
            IPAddress ipa = IPAddress.Parse(ip);
            IPEndPoint ipe = new IPEndPoint(ipa, port);
            host_sk.Bind(ipe);
            //初始化socket

            host_sk.Listen(0);
            //开始监听

            host_sk.BeginAccept(new AsyncCallback(Recallaccept), host_sk);
            //异步接受
        }
    }
    //监听请求，返回是否是主机

    void Refresh()
    {
        if (loading == false && local_ip != null)
        {
            loading = true;
            is_host_map.Clear();
            pinged = false;
            loaded = false;
            queried = false;
            list.Clear();
            for (int i = 0; i < rooms.Count; i++)
            {
                Destroy(rooms[i]);
            }
            rooms.Clear();
            //重置


            StartCoroutine(Load_text());

            StartCoroutine(Get_all_ip());

            StartCoroutine(Query_all_ip());

            StartCoroutine(Reload_ui());
        }
    }
    //刷新局域网房间


    void Recallaccept(IAsyncResult iar)
    {
        Socket mainsocket = iar.AsyncState as Socket;
        //外部绑定ip和端口的主sockest
        Socket clientsocket = mainsocket.EndAccept(iar);
        //返回监听服务端的socket

        string host_res = is_host ? "1" : "0";
        clientsocket.Send(Encoding.UTF8.GetBytes(host_res));
        clientsocket.Close();
        //回应

        mainsocket.BeginAccept(Recallaccept, clientsocket);
        //继续接受
    }
    //6060端口监听回调,发送是否为主机的消息


    string Get_Wlan_Ipv4()
    {
        NetworkInterface[] arr = NetworkInterface.GetAllNetworkInterfaces();
        //获得所有网络接口

        foreach (NetworkInterface i in arr)
        {
            if (i.OperationalStatus == OperationalStatus.Up && i.Name.Equals("WLAN"))
            {
                //如果在线且网卡名为WLAN
                foreach (UnicastIPAddressInformation add in i.GetIPProperties().UnicastAddresses)
                {
                    if (add.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        //如果地址协议簇是ipv4
                        return add.Address.ToString();
                    }
                }
            }
        }

        return null;
    }
    //获得本机虚拟网卡WLAN的ipv4地址

    void Set_room_pos(int i, GameObject rm, string ip)
    {
        float maxY = 1 - (i - 1) * 0.1f;
        float minY = maxY - 0.1f;
        float maxX = 1f;
        float minX = 0f;

        GameObject rc = GameObject.FindGameObjectWithTag("RoomContainer");
        rm.transform.SetParent(rc.transform);
        //加入到

        RectTransform rt = rm.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(minX, minY);
        rt.anchorMax = new Vector2(maxX, maxY);
        rt.offsetMax = new Vector2(0f, 0f);
        rt.offsetMin = new Vector2(0f, 0f);
        rt.localScale = new Vector3(1f, 1f, 1f);

        TMP_Text tt = rt.transform.GetChild(0).GetComponent<TMP_Text>();
        tt.text = "IP:" + ip;
    }
    //设置第i个room 锚点位置信息

    IEnumerator Get_all_ip()
    {
        string myip = refresh.local_ip;
        string[] temp = myip.Split('.');
        string ip = temp[0] + "." + temp[1] + "." + temp[2] + ".";
        //当前局域网网络号

        
        for (int i = 1; i <= 255; i++)
        {
            TMP_Text lt = roomsselect.GetChild(6).GetComponent<TMP_Text>();
            lt.text = "搜索本地局域网(" + i.ToString() + "/255" + ")......";

            string pingip = ip + i.ToString();
            UnityEngine.Ping p = new UnityEngine.Ping(pingip);

            yield return new WaitForSeconds(0.08f);

            if (p.isDone)
            {
                list.Add(pingip);
            }
        }


        pinged = true;
    }
    //获取局域网所有在线ip（循环ping）

    IEnumerator Query_all_ip()
    {
        while (true)
        {
            yield return 0;
            if (pinged)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    yield return 0;

                    Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    string ip = list[i];
                    int port = 6060;//默认确认主机的端口
                    IPAddress ipa = IPAddress.Parse(ip);
                    IPEndPoint ipe = new IPEndPoint(ipa, port);
                    //初始化socket

                    try
                    {
                        sk.Connect(ipe);
                    }catch(SocketException)
                    {
                        sk.Close();
                        is_host_map.Add(list[i], false);
                        continue;
                    }
                    //异常捕获，直接跳过不处理

                    byte[] res = new byte[10];
                    int len = sk.Receive(res);
                    string host= Encoding.UTF8.GetString(res, 0, len);
                    if (host[0] == '1')
                    {
                        is_host_map.Add(list[i], true);
                    }
                    else
                    {
                        is_host_map.Add(list[i], false);
                    }
                    sk.Close();
                }

                queried = true;
                yield break;
            }
        }
    }
    //查询所有在线ip，你是不是主机

    IEnumerator Reload_ui()
    {

        while (true)
        {
            yield return 0;

            if (queried)
            {
                for(int i=0;i<list.Count;i++)
                {
                    yield return 0;

                    if (is_host_map[list[i]])
                    {
                        GameObject rm = Instantiate(Resources.Load<GameObject>("prefabs/Multi/ui/room"));
                        Set_room_pos(rooms.Count+1, rm, list[i]);
                        rooms.Add(rm);
                    }
                }
                loaded = true;
                yield break;
            }
        }
    }
    //加载UI

    IEnumerator Load_text()
    {
        TMP_Text lt = roomsselect.GetChild(6).GetComponent<TMP_Text>();
        lt.enabled = true;

        while (true)
        {
            yield return 0;
            if (loaded)
            {
                lt.enabled = false;
                loading = false;
                yield break;
            }
        }
    }
    //加载《加载中》提示文字


    private void Quit()
    {
        Image ima = roomsselect.GetChild(7).GetComponent<Image>();
        ima.enabled = true;
        
        host_sk.Close();

        SceneManager.LoadScene("MainPage");
    }
    //退出

    public static void Close_all_socket()
    {
        host_sk.Close();
    }
    //关闭socket
}
