using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beibao : MonoBehaviour
{
    public static Beibao beibao;

    private void Awake()
    {
        beibao = gameObject.GetComponent<Beibao>();
    }
    

    public void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Cancel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    
}
