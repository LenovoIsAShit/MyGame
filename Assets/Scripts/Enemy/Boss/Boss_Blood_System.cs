using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss��Ѫ����ʾ�͹���
/// 
/// 
/// </summary>


public class Boss_Blood_System : MonoBehaviour
{
    public static Boss_Blood_System instance;
    //����
    float hp, max_hp;
    //Ѫ��
    Transform blood_image;
    //Ѫ�������


    private void Awake()
    {
        instance=GetComponent<Boss_Blood_System>();

        Init();
    }


    void Init()
    {
        blood_image = transform.GetChild(0);

        hp = 100f;
        max_hp = 100f;
    }

    public void Change_hp(float dx)
    {
        if (dx + hp > 0)
        {
            if (dx + hp >= max_hp)
            {
                float t = 100 - hp;
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

        if (hp <= 0)
        {
            Die();
        }
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
            quick.offsetMax = new Vector2(-((max_hp - hp) / max_hp) * 477f, 0f);
            quick.offsetMin = new Vector2(-((max_hp - hp) / max_hp) * 477f, 0f);
            //���Ѫ������


            yield return 0;

            float nx = Mathf.Lerp(slow.offsetMax.x, quick.offsetMax.x, 0.01f);
            slow.offsetMax = new Vector2(nx, 0f);
            slow.offsetMin = new Vector2(nx, 0f);


            if (Mathf.Abs(slow.offsetMax.x - quick.offsetMax.x)/477f <= 0.005f)
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
        Time.timeScale = 0.3f;

        BossBehavior.Instance.enabled = false;

        win.quitUI.Show();

    }//���BOSS��ͨ��

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


}
