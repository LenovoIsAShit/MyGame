using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 创建房间按钮：
///     改变UI，启动server脚本，开启主机功能
///     
/// 挂载到room_select框
/// </summary>


public class create_room : MonoBehaviour
{
    Transform roomsselect;
    //实际的UI对象

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
        //主canvas的房间创建按钮

        Button irn = roomsselect.GetChild(8).GetChild(3).GetComponent<Button>();
        irn.onClick.AddListener(Increase_room_num);
        //增加房间数量

        Button drn = roomsselect.GetChild(8).GetChild(4).GetComponent<Button>();
        drn.onClick.AddListener(Decrease_room_num);
        //减少房间数量

        Button ok = roomsselect.GetChild(8).GetChild(5).GetComponent<Button>();
        ok.onClick.AddListener(Room_query_Ok);
        //房间确认按钮 

        Button cancel = roomsselect.GetChild(8).GetChild(6).GetComponent<Button>();
        cancel.onClick.AddListener(Room_query_Cancel);
        //房间取消确认
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void Show_roomnum_query()
    {
        GameObject rq = roomsselect.GetChild(8).gameObject;
        rq.SetActive(true);
    }
    //显示房间数量询问框

    void Start_Server()
    {
        TMP_Text tt = roomsselect.GetChild(8).GetChild(2).GetComponent<TMP_Text>();
        server.max_player_num = int.Parse(tt.text);
        //传递房间人数

        Server = Instantiate(Resources.Load<GameObject>("prefabs/Multi/ingame/Server"));
        contro = Instantiate(Resources.Load<GameObject>("prefabs/Multi/ingame/Gamecontroller"));


    }
    //将Show_roomnum_query中的房间数量传递给server脚本的静态变量
    //并实例化server

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
    //增减房间数量
    void Room_query_Ok()
    {
        Image loading = roomsselect.GetChild(7).GetComponent<Image>();
        loading.enabled = true;
        //显示加载中图片

        Start_Server();

        roomsselect.gameObject.SetActive(false);
        //关闭所有UI
    }
    //确定创建，并调用Start_Server()

    void Room_query_Cancel()
    {
        TMP_Text tt = roomsselect.GetChild(8).GetChild(2).GetComponent<TMP_Text>();
        tt.text = "1";
        GameObject rq = roomsselect.GetChild(8).gameObject;
        rq.SetActive(false);
    }
    //取消创建
}
