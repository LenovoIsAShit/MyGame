using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �˳���Ϸ��ť
/// 
/// ������mainpage�������˳���Ϸ��ť
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
