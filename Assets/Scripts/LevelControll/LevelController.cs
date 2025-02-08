using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 关卡控制器
///         
/// 
/// 
///         控制关卡流程
/// </summary>
public class LevelController : MonoBehaviour
{
    public static LevelController levelcontrol;

    Vector3 player_start_pos;//玩家起始位置
    int level;//当前关卡
    public static bool Inboss;//boss过度状态

    private void Awake()
    {
        levelcontrol = GetComponent<LevelController>();

        player_start_pos = new Vector3(419.2857f, 208.1429f, 0f);
        level = 1;
    }

    private void Start()
    {
        ShowLevel(level);
        Inboss = false;
    }




    void Start_level(int i)
    {
        EnemyController.Set_max_num(level + 5);
        //EnemyController.Set_max_num(1);

        EnemyController.enemy_Controller.Start_make_enemy();

        //move.player.transform.position = player_start_pos;
    }

    void ShowLevel(int i)
    {

        LevelUi.levelui.Set_level_text(i);


        float startoffset=577f;
        float endoffset=570f;

        RectTransform rt = LevelUi.levelui.transform.GetChild(0).GetComponent<RectTransform>();
        rt.offsetMin = new Vector2(-startoffset, 0);
        rt.offsetMax = new Vector2(-startoffset, 0);

        Sequence sq1 = DOTween.Sequence();
        Tween t1= DOTween.To(
            () => rt.offsetMax,
            x => rt.offsetMax = x,
            new Vector2(0f, 0f),
            0.8f
            ).Pause();
        Tween t2 = DOTween.To(
            () => rt.offsetMin,
            x => rt.offsetMin = x,
            new Vector2(0f, 0f),
            0.8f
            ).Pause();

        sq1.Join(t1).Pause();
        sq1.Join(t2).Pause();
        
        Sequence sq2 = DOTween.Sequence();
        Tween t3 = DOTween.To(
            () => rt.offsetMax,
            x => rt.offsetMax = x,
            new Vector2(endoffset, 0f),
            0.8f
            ).Pause();
        Tween t4 = DOTween.To(
            () => rt.offsetMin,
            x => rt.offsetMin = x,
            new Vector2(endoffset, 0f),
            0.8f
            ).Pause();

        sq2.Join(t3).Pause();
        sq2.Join(t4).Pause();

        sq1.OnComplete(() => StartCoroutine("keepgoing", sq2));
        sq2.OnComplete(() => Start_level(i));
        sq1.Play();
    }

    public void NextLevel()
    {
        level++;
    }


    IEnumerator keepgoing(Sequence sq2)
    {
        yield return new WaitForSeconds(1.3f);
       

        sq2.Play();

        yield break;
    }

    IEnumerator StatNextLevel()
    {
        yield return new WaitForSeconds(1.3f);

        EnemyController.enemy_Controller.Reset_nownum(0);

        NextLevel();

        ShowLevel(level);

        yield break;
    }
    //下一关


    public void Check_level()
    {
        if (EnemyController.enemy_Controller.Get_nownum() == 0)
        {
            if (level <= 2)
            {
                StartCoroutine(StatNextLevel());
            }else
            {
                StartCoroutine(StartBossLevel());
                /*
                OP.instance.Get("Boss1");
                OP.instance.Get("Boss_blood_system");*/
            }
        }

    }

    IEnumerator StartBossLevel()
    {
        Inboss = true;
        //切换BOSS状态

        GameObject boss_b = OP.instance.Get("Boss_blood_system");
        boss_b.SetActive(false);
        GameObject boss= OP.instance.Get("Boss1");
        boss.GetComponent<BossBehavior>().enabled = false;
        boss.SetActive(false);
        Local_camera.cm.enabled = false;
        Vector3 rem = Local_camera.cm.transform.position;

        Sequence sq1 = DOTween.Sequence();
        Tween t1 = Local_camera.cme.transform.DOMove(Local_camera.cm.disvec + boss.transform.position, 1.5f).SetAutoKill(true).Pause();
        sq1.Join(t1).Pause();
        //摄像机去

        Sequence sq2 = DOTween.Sequence();
        Tween t2 = Local_camera.cme.transform.DOMove(rem, 1f).SetAutoKill(true).Pause();
        sq2.Join(t2).Pause();
        //摄像机回

        GameObject i1 = OP.instance.Get("warning");
        i1.transform.position = new Vector3(boss.transform.position.x, 208.1429f, boss.transform.position.z);
        i1.SetActive(false);
        Sequence sq3 = DOTween.Sequence();
        Tween t3 = i1.transform.DOScale(new Vector3(3f, 3f, 3f), 2f).SetAutoKill(true).Pause();
        var cmp = i1.GetComponent<Image>();
        Tween t4 = DOTween.To(
            ()=>cmp.color,
            x=>cmp.color=x,
            new Color(cmp.color.r,cmp.color.g,cmp.color.b,0f),
            1.5f
            ).SetAutoKill(true).Pause();
        sq3.Join(t3).Pause();
        sq3.Join(t4).Pause();
        //图片

        GameObject i2 = OP.instance.Get("Display_pos");
        i2.transform.position=new Vector3(boss.transform.position.x,208.2f,boss.transform.position.z);
        i2.SetActive(false);
        i2.transform.localScale = new Vector3(2f, 2f, 2f);
        Sequence sq4 = DOTween.Sequence();
        Tween t5 = i2.transform.DOScale(new Vector3(0f, 0f, 0f), 0.8f).SetAutoKill(true).Pause();
        sq4.Join(t5).Pause();
        //显示地点 



        sq1.OnComplete(() =>
        {
            i1.SetActive(true);
            sq3.Play();
        });

        sq3.OnComplete(() =>
        {
            OP.instance.Del(i1);
            i2.SetActive(true);


            sq4.Play();
        });

        sq4.OnComplete(() =>
        {
            OP.instance.Del(i2);

            boss.SetActive(true);


            boss.transform
                .DOMoveY(50, 0.3f)
                .From(true)
                .SetAutoKill(true).OnComplete(() =>
                {
                    sq2.Play();
                }
            );
        });

        sq2.OnComplete(() =>
        {

            boss.GetComponent<BossBehavior>().enabled = true;
            boss_b.SetActive(true);
            Inboss = false;
            Local_camera.cm.enabled = true;
        });

        sq1.Play();

        yield break;
    }
}
