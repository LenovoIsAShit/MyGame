using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ű�
/// 
/// 
/// ������Monster
/// </summary>

public class detect_system : MonoBehaviour
{
    bool detected;
    //�Ƿ��ڷ�Χ��
    Transform single_camera, detected_image, player;
    //���,��̾��ͼƬ,���

    Quaternion dir;
    //ͼƬ����

    float r, degree, sind, cosd;
    //̽��뾶�ͼн�

    void Awake()
    {
        Init();
    }

    void FixedUpdate()
    {
        Set_dir();
        //���ַ���

        Detect_check();
        //ÿ֡���
    }

    void Init()
    {
        r = 10f;
        degree = 60f;//��
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
    //�ı�Ϊ���ֵ���״̬


    void Detect_check()
    {
        if (Check_pos()&&detected==false)
        {
            Change_attack_state();

            detected = true;

            BehaviorTree bt = GetComponent<BehaviorTree>();
            bt.GetVariable("detected").SetValue(true);
            //������Ϊ��������

        }
        //�ı�״̬��Ͳ������
    }
    //������֣���ı�״̬

    /// <summary>
    /// /////////////////////////////////////////////////////////////
    /// </summary>
    
    
    void Set_detected_image()
    {
        detected_image.gameObject.SetActive(true);
    }//���ø�̾��ͼƬ�ɼ�

    void Set_red_eye_light()
    {
        Light light_l = transform.GetChild(0).GetChild(1).GetComponent<Light>();
        Light light_r = transform.GetChild(0).GetChild(2).GetComponent<Light>();
        light_l.color = Color.red;
        light_r.color = Color.red;
    }//�۾�����ɫ��Ϊ��ɫ

    bool Check_pos()
    {
        Vector3 p_pos = player.transform.position;
        Vector3 m_pos = transform.position;
        p_pos.Set(p_pos.x, 0f, p_pos.z);
        m_pos.Set(m_pos.x, 0f, m_pos.z);
        //���λ�úʹ�λ��

        Vector3 pl = transform.TransformPoint(-sind, 0f, cosd) - m_pos;
        Vector3 pr = transform.TransformPoint(sind, 0f, cosd) - m_pos;
        //���ҽ���
        Vector3 p_vec = p_pos - m_pos;
        //����ָ����ҵ�����

        Vector3 res_l = Vector3.Cross(p_vec, pl);
        Vector3 res_r = Vector3.Cross(p_vec, pr);
        //����ڽǶ��ڣ�������ұ߽�������Ӧ���෴

        if (res_l.y * res_r.y < 0)
        {
            //�ڽǶ���
            if (r * r >= p_vec.sqrMagnitude)
            {
                //�ڷ�Χ��
                return true;
            }
            else
            {
                //�ڷ�Χ��
                return false;
            }
        }
        else
        {
            //���ڽǶ���
            return false;
        }
    }
    //��鷶Χ���Ƿ������

}
