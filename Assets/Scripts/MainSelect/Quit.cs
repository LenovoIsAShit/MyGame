using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 退出游戏按钮
/// 
/// 挂载于mainpage场景的退出游戏按钮
/// </summary>
public class Quit : MonoBehaviour
{
    void Awake()
    {
        Button quit_button=this.GetComponent<Button>();
        quit_button.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
