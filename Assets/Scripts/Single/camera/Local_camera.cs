using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ϸ-����ű�
/// </summary>


public class Local_camera : MonoBehaviour
{
    GameObject player;
    //���
    Vector3 aimpos,disvec;
    //����ƶ�Ŀ��㣬���ָ�����������
    float tan, height, trigle;
    //�нǸ߶�
    public static GameObject cme;
    //���(����)

    private void Awake()
    {
        cme = this.gameObject;
    }

    void Start()
    {

        Init();
    }
    void LateUpdate()
    {
        Set_Aimpos();

        Move_to_aimpos();
    }

    ///////////////////    ���ߺ���   ////////////////////////
    void Init()
    {
        height = 10;
        trigle = 40f;
        tan = Mathf.Tan(trigle);
        player = move.player;

        Set_disvec();
        //����Ĭ��disvec
        Set_Aimpos();
        //����Ŀ���
        Init_camera();
        //��ʼ�����
    }
    //��ʼ��

    void Set_disvec()
    {
        Vector3 touying=(transform.position-player.transform.position).normalized;
        touying = touying * (height / tan);
        disvec.Set(touying.x, height, touying.z);
    }
    void Set_Aimpos()
    {
        aimpos = player.transform.position + disvec;
    }
    void Init_camera()
    {
        transform.position = aimpos;
        transform.LookAt(player.transform);
        transform.GetComponent<Camera>().orthographic = true;
    }
    private void Move_to_aimpos()
    {
        transform.position = Vector3.Lerp(transform.position, aimpos, 0.01f);
    }


    ///////////////////    �ⲿ�ӿ�   ////////////////////////
    
    public Vector3 Get_Right()
    {
        Vector3 right = transform.right;
        right.Set(right.x, 0f, right.z);
        return right.normalized;
    }
    public Vector3 Get_Forward()
    {
        Vector3 forward = transform.forward;
        forward.Set(forward.x, 0f, forward.z);
        return forward.normalized;
    }
}
