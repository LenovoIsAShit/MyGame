using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ������Ϸѡ��ť 
/// 
/// ������mainpage�����ĵ�����Ϸ��ť
/// </summary>
public class Multi : MonoBehaviour
{
    void Awake()
    {
        Button single_model = this.GetComponent<Button>();
        single_model.onClick.AddListener(StartSingleModel);
    }

    private void StartSingleModel()
    {
        Image ima = this.transform.parent.transform.GetChild(3).GetComponent<Image>();
        ima.enabled = true;

        SceneManager.LoadScene("Multi");
    }
}
