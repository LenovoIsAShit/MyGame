using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 取消房间创建
///   
/// 挂载于提示框
/// </summary>

public class cancel_create : MonoBehaviour
{

    void Awake()
    {
        Button btn = transform.GetChild(2).GetComponent<Button>();
        btn.onClick.AddListener(Close_frame);
    }

    void Close_frame()
    {
        this.gameObject.SetActive(false);
    }
}
