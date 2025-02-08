using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class win : MonoBehaviour
{
    [HideInInspector]
    public Transform ui;
    //退出确定框

    public static win quitUI;
    //退出单例

    void Awake()
    {
        ui = transform.GetChild(0);
        quitUI = GetComponent<win>();

        Button quit = ui.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        quit.onClick.AddListener(Quit);
        //直接挂载退出事件


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
