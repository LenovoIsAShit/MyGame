using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ӵ�
/// 
/// 
/// </summary>

public class bomb_test : bomb
{
    Transform bomb;
    //�ӵ������

    void Start()
    {
        base.damage = damage;
        base.speed = 10f;

        bomb = this.transform.GetChild(0);

        base.beibao_image = "1";
    }
    //�޸�����


    public override void Collide_effect(Collider other)
    {
        Blood_system bs = other.gameObject.GetComponent<Blood_system>();
        if (bs.Get_hp() > 0) 
        {
            //ֹͣ�ƶ�
            base.stop = true;

            //ֻ��Ѫ������0�Ż����Ѫ��
            bs.Change_hp(-damage);



            //��ըЧ��
            this.gameObject.SetActive(false);
            Exploded();
            //Destroy(this.gameObject);
            OP.instance.Del(this.gameObject);
        }
    }
    //�ӵ�����


    public override void Killself()
    {
        if (!grid.Edge_check(this.transform.position))
        {
            OP.instance.Del(this.gameObject);
        }
    }
    //�ӵ���ɱ�ж�

    void Exploded()
    {
        //GameObject exp = Instantiate(Resources.Load<GameObject>("prefabs/Bomb/star_bomb/boom"));
        GameObject exp = Instantiate(OP.Instance.Get("boom"));
        exp.transform.position = this.transform.position;
        exp.GetComponent<ParticleSystem>().Play();
    }
    //��ըЧ��
}
