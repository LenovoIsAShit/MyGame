using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���߰�ť
/// </summary>

public class botton_Dj : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowWupingPage);
    }

    void ShowWupingPage()
    {
        transform.parent.GetChild(1).gameObject.SetActive(true);
        transform.parent.GetChild(0).gameObject.SetActive(false);
    }
}
