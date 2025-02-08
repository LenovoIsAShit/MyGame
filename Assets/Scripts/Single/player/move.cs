using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家移动脚本
///     在摄像机执行awake，设置完位置后，根据摄像机与
///     Player的相对位置得到前后左右方向，并移动
/// </summary>

public class move : MonoBehaviour
{
    Vector3 movedir, right, forward;
    GameObject came;

    public static GameObject player;
    //提供给外部

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
        //更新速度

        if (LevelController.Inboss != true)
        {
            Movedir_Judge();
            //更新方向
            Start_Move();
            //移动
        }
    }

    ///////////////////    工具函数   ////////////////////////
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
    //初始化

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

    ///////////////////    外部接口   ////////////////////////

    public Vector3 Get_movedir()
    {
        return movedir;
    }
}
