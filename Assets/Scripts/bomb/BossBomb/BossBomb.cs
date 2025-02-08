using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss的弹幕
/// 
/// 
/// </summary>
public class BossBomb : MonoBehaviour
{
    Quaternion dir;
    //图片面对方向
    Transform single_camera;
    //相机
    Transform bomb_image;
    //图片
    float speed;
    //移动速度
    Vector3 start_dir;
    //移动方向
    float damage;
    //伤害

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        Move();

        DieCheck();

        Set_image_dir();
    }




    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        bomb_image = transform.GetChild(0);
        single_camera = Local_camera.cme.transform;
        var comp = single_camera.GetComponent<Local_camera>();
        dir = Quaternion.LookRotation(comp.Get_Forward());

        speed = 10f;
        damage = 20f;

        Set_image_dir();
    }


    /// <summary>
    /// 保持方向=>面对摄像机
    /// </summary>
    void Set_image_dir()
    {
        bomb_image.rotation = dir;
    }


    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        transform.position += (start_dir.normalized * speed * Time.deltaTime);
    }


    /// <summary>
    /// 子弹销毁判断
    /// </summary>
    void DieCheck()
    {
        if (!grid.Edge_check(this.transform.position))
        {

            OP.instance.Del(this.gameObject);
        }
    }


    /// <summary>
    /// 触发器
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Blood_system bs = other.GetComponent<Blood_system>();
        bs.Change_hp(-damage);
        OP.instance.Del(this.gameObject);
    }


    /// <summary>
    /// 设置移动方向
    /// </summary>
    /// <param name="stdir"></param>
    public void Set_Startdir(Vector3 stdir)
    {
        this.start_dir = stdir;
    }
}
