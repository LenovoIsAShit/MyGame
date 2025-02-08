using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试子弹
/// 
/// 
/// </summary>

public class bomb_test : bomb
{
    Transform bomb;
    //子弹的组件

    void Start()
    {
        base.damage = damage;
        base.speed = 10f;

        bomb = this.transform.GetChild(0);

        base.beibao_image = "1";
    }
    //修改属性


    public override void Collide_effect(Collider other)
    {
        switch (other.tag)
        {
            case "Monster":
                Monster_collide(other);
                break;
            case "Boss":
                Boss_collide(other);
                break;
    }

    }
    //子弹触发


    public override void Killself()
    {
        if (!grid.Edge_check(this.transform.position))
        {

            OP.instance.Del(this.gameObject);
        }
    }
    //子弹自杀判断

    void Exploded()
    {
        //GameObject exp = Instantiate(Resources.Load<GameObject>("prefabs/Bomb/star_bomb/boom"));
        GameObject exp = OP.Instance.Get("boom");
        exp.transform.position = this.transform.position;
        exp.GetComponent<ParticleSystem>().Play();
    }
    //爆炸效果



    void Monster_collide(Collider other)
    {
        Blood_system bs = other.gameObject.GetComponent<Blood_system>();
        if (bs.Get_hp() > 0)
        {
            //停止移动
            base.stop = true;

            //只有血量大于0才会减少血量
            bs.Change_hp(-damage);


            //爆炸效果
            this.gameObject.SetActive(false);
            Exploded();
            //Destroy(this.gameObject);
            OP.instance.Del(this.gameObject);
        }
    }
    //如果撞的是Monster


    void Boss_collide(Collider other)
    {
        Boss_Blood_System.instance.Change_hp(-damage*0.1f);

        OP.instance.Del(this.gameObject);
    }
    //如果撞的是Boss
}
