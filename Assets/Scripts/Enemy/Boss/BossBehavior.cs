using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossBehavior : MonoBehaviour
{
    float speed, acc;
    //速度和加速度
    List<Vector3> pos;
    //移动目标点
    int pos_index;
    //现在正在往哪个点移动
    float t,remt;
    //弹幕间隔时间(毫秒)，现在记录时间，上一次记录时间
    float state_t, state_remt;
    //状态时间记录
    bool inraying;
    //是否正在发波中
    public static BossBehavior Instance;
    //单例

    /// <summary>
    /// 状态
    /// </summary>
    public enum State
    {
        Ray,
        //放波
        chase,
        //追踪
    }
    [HideInInspector]
    public State now;


    private void Awake()
    {
        Init();

        Instance=GetComponent<BossBehavior>();
    }

    private void Update()
    {

        StateCheck();
        //状态检查

        FSM(now);
        //状态机

        SpeedCheck();
        //速度

    }



    /// <summary>
    /// 有限状态机
    /// </summary>
    /// <param name="now"></param>
    void FSM(State now)
    {
        switch (now)
        {
            case State.Ray:
                if (inraying==false)
                {
                    ReleaseRay();
                }
                break;
            case State.chase:
                ChasePlayer();
                break;
        }
    }
    
    

    /// <summary>
    /// 放波状态=》横扫全屏
    /// </summary>
    void ReleaseRay()
    {
        inraying = true;

        StartCoroutine(StartRR());
    }

    IEnumerator StartRR()
    {
        Vector3 pos = transform.position;
        pos.Set(pos.x, 208.56f, pos.z);
        LookAt(move.player.transform.position);

        GameObject rayl = OP.instance.Get("ray");
        rayl.transform.position = pos;
        rayl.transform.localScale = new Vector3(1f, 1f, 1f);
        rayl.SetActive(true);
        GameObject rayr = OP.instance.Get("ray");
        rayr.transform.position = pos;
        rayr.transform.localScale = new Vector3(1f, 1f, 1f);
        rayr.SetActive(true);
        Vector3 dirl = transform.TransformVector(new Vector3(-1f, 0f, 1f));
        rayl.transform.rotation = Quaternion.LookRotation(dirl);
        Vector3 dirr = transform.TransformVector(new Vector3(1f, 0f, 1f));
        rayr.transform.rotation = Quaternion.LookRotation(dirr);
        //第一次两边

        GameObject last = OP.instance.Get("ray");
        last.transform.position= pos;
        last.transform.rotation = Quaternion.LookRotation(transform.forward);
        last.transform.localScale = new Vector3(15f, 15f, 15f);
        last.SetActive(false);
        //第二次中间

        Sequence sq1 = DOTween.Sequence();
        Tween t1 = rayl.transform.DORotateQuaternion(Quaternion.LookRotation(transform.forward), 1f).SetAutoKill(true).Pause();
        Tween t2 = rayr.transform.DORotateQuaternion(Quaternion.LookRotation(transform.forward), 1f).SetAutoKill(true).Pause();
        //第一次动画

        last.transform.localScale = new Vector3(15f, 15f, 15f);
        Tween t3 = last.transform.DOScale(new Vector3(0f, 0f, 0f), 2f).SetAutoKill(true).Pause();
        //第二次动画

        t3.OnComplete(() =>
        {
            OP.instance.Del(last);
            OP.instance.Del(rayl);
            OP.instance.Del(rayr);

            now = State.chase;
            inraying = false;
        });

        sq1.Join(t1).Pause();
        sq1.Join(t2).Pause();
        sq1.OnComplete(() =>
        {
            RaycastHit hf;
            if(Physics.Raycast(transform.position,transform.forward,out hf))
            {
                if (hf.collider.gameObject.tag.Equals("SinglePlayer"))
                {
                    var cmp = hf.collider.gameObject.GetComponent<Blood_system>();
                    cmp.Change_hp(-60f);
                }
            }//射线检测

            rayl.SetActive(false);
            rayr.SetActive(false);
            last.SetActive(true);
            t3.Play();
        });

        sq1.Play();

        yield break;
    }



    /// <summary>
    /// 每隔固定时间就要发射一次弹幕
    /// </summary>
    void Barrage()
    {
        if (Time.time - remt >= t)
        {
            ReleaseBarrage();
            remt = Time.time;
        }
    }

    /// <summary>
    /// 动作 =》向八个方向发射弹幕
    /// </summary>
    void ReleaseBarrage()
    {
        Vector3[] dirs = new Vector3[8];
        float[] dz = { 1, 1, 0, -1, -1, -1, 0, 1 };
        float[] dx = { 0, 1, 1, 1, 0, -1, -1, -1 };
        for (int i = 0; i < 8; i++)
        {
            dirs[i] = new Vector3(dx[i], 0, dz[i]);
            dirs[i] = transform.TransformVector(dirs[i]).normalized;
        }

        for(int i = 0; i < 8; i++)
        {
            
            GameObject obj = OP.instance.Get("barrage");
            obj.SetActive(true);
            var cmp = obj.GetComponent<BossBomb>();
            cmp.Set_Startdir(dirs[i]);
            obj.transform.position = new Vector3(transform.position.x, 208.777f, transform.position.z);
        }
    }

    /// <summary>
    /// 追逐状态=》追着普蕾尔打
    /// </summary>
    void ChasePlayer()
    {
        if (pos_index >= pos.Count)
        {
            CleanUpList();
            GetNewList();
        }
        else
        {


            Vector3 aim = pos[pos_index];
            Vector3 dir = aim - transform.position;
            float dis_d_2 = speed * speed * Time.deltaTime * Time.deltaTime;
            float dis_need_2 = dir.sqrMagnitude;
            if (dis_d_2 >= dis_need_2)
            {
                transform.position = aim;
                pos_index++;
            }
            else
            {
                transform.position += (dir.normalized * speed * Time.deltaTime);
            }
        }

        Barrage();
        //弹幕
        LookAt(move.player.transform.position);
        //注视玩家
    }



    /// <summary>
    /// 每隔固定时间切换状态
    /// </summary>
    void StateCheck()
    {
        if (Time.time - state_remt >= state_t)
        {
            if (now == State.chase)
            {
                now = State.Ray;
                state_remt = Time.time;
            }
        }
    }





    /// <summary>
    /// 总初始化
    /// </summary>
    private void Init()
    {
        speed = 0f;
        acc = 2.4f;

        inraying = false;

        t = 0.9f;
        remt = Time.time;
        state_t = 10f;
        state_remt = Time.time;

        now = State.chase;

        pos_index = 0;
        pos = new List<Vector3>();

    }


    /// <summary>
    /// 清除路径点 
    /// </summary>
    void CleanUpList()
    {

        pos.Clear();
        //销毁pos

        pos_index=0;
        //重置下标
    }


    /// <summary>
    /// 获得新路径点
    /// </summary>
    void GetNewList()
    {
        pos = grid.Astar(transform.position, move.player.transform.position);
    }


    /// <summary>
    /// 速度更改
    /// </summary>
    void SpeedCheck()
    {
        if (speed < acc)
        {
            speed += acc * Time.deltaTime;
        }
        else
        {
            speed = acc;
        }
    }


    /// <summary>
    /// 注视玩家
    /// </summary>
    void LookAt(Vector3 pos)
    {
        Vector3 vec = (pos - transform.position).normalized;
        vec.Set(vec.x, 0f, vec.z);
        transform.rotation= Quaternion.LookRotation(vec);
    }
}
