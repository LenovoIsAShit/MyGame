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
    [HideInInspector]
    public Transform ui;
    //�˳�ȷ����

    public static quit_to_mainpage quitUI;
    //�˳�����

    void Awake()
    {
        ui=transform.GetChild(0);
        quitUI = GetComponent<quit_to_mainpage>();

        Button quit = ui.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        quit.onClick.AddListener(Quit);
        //ֱ�ӹ����˳��¼�

        Button cancel = ui.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        cancel.onClick.AddListener(Cancel);
        //ȡ��
    }
    void FixedUpdate()
    {

    }

    private void Quit()
    {
        Image ima = ui.GetChild(1).GetComponent<Image>();
        ima.enabled = true;

        SceneManager.LoadScene("MainPage");
    }

    public void Cancel()
    {
        ui.gameObject.SetActive(false);
    }

    public void Show()
    {
        ui.gameObject.SetActive(true);
    }
}
