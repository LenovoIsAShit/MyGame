using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 单人游戏选择按钮 
/// 
/// 挂载于mainpage的多人联机按钮
/// </summary>
public class Single : MonoBehaviour
{
    void Awake()
    {
        Button single_model=this.GetComponent<Button>();
        single_model.onClick.AddListener(StartSingleModel);
    }

    private void StartSingleModel()
    {
        Image ima = this.transform.parent.transform.GetChild(3).GetComponent<Image>();
        ima.enabled = true;

        SceneManager.LoadScene("Single");
    }
}
