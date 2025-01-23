using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击检测脚本
/// 
/// 
/// 挂载于Monster
/// </summary>

public class detect_system : MonoBehaviour
{
    bool detected;
    //是否在范围内
    Transform single_camera, detected_image, player;
    //相机,感叹号图片,玩家

    Quaternion dir;
    //图片方向

    float r, degree, sind, cosd;
    //探测半径和夹角

    void Awake()
    {
        Init();
    }

    void FixedUpdate()
    {
        Set_dir();
        //保持方向

        Detect_check();
        //每帧检测
    }

    void Init()
    {
        r = 10f;
        degree = 60f;//度
        sind=Mathf.Sin(degree);
        cosd = Mathf.Cos(degree);

        detected = false;

        detected_image = transform.GetChild(2).GetChild(0);
        single_camera = GameObject.FindGameObjectWithTag("SingleCamera").transform;
        player = GameObject.FindGameObjectWithTag("SinglePlayer").transform;
        var comp = single_camera.GetComponent<Local_camera>();
        dir = Quaternion.LookRotation(comp.Get_Forward());

        Set_dir();
    }

    void Set_dir()
    {
        detected_image.rotation = dir;
    }

    public void Change_attack_state()
    {
        Set_detected_image();

        Set_red_eye_light();
    }
    //改变为发现敌人状态


    void Detect_check()
    {
        if (Check_pos()&&detected==false)
        {
            Change_attack_state();

            detected = true;

            BehaviorTree bt = GetComponent<BehaviorTree>();
            bt.GetVariable("detected").SetValue(true);
            //告诉行为树发现了

        }
        //改变状态后就不检测了
    }
    //如果发现，则改变状态

    /// <summary>
    /// /////////////////////////////////////////////////////////////
    /// </summary>
    
    
    void Set_detected_image()
    {
        detected_image.gameObject.SetActive(true);
    }//设置感叹号图片可见

    void Set_red_eye_light()
    {
        Light light_l = transform.GetChild(0).GetChild(1).GetComponent<Light>();
        Light light_r = transform.GetChild(0).GetChild(2).GetComponent<Light>();
        light_l.color = Color.red;
        light_r.color = Color.red;
    }//眼睛从蓝色变为红色

    bool Check_pos()
    {
        Vector3 p_pos = player.transform.position;
        Vector3 m_pos = transform.position;
        p_pos.Set(p_pos.x, 0f, p_pos.z);
        m_pos.Set(m_pos.x, 0f, m_pos.z);
        //玩家位置和此位置

        Vector3 pl = transform.TransformPoint(-sind, 0f, cosd) - m_pos;
        Vector3 pr = transform.TransformPoint(sind, 0f, cosd) - m_pos;
        //左右界限
        Vector3 p_vec = p_pos - m_pos;
        //敌人指向玩家的向量

        Vector3 res_l = Vector3.Cross(p_vec, pl);
        Vector3 res_r = Vector3.Cross(p_vec, pr);
        //如果在角度内，叉乘左右边界结果方向应该相反

        if (res_l.y * res_r.y < 0)
        {
            //在角度内
            if (r * r >= p_vec.sqrMagnitude)
            {
                //在范围内
                return true;
            }
            else
            {
                //在范围外
                return false;
            }
        }
        else
        {
            //不在角度内
            return false;
        }
    }
    //检查范围内是否有玩家

}
