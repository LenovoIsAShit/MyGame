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
    Vector3 start_pos;
    //起始位置
    Vector3 start_dir;
    //起始方向
    public float speed;
    //速度
    string prefab_path;
    //子弹预制件asset路径
    GameObject fa_prefab;
    //真正的子弹对象
    public float damage;
    //伤害
    public bool stop;
    //停止标志

    void FixedUpdate()
    {
        Move();

        Killself();
    }

    public void Init(Vector3 start_pos,Vector3 start_dir,float speed,string prefab_path,float damage)
    {
        this.speed = speed;
        this.start_pos= start_pos;
        this.start_dir = start_dir;
        this.prefab_path = prefab_path;
        this.damage = damage;

        fa_prefab = this.gameObject;
        fa_prefab.transform.position = start_pos;
        fa_prefab.transform.rotation = Quaternion.LookRotation(start_dir);
        fa_prefab.AddComponent<BoxCollider>();
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.gameObject.layer = LayerMask.NameToLayer("Bomb");
        //实际的子弹对象

        GameObject prefab=Instantiate(Resources.Load<GameObject>(prefab_path));
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
}
