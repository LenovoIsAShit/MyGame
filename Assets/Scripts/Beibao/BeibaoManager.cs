using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理背包的一切动作
/// </summary>

public class BeibaoManager : MonoBehaviour
{
    public static GameObject beibaoManager;
    public static GameObject Beibao_Manager
    {
        get { return beibaoManager; }
    }
    //背包单例

    bomb wuqi;

    List<bomb> wuqi_cangku;
    List<GameObject> bc;//对象

    public List<bomb> Wuqi_cangku
    {
        get { return wuqi_cangku; }
    }

    /// <summary>
    /// 添加武器
    /// </summary>
    /// <param name="bomb_name"></param>
    void Add_wuqi_cangku(string bomb_name)
    {
        bomb b = null;
        switch (bomb_name)
        {
            case "fire_bomb":
                b = OP.Instance.Get("fire_bomb").GetComponent<bomb_test>();
                b.Set_check_state(false);
                b.stop = true;
                break;
        }
        wuqi_cangku.Add(b);
    }


    /// <summary>
    /// 设置主武器
    /// </summary>
    /// <param name="now"></param>
    void Set_main_wuqi(bomb now)
    {
        wuqi = now;
        //设置主子弹

        GameObject beibao = Beibao.beibao.gameObject;
        Image image = beibao.transform.GetChild(1).GetChild(0).GetComponent<Image>();

        image.sprite = ABLoadManager.instance.GetImage("image", now.beibao_image);
    }


    void Awake()
    {
        wuqi_cangku = new List<bomb>();
        beibaoManager = this.gameObject;
    }
    void Start()
    {
        Add_wuqi_cangku("fire_bomb");
        Set_main_wuqi(wuqi_cangku[0]);
    }
}
