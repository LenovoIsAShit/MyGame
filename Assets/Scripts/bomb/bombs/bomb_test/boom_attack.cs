using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ±¬Õ¨Ð§¹û-Åö×²¼ì²â
/// 
/// 
/// </summary>


public class boom_attack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Blood_system bs = other.gameObject.GetComponent<Blood_system>();
        if (bs.Get_hp() > 0)
        {
            bs.Change_hp(-5);
        }
    }
}
