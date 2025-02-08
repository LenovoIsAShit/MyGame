using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP管理脚本
///     提供增减血量方法
/// 
/// 挂载于Player
/// </summary>
public class Blood_system : MonoBehaviour
{
    float hp,max_hp;
    //血量

    Transform blood_image;
    //血量子组件
    Transform single_camera;
    //相机

    Quaternion dir;
    //图片面对方向

    void Start()
    {
        //start是因为先要等待相机awake设置
        Init();

    }


    void LateUpdate()
    {
        //lateupdate等随父组件旋转后，再设置，否则会闪烁
        Set_dir();


    }

    void Init()
    {
        blood_image=transform.GetChild(1);
        single_camera = Local_camera.cme.transform;
        var comp = single_camera.GetComponent<Local_camera>();
        dir = Quaternion.LookRotation(comp.Get_Forward());
        //血量朝向

        hp = 100f;
        max_hp = 100f;

        Set_dir();
    }

    void Set_dir()
    {
        blood_image.rotation = dir;
    }
    //每帧设置方向

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
        //死亡判断

    }
    //改变血量
    IEnumerator Cg(float dx)
    {
        RectTransform quick = null, slow = null;//快慢变换
        
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
            //快的血条动画

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
            //慢的血条变化
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
    //死亡判断

    public float Get_hp()
    {
        return hp;
    }//返回血量


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
    //掉落物品轮询

    void HealCube()
    {
        System.Random r = new System.Random();
        if (r.NextDouble() >= 0.6f)
        {
            GameObject hc = OP.Instance.Get("HealCube");
            hc.transform.position = new Vector3(this.gameObject.transform.position.x, 208.7f, this.gameObject.transform.position.z);
        }

    }
    //治疗物品


    void Monster_die()
    {
        gameObject.SetActive(false);

        FallingOffCheck();
        //生成掉落轮询

        EnemyController.enemy_Controller.Decrease_nownum();

        LevelController.levelcontrol.Check_level();

        OP.instance.Del(this.gameObject);
    }
    //怪物死亡


    void SinglePlayer_die()
    {
        LevelController.Inboss = true;
        Player_die.quitUI.Show();
    }
    //玩家死亡
}
