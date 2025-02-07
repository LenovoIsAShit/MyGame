using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 用来显示敌人生成位置
/// 
/// 
/// </summary>
public class Display_pos : MonoBehaviour
{
    static float time;

    private void Awake()
    {

        
    }
    private void OnEnable()
    {
        
    }

    public void play()
    {
        var rt = GetComponent<RectTransform>();
        rt.localScale = new Vector3(1f, 1f, 1f);

        Start_animation();
    }

    void Start_animation()
    {
        var rt = GetComponent<RectTransform>();
        DOTween.To(
            () => rt.localScale,
            x => rt.localScale = x,
            new Vector3(0f,0f,0f),
            time
        ).OnComplete(DelMyself);

    }

    void DelMyself()
    {
        OP.instance.Del(this.gameObject);
    }

    public static void Set_time(float time)
    {
        Display_pos.time = time;
    }

    
}
