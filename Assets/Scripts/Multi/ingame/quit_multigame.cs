using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �˳�������Ϸ�����ͷ�socket��Դ���л�����
/// 
/// ������Multi_Ui
/// </summary>


public class quit_multigame : MonoBehaviour
{
    GameObject qtm;
    //�˳���
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
    //�˳�����ģʽ

    void Close_sockets()
    {
        if (refresh.is_host == false)
        {
            //��Ҷ�
            client.Close_all_socket();
        }
        else
        {
            //������
            server.Close_all_socket();
        }
        refresh.Close_all_socket();
    }
    //�ر��׽���

    void Show_query_frame()
    {
        qtm.SetActive(true);
    }
    //��ʾ�˳�ѯ�ʿ�
    void Close_query_frame()
    {
        qtm.SetActive(false);
    }
    //��ʾ�˳�ѯ�ʿ�
    void Show_loading_image()
    {
        GameObject loading = transform.GetChild(2).gameObject;
        loading.SetActive(true);

    }
    //��ʾ����ҳ��
    void Change_scene()
    {
        SceneManager.LoadScene("MainPage");
    }
    //�л�����

    void Key_Check()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Show_query_frame();
        }
    }
    //�������
}

