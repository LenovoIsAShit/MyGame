using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUi : MonoBehaviour
{
    public static LevelUi levelui;
    private void Awake()
    {
        levelui = GetComponent<LevelUi>();
    }

    public void Set_level_text(int i)
    {
        TMP_Text tx = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        tx.text= "LEVEL"+i.ToString();
    }
}
