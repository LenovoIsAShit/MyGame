using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 射击,发射子弹
/// 
///     按一次只会发射一次
/// 
/// </summary>


public class shoot : MonoBehaviour
{
    GameObject controller;

    bool shooting;
    //射击中

    private void Start()
    {
        shooting = false;
        controller = GameObject.FindGameObjectWithTag("BombController");
    }

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && shooting == false)
        {
            shooting = true;
            var cmp = controller.GetComponent<bomb_testFactory>();
            cmp.Create(this.gameObject);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && shooting == true)
        {
            shooting = false;
        }
    }
}
