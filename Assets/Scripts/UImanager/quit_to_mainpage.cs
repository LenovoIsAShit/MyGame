using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 从单人模式退出到主页面
///     挂载到主canvas上
/// </summary>

public class quit_to_mainpage : MonoBehaviour
{
    [HideInInspector]
    public Transform ui;
    //退出确定框

    public static quit_to_mainpage quitUI;
    //退出单例

    void Awake()
    {
        ui=transform.GetChild(0);
        quitUI = GetComponent<quit_to_mainpage>();

        Button quit = ui.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        quit.onClick.AddListener(Quit);
        //直接挂载退出事件

        Button cancel = ui.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        cancel.onClick.AddListener(Cancel);
        //取消
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
