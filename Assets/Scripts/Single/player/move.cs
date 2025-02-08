using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ƶ��ű�
///     �������ִ��awake��������λ�ú󣬸����������
///     Player�����λ�õõ�ǰ�����ҷ��򣬲��ƶ�
/// </summary>

public class move : MonoBehaviour
{
    Vector3 movedir, right, forward;
    GameObject came;

    public static GameObject player;
    //�ṩ���ⲿ

    public static float acceleration, speed;

    void Awake()
    {
        player = this.gameObject;
    }

    void Start()
    {
        Init();

    }

    void FixedUpdate()
    {
        Speed_Judge();
        //�����ٶ�

        if (LevelController.Inboss != true)
        {
            Movedir_Judge();
            //���·���
            Start_Move();
            //�ƶ�
        }
    }

    ///////////////////    ���ߺ���   ////////////////////////
    void Init()
    {
        acceleration = 3f;
        speed = 0f;
        movedir = new Vector3(0f, 0f, 0f);
        forward = new Vector3(0f, 0f, 0f);
        right = new Vector3(0f, 0f, 0f);
        came = Local_camera.cme;

        Set_Forward();
        Set_Right();
    }
    //��ʼ��

    void Set_Forward()
    {
        forward = came.GetComponent<Local_camera>().Get_Forward();
    }

    void Set_Right()
    {
        right = came.GetComponent<Local_camera>().Get_Right();
    }

    void Speed_Judge()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (speed < acceleration)
            {
                speed += (Time.deltaTime * acceleration);
            }
            else
            {
                speed = acceleration;
            }
        }
        else
        {
            speed -= (Time.deltaTime * acceleration * 3f);
            if (speed <= 0) speed = 0;
        }
    }
    void Movedir_Judge()
    {
        if (Input.GetKey(KeyCode.W)) movedir += forward;
        if (Input.GetKey(KeyCode.S)) movedir -= forward;
        if (Input.GetKey(KeyCode.A)) movedir -= right;
        if (Input.GetKey(KeyCode.D)) movedir += right;
        movedir = movedir.normalized;
    }
    void Start_Move()
    {
        if (movedir.x != 0f || movedir.z != 0f) transform.rotation = Quaternion.LookRotation(movedir);
        if(!Input.GetKey(KeyCode.R))transform.position += (speed * Time.deltaTime * movedir);
    }

    ///////////////////    �ⲿ�ӿ�   ////////////////////////

    public Vector3 Get_movedir()
    {
        return movedir;
    }
}
