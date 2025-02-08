using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 模型脚本
///     保持模型方向和改变动作状态
/// </summary>


public class model : MonoBehaviour
{
    Transform mdl,came;
    //模型
    Animator ani;
    
    void Awake()
    {
        mdl = transform.GetChild(0);
        ani = mdl.GetComponent<Animator>();
    }

    void Start()
    {
        came = Local_camera.cme.transform;
        Local_camera cm = came.GetComponent<Local_camera>();
        mdl.rotation = Quaternion.LookRotation(cm.Get_Forward());//初始朝向
    }

    private void FixedUpdate()
    {
        if (!LevelController.Inboss)
        {
            Update_Model_State();
        }
    }

    void LateUpdate()
    {
        Update_Dir();
    }

    ///////////////////    工具函数   ////////////////////////

    void Update_Dir()
    {
        move mv=GetComponent<move>();
        Vector3 md = mv.Get_movedir();
        if (md.x != 0f || md.z != 0f) mdl.rotation = Quaternion.LookRotation(md);
    }

    void Update_Model_State()
    {
        bool running = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
                       Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        bool jumping = Input.GetKey(KeyCode.Space);

        bool dance = Input.GetKey(KeyCode.R);

        if (dance)
        {
            ani.SetBool("jump", false);
            ani.SetBool("run", false);
            ani.SetBool("dance", true);
        }
        else
        {
            ani.SetBool("dance", false);

            if (running)
            {
                ani.SetBool("run", true);

                if (jumping) ani.SetBool("rtj", true);
                else ani.SetBool("rtj", false);
            }
            else
            {
                ani.SetBool("run", false);

                if (jumping) ani.SetBool("jump", true);
                else ani.SetBool("jump", false);
            }
        }
    }
}
