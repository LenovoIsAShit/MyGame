using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP����ű�
///     �ṩ����Ѫ������
/// 
/// ������Player
/// </summary>
public class Blood_system : MonoBehaviour
{
    float hp,max_hp;
    //Ѫ��

    Transform blood_image;
    //Ѫ�������
    Transform single_camera;
    //���

    Quaternion dir;
    //ͼƬ��Է���

    void Start()
    {
        //start����Ϊ��Ҫ�ȴ����awake����
        Init();

    }


    void LateUpdate()
    {
        //lateupdate���游�����ת�������ã��������˸
        Set_dir();


    }

    void Init()
    {
        blood_image=transform.GetChild(1);
        single_camera = Local_camera.cme.transform;
        var comp = single_camera.GetComponent<Local_camera>();
        dir = Quaternion.LookRotation(comp.Get_Forward());
        //Ѫ������

        hp = 100f;
        max_hp = 100f;

        Set_dir();
    }

    void Set_dir()
    {
        blood_image.rotation = dir;
    }
    //ÿ֡���÷���

    public void Change_hp(float dx)
    {
        if (dx + hp > 0)
        {
            if (dx + hp >= max_hp) {
                float t = 100-hp;
                hp = 100;
                StartCoroutine("Cg", t);
            }
            else
            {
                hp += dx;
                StartCoroutine("Cg", dx);
            }
        }
        else 
        {
            hp = 0;
            StartCoroutine("Cg", -hp); 
        }


        Die();
        //�����ж�

    }
    //�ı�Ѫ��
    IEnumerator Cg(float dx)
    {
        RectTransform quick = null, slow = null;//�����任
        
        if (dx > 0)
        {
            quick = blood_image.GetChild(0).GetComponent<RectTransform>();
            slow = blood_image.GetChild(1).GetComponent<RectTransform>();
        }
        else
        {
            quick = blood_image.GetChild(1).GetComponent<RectTransform>();
            slow = blood_image.GetChild(0).GetComponent<RectTransform>();
        }

        while (true)
        {
            quick.offsetMax = new Vector2(-(100f - hp) / max_hp, 0f);
            quick.offsetMin = new Vector2(-(100f - hp) / max_hp, 0f);
            //���Ѫ������

            yield return 0;

            float nx = Mathf.Lerp(slow.offsetMax.x, quick.offsetMax.x, 0.01f);
            slow.offsetMax = new Vector2(nx, 0f);
            slow.offsetMin = new Vector2(nx, 0f);


            if (Mathf.Abs(slow.offsetMax.x - quick.offsetMax.x) <= 0.005f)
            {
                yield return 0;
                slow.offsetMax = quick.offsetMax;
                slow.offsetMin = quick.offsetMin;


                yield break;
            }
            //����Ѫ���仯
        }


    }

    void Die()
    {
        if (hp == 0)
        {
            switch (gameObject.tag)
            {
                case "Monster":
                    Monster_die();
                    break;
                case "SinglePlayer":
                    SinglePlayer_die();
                    break;
            }
        }
    }
    //�����ж�

    public float Get_hp()
    {
        return hp;
    }//����Ѫ��


    public void Reset_hp()
    {
        Init();

        hp = 100;

        RectTransform quick = blood_image.GetChild(0).GetComponent<RectTransform>();
        RectTransform slow = blood_image.GetChild(1).GetComponent<RectTransform>();
        quick.offsetMax = new Vector2(0f, 0f);
        quick.offsetMin = new Vector2(0f, 0f);
        slow.offsetMax = new Vector2(0f, 0f);
        slow.offsetMin = new Vector2(0f, 0f);
    }

    void FallingOffCheck()
    {
        HealCube();
    }
    //������Ʒ��ѯ

    void HealCube()
    {
        System.Random r = new System.Random();
        if (r.NextDouble() >= 0.6f)
        {
            GameObject hc = OP.Instance.Get("HealCube");
            hc.transform.position = new Vector3(this.gameObject.transform.position.x, 208.7f, this.gameObject.transform.position.z);
        }

    }
    //������Ʒ


    void Monster_die()
    {
        gameObject.SetActive(false);

        FallingOffCheck();
        //���ɵ�����ѯ

        EnemyController.enemy_Controller.Decrease_nownum();

        LevelController.levelcontrol.Check_level();

        OP.instance.Del(this.gameObject);
    }
    //��������


    void SinglePlayer_die()
    {
        LevelController.Inboss = true;
        Player_die.quitUI.Show();
    }
    //�������
}
