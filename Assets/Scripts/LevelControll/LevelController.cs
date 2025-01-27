using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    List<int> max_monster;//怪物最大数量

    private void Awake()
    {
        levelcontrol = GetComponent<LevelController>();

        player_start_pos = new Vector3(419.2857f, 208.1429f, 0f);
        level = 1;
        max_monster = new List<int>();
        for(int i = 0; i < 10; i++)
        {
            max_monster.Add(i * 5);
        }
    }

    private void Start()
    {
        ShowLevel(level);
    }




    void Start_level(int i)
    {
        EnemyController.Set_max_num(max_monster[i]);

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
        StartCoroutine(StatNextLevel());
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

        level++;

        EnemyController.enemy_Controller.Reset_nownum(0);

        ShowLevel(level);

        yield break;
    }
    //下一关


    public void Check_level()
    {
        if (EnemyController.enemy_Controller.Get_nownum() == 0)
        {
            StartCoroutine(StatNextLevel());
        }
    }
}
