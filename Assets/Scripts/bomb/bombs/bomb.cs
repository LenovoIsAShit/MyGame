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
    Vector3 start_pos;
    //��ʼλ��
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
}
