using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������䰴ť��
///     �ı�UI������server�ű���������������
///     
/// ���ص�room_select��
/// </summary>


public class create_room : MonoBehaviour
{
    Transform roomsselect;
    //ʵ�ʵ�UI����

    public static GameObject Server;
    public static GameObject contro;

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void Awake()
    {
        roomsselect = transform.GetChild(0);
            
        Button crt = roomsselect.GetChild(4).GetChild(0).GetComponent<Button>();
        crt.onClick.AddListener(Show_roomnum_query);
        //��canvas�ķ��䴴����ť

        Button irn = roomsselect.GetChild(8).GetChild(3).GetComponent<Button>();
        irn.onClick.AddListener(Increase_room_num);
        //���ӷ�������

        Button drn = roomsselect.GetChild(8).GetChild(4).GetComponent<Button>();
        drn.onClick.AddListener(Decrease_room_num);
        //���ٷ�������

        Button ok = roomsselect.GetChild(8).GetChild(5).GetComponent<Button>();
        ok.onClick.AddListener(Room_query_Ok);
        //����ȷ�ϰ�ť 

        Button cancel = roomsselect.GetChild(8).GetChild(6).GetComponent<Button>();
        cancel.onClick.AddListener(Room_query_Cancel);
        //����ȡ��ȷ��
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void Show_roomnum_query()
    {
        GameObject rq = roomsselect.GetChild(8).gameObject;
        rq.SetActive(true);
    }
    //��ʾ��������ѯ�ʿ�

    void Start_Server()
    {
        TMP_Text tt = roomsselect.GetChild(8).GetChild(2).GetComponent<TMP_Text>();
        server.max_player_num = int.Parse(tt.text);
        //���ݷ�������

        Server = Instantiate(Resources.Load<GameObject>("prefabs/Multi/ingame/Server"));
        contro = Instantiate(Resources.Load<GameObject>("prefabs/Multi/ingame/Gamecontroller"));


    }
    //��Show_roomnum_query�еķ����������ݸ�server�ű��ľ�̬����
    //��ʵ����server

    void Increase_room_num()
    {
        TMP_Text tt = roomsselect.GetChild(8).GetChild(2).GetComponent<TMP_Text>();
        if (int.Parse(tt.text) < 4)
        {
            tt.text = (int.Parse(tt.text) + 1).ToString();
        }
    }
    void Decrease_room_num()
    {
        TMP_Text tt = roomsselect.GetChild(8).GetChild(2).GetComponent<TMP_Text>();
        if (int.Parse(tt.text) > 1)
        {
            tt.text = (int.Parse(tt.text) - 1).ToString();
        }
    }
    //������������
    void Room_query_Ok()
    {
        Image loading = roomsselect.GetChild(7).GetComponent<Image>();
        loading.enabled = true;
        //��ʾ������ͼƬ

        Start_Server();

        roomsselect.gameObject.SetActive(false);
        //�ر�����UI
    }
    //ȷ��������������Start_Server()

    void Room_query_Cancel()
    {
        TMP_Text tt = roomsselect.GetChild(8).GetChild(2).GetComponent<TMP_Text>();
        tt.text = "1";
        GameObject rq = roomsselect.GetChild(8).gameObject;
        rq.SetActive(false);
    }
    //ȡ������
}
