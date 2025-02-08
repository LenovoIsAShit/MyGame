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
        Collide_check(other);
    }

    void Collide_check(Collider other)
    {
        switch (other.tag)
        {
            case "Monster":
                Collide_Monster(other);
                break;
            case "Boss":
                Collide_Boss(other);
                break;
        }
    }

    void Collide_Monster(Collider other)
    {
        Blood_system bs = other.gameObject.GetComponent<Blood_system>();
        if (bs.Get_hp() > 0)
        {
            bs.Change_hp(-5);
        }
    }

    void Collide_Boss(Collider other)
    {
        Boss_Blood_System.instance.Change_hp(-5 * 0.3f);
    }
}
