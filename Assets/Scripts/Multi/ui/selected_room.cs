using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����ѡ��
///     ��Ҫ���ص�ÿ�����䰴ť�ϣ�����ѡ��ķ���
///     
/// ������Ԥ�Ƽ�room����
/// </summary>

public class selected_room : MonoBehaviour
{

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void Awake()
    {
        enter_room.room_ip = "null";
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(update_room_ip);
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void update_room_ip()
    {
        Debug.Log(this.transform.name);

        TMP_Text tt = transform.GetChild(0).GetComponent<TMP_Text>();

        string[] str = tt.text.Split(":");

        enter_room.room_ip = str[1];
    }
}
