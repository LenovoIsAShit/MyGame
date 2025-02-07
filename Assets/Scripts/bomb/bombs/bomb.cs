using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹基类
/// 
/// 
/// 
/// </summary>


public abstract class bomb:MonoBehaviour
{
    Vector3 start_dir;
    //起始方向
    public float speed;
    //速度
    public float damage;
    //伤害
    public bool stop;
    //停止标志
    public string beibao_image;
    //武器栏的图片
    public string bomb_name;
    //子弹名字
    bool Check_state;
    //是否每帧检查

    string prefab_name;
    //子弹预设件名
    GameObject fa_prefab;
    //真正的子弹对象

    void FixedUpdate()
    {

        if (Check_state)
        {
            Move();

            Killself();
        }
    }

    public void Init(Vector3 start_dir,float speed,string prefab_name,string beibao_image,string bomb_name, float damage,bool Chek_state=true)
    {
        this.speed = speed;
        this.start_dir = start_dir;
        this.prefab_name = prefab_name;
        this.damage = damage;
        this.beibao_image = beibao_image;
        this.bomb_name = bomb_name;
        this.Check_state = Chek_state;  

        fa_prefab = this.gameObject;
        fa_prefab.transform.rotation = Quaternion.LookRotation(start_dir);
        fa_prefab.AddComponent<BoxCollider>();
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.gameObject.layer = LayerMask.NameToLayer("Bomb");
        //实际的子弹对象


        //GameObject prefab=Instantiate(Resources.Load<GameObject>(prefab_path));
        GameObject prefab = OP.instance.Get(prefab_name);
        prefab.transform.rotation = fa_prefab.transform.rotation;
        prefab.transform.SetParent(fa_prefab.transform);
        prefab.transform.localPosition = new Vector3(0f, 0f, 0f);
        //预制件

    }
    //初始化参数
    public void Move()
    {
        if (stop==false)
        {
            fa_prefab.transform.position += (start_dir.normalized * speed * Time.deltaTime);
        }//如果还没停止则继续移动
    }
    //移动
    private void OnTriggerEnter(Collider other)
    {
        Collide_effect(other);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////

    public abstract void Killself();
    //销毁判断
    public abstract void Collide_effect(Collider other);
    //碰撞效果(用于触发或碰撞)
    public void Set_check_state(bool ck)
    {
        this.Check_state = ck;
    }

    public void Copy(bomb bomb)
    {
        Init(
              move.player.GetComponent<Attack_dir>().Get_dir(),
              bomb.speed,
              bomb.prefab_name,
              bomb.beibao_image,
              bomb.bomb_name,
              bomb.damage
        );
        
    }

}
