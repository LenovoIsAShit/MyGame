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
    Transform ui;
    //退出确定框
    void Awake()
    {
        ui=transform.GetChild(0);

        Button quit = ui.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        quit.onClick.AddListener(Quit);
        //直接挂载退出事件

        Button cancel = ui.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        cancel.onClick.AddListener(Cancel);
        //取消
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ui.gameObject.SetActive(true);
            //显示退出确认窗口 
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
