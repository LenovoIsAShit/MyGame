using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ÷Œ¡∆∑ΩøÈ
/// </summary>
public class HealCube : MonoBehaviour
{


    public void OnTriggerEnter(Collider other)
    {
        Blood_system bs=other.gameObject.GetComponent<Blood_system>();
        bs.Change_hp(30);

        OP.Instance.Del(this.gameObject);
    }


}
