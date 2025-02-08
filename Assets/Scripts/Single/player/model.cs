using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ģ�ͽű�
///     ����ģ�ͷ���͸ı䶯��״̬
/// </summary>


public class model : MonoBehaviour
{
    Transform mdl,came;
    //ģ��
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
        mdl.rotation = Quaternion.LookRotation(cm.Get_Forward());//��ʼ����
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

    ///////////////////    ���ߺ���   ////////////////////////

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
