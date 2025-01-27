using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ӵ�����
/// 
/// 
/// 
/// </summary>


public abstract class bomb:MonoBehaviour
{
    Vector3 start_dir;
    //��ʼ����
    public float speed;
    //�ٶ�
    string prefab_path;
    //�ӵ�Ԥ�Ƽ�asset·��
    GameObject fa_prefab;
    //�������ӵ�����
    public float damage;
    //�˺�
    public bool stop;
    //ֹͣ��־
    public string beibao_image_path;
    //��������ͼƬ
    public string bomb_name;
    //�ӵ�����
    bool Check_state;
    //�Ƿ�ÿ֡���


    void FixedUpdate()
    {

        if (Check_state)
        {
            Move();

            Killself();
        }
    }

    public void Init(Vector3 start_dir,float speed,string prefab_path,string beibao_image_path,string bomb_name, float damage,bool Chek_state=true)
    {
        this.speed = speed;
        this.start_dir = start_dir;
        this.prefab_path = prefab_path;
        this.damage = damage;
        this.beibao_image_path = beibao_image_path;
        this.bomb_name = bomb_name;
        this.Check_state = Chek_state;  

        fa_prefab = this.gameObject;
        fa_prefab.transform.rotation = Quaternion.LookRotation(start_dir);
        fa_prefab.AddComponent<BoxCollider>();
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.gameObject.layer = LayerMask.NameToLayer("Bomb");
        //ʵ�ʵ��ӵ�����

        GameObject prefab=Instantiate(Resources.Load<GameObject>(prefab_path));
        prefab.transform.rotation = fa_prefab.transform.rotation;
        prefab.transform.SetParent(fa_prefab.transform);
        prefab.transform.localPosition = new Vector3(0f, 0f, 0f);
        //Ԥ�Ƽ�

    }
    //��ʼ������
    public void Move()
    {
        if (stop==false)
        {
            fa_prefab.transform.position += (start_dir.normalized * speed * Time.deltaTime);
        }//�����ûֹͣ������ƶ�
    }
    //�ƶ�
    private void OnTriggerEnter(Collider other)
    {
        Collide_effect(other);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////

    public abstract void Killself();
    //�����ж�
    public abstract void Collide_effect(Collider other);
    //��ײЧ��(���ڴ�������ײ)
    public void Set_check_state(bool ck)
    {
        this.Check_state = ck;
    }

    public void Copy(bomb bomb)
    {
        Init(
              move.player.GetComponent<Attack_dir>().Get_dir(),
              bomb.speed,
              bomb.prefab_path,
              bomb.beibao_image_path,
              bomb.bomb_name,
              bomb.damage
        );
        
    }

}
