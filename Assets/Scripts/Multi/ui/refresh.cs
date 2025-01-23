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
/// ˢ�°�ť�������UI�ϼ��س����������з���
/// 
/// ������Multi_Ui
/// </summary>

public class refresh : MonoBehaviour
{
    Transform roomsselect;
    //ʵ�ʵ�UI����

    public static string local_ip;
    //����WLAN����ipv4��ַ

    public static bool is_host;
    //�Ƿ���Ϊ�������˷���
    List<string> list;
    //���õľ�����������IP
    List<GameObject> rooms;
    //ʵ�����ķ���

    Dictionary<string, bool>is_host_map;
    //ĳip�Ƿ�Ϊ����

    public static Socket host_sk;
    //���ڷ�Ӧ�Ƿ�Ϊ����

    bool pinged;//�Ƿ�ping��
    bool queried;//�Ƿ�ѯ�����
    bool loaded;//�Ƿ������UI
    bool loading;//�Ƿ����ڼ��� 

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
        //���ؼ���_ˢ��

        Button back_mainpage = roomsselect.GetChild(2).GetChild(0).GetComponent<Button>();
        back_mainpage.onClick.AddListener(Quit);
        //���ؼ���_�˳�
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
            int port = 6060;//Ĭ��ȷ�������Ķ˿�
            IPAddress ipa = IPAddress.Parse(ip);
            IPEndPoint ipe = new IPEndPoint(ipa, port);
            host_sk.Bind(ipe);
            //��ʼ��socket

            host_sk.Listen(0);
            //��ʼ����

            host_sk.BeginAccept(new AsyncCallback(Recallaccept), host_sk);
            //�첽����
        }
    }
    //�������󣬷����Ƿ�������

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
            //����


            StartCoroutine(Load_text());

            StartCoroutine(Get_all_ip());

            StartCoroutine(Query_all_ip());

            StartCoroutine(Reload_ui());
        }
    }
    //ˢ�¾���������


    void Recallaccept(IAsyncResult iar)
    {
        Socket mainsocket = iar.AsyncState as Socket;
        //�ⲿ��ip�Ͷ˿ڵ���sockest
        Socket clientsocket = mainsocket.EndAccept(iar);
        //���ؼ�������˵�socket

        string host_res = is_host ? "1" : "0";
        clientsocket.Send(Encoding.UTF8.GetBytes(host_res));
        clientsocket.Close();
        //��Ӧ

        mainsocket.BeginAccept(Recallaccept, clientsocket);
        //��������
    }
    //6060�˿ڼ����ص�,�����Ƿ�Ϊ��������Ϣ


    string Get_Wlan_Ipv4()
    {
        NetworkInterface[] arr = NetworkInterface.GetAllNetworkInterfaces();
        //�����������ӿ�

        foreach (NetworkInterface i in arr)
        {
            if (i.OperationalStatus == OperationalStatus.Up && i.Name.Equals("WLAN"))
            {
                //���������������ΪWLAN
                foreach (UnicastIPAddressInformation add in i.GetIPProperties().UnicastAddresses)
                {
                    if (add.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        //�����ַЭ�����ipv4
                        return add.Address.ToString();
                    }
                }
            }
        }

        return null;
    }
    //��ñ�����������WLAN��ipv4��ַ

    void Set_room_pos(int i, GameObject rm, string ip)
    {
        float maxY = 1 - (i - 1) * 0.1f;
        float minY = maxY - 0.1f;
        float maxX = 1f;
        float minX = 0f;

        GameObject rc = GameObject.FindGameObjectWithTag("RoomContainer");
        rm.transform.SetParent(rc.transform);
        //���뵽

        RectTransform rt = rm.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(minX, minY);
        rt.anchorMax = new Vector2(maxX, maxY);
        rt.offsetMax = new Vector2(0f, 0f);
        rt.offsetMin = new Vector2(0f, 0f);
        rt.localScale = new Vector3(1f, 1f, 1f);

        TMP_Text tt = rt.transform.GetChild(0).GetComponent<TMP_Text>();
        tt.text = "IP:" + ip;
    }
    //���õ�i��room ê��λ����Ϣ

    IEnumerator Get_all_ip()
    {
        string myip = refresh.local_ip;
        string[] temp = myip.Split('.');
        string ip = temp[0] + "." + temp[1] + "." + temp[2] + ".";
        //��ǰ�����������

        
        for (int i = 1; i <= 255; i++)
        {
            TMP_Text lt = roomsselect.GetChild(6).GetComponent<TMP_Text>();
            lt.text = "�������ؾ�����(" + i.ToString() + "/255" + ")......";

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
    //��ȡ��������������ip��ѭ��ping��

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
                    int port = 6060;//Ĭ��ȷ�������Ķ˿�
                    IPAddress ipa = IPAddress.Parse(ip);
                    IPEndPoint ipe = new IPEndPoint(ipa, port);
                    //��ʼ��socket

                    try
                    {
                        sk.Connect(ipe);
                    }catch(SocketException)
                    {
                        sk.Close();
                        is_host_map.Add(list[i], false);
                        continue;
                    }
                    //�쳣����ֱ������������

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
    //��ѯ��������ip�����ǲ�������

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
    //����UI

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
    //���ء������С���ʾ����


    private void Quit()
    {
        Image ima = roomsselect.GetChild(7).GetComponent<Image>();
        ima.enabled = true;
        
        host_sk.Close();

        SceneManager.LoadScene("MainPage");
    }
    //�˳�

    public static void Close_all_socket()
    {
        host_sk.Close();
    }
    //�ر�socket
}
