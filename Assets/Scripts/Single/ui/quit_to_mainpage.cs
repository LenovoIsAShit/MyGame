using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �ӵ���ģʽ�˳�����ҳ��
///     ���ص���canvas��
/// </summary>

public class quit_to_mainpage : MonoBehaviour
{
    Transform ui;
    //�˳�ȷ����
    void Awake()
    {
        ui=transform.GetChild(0);

        Button quit = ui.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        quit.onClick.AddListener(Quit);
        //ֱ�ӹ����˳��¼�

        Button cancel = ui.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        cancel.onClick.AddListener(Cancel);
        //ȡ��
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ui.gameObject.SetActive(true);
            //��ʾ�˳�ȷ�ϴ��� 
        }
    }

    private void Quit()
    {
        Image ima = ui.GetChild(1).GetComponent<Image>();
        ima.enabled = true;

        SceneManager.LoadScene("MainPage");
    }

    private void Cancel()
    {
        ui.gameObject.SetActive(false);
    }
}
