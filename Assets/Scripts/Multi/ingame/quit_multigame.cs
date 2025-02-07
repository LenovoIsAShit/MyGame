using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 退出多人游戏，并释放socket资源，切换场景
/// 
/// 挂载于Multi_Ui
/// </summary>


public class quit_multigame : MonoBehaviour
{
    GameObject qtm;
    //退出框
    void Awake()
    {
        qtm = transform.GetChild(1).gameObject;

        Button qt = qtm.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        qt.onClick.AddListener(Quit);

        Button cl = qtm.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        cl.onClick.AddListener(Close_query_frame);
    }

    void FixedUpdate()
    {
        Key_Check();
    }

    void Quit()
    {
        Show_loading_image();

        Close_sockets();

        Change_scene();
    }
    //退出多人模式

    void Close_sockets()
    {
        if (refresh.is_host == false)
        {
            //玩家端
            client.Close_all_socket();
        }
        else
        {
            //主机端
            server.Close_all_socket();
        }
        refresh.Close_all_socket();
    }
    //关闭套接字

    void Show_query_frame()
    {
        qtm.SetActive(true);
    }
    //显示退出询问框
    void Close_query_frame()
    {
        qtm.SetActive(false);
    }
    //显示退出询问框
    void Show_loading_image()
    {
        GameObject loading = transform.GetChild(2).gameObject;
        loading.SetActive(true);

    }
    //显示加载页面
    void Change_scene()
    {
        SceneManager.LoadScene("MainPage");
    }
    //切换场景

    void Key_Check()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Show_query_frame();
        }
    }
    //按键检测
}

