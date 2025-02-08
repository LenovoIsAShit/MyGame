using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class win : MonoBehaviour
{
    [HideInInspector]
    public Transform ui;
    //�˳�ȷ����

    public static win quitUI;
    //�˳�����

    void Awake()
    {
        ui = transform.GetChild(0);
        quitUI = GetComponent<win>();

        Button quit = ui.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        quit.onClick.AddListener(Quit);
        //ֱ�ӹ����˳��¼�


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

    public void Show()
    {
        ui.gameObject.SetActive(true);
    }
}
