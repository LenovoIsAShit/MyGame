using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss�ĵ�Ļ
/// 
/// 
/// </summary>
public class BossBomb : MonoBehaviour
{
    Quaternion dir;
    //ͼƬ��Է���
    Transform single_camera;
    //���
    Transform bomb_image;
    //ͼƬ
    float speed;
    //�ƶ��ٶ�
    Vector3 start_dir;
    //�ƶ�����
    float damage;
    //�˺�

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
    /// ��ʼ��
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
    /// ���ַ���=>��������
    /// </summary>
    void Set_image_dir()
    {
        bomb_image.rotation = dir;
    }


    /// <summary>
    /// �ƶ�
    /// </summary>
    private void Move()
    {
        transform.position += (start_dir.normalized * speed * Time.deltaTime);
    }


    /// <summary>
    /// �ӵ������ж�
    /// </summary>
    void DieCheck()
    {
        if (!grid.Edge_check(this.transform.position))
        {

            OP.instance.Del(this.gameObject);
        }
    }


    /// <summary>
    /// ������
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Blood_system bs = other.GetComponent<Blood_system>();
        bs.Change_hp(-damage);
        OP.instance.Del(this.gameObject);
    }


    /// <summary>
    /// �����ƶ�����
    /// </summary>
    /// <param name="stdir"></param>
    public void Set_Startdir(Vector3 stdir)
    {
        this.start_dir = stdir;
    }
}
