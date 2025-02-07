using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class botton_Wq : MonoBehaviour
{
    List<GameObject> wuqi_gezi;//武器格子
    List<GameObject> wuping_wezi;//武器格子

    public enum state
    {
        InDel,
        InLoad,
        idle
    }

    public state now;

    void Start()
    {
        now = state.idle;
        GetComponent<Button>().onClick.AddListener(ShowWuqiPage);

        wuqi_gezi = new List<GameObject>();
        wuping_wezi = new List<GameObject>();
    }

    void ShowWuqiPage()
    {
        StartCoroutine(Del_all_gezi());

        StartCoroutine(Reload_wq());

        transform.parent.GetChild(0).gameObject.SetActive(true);
        transform.parent.GetChild(1).gameObject.SetActive(false);
    }

    IEnumerator Reload_wq()
    {
        while (now != state.InLoad)
        {
            yield return null;
        }

        var list = BeibaoManager.Beibao_Manager.GetComponent<BeibaoManager>().Wuqi_cangku;

        int i = 0;
        foreach(var item in list)
        {
            wuqi_gezi.Add(Get_wuqi_gezi(item.bomb_name));
            SetPos(i, wuqi_gezi[i]);
            i++;
            yield return null;
        }

        now = state.idle;

        yield break;
    }
    //重新加载武器界面

    GameObject Get_wuqi_gezi(string name,string path="prefabs/Beibao/wuqi_gezi")
    {
        GameObject gz = OP.instance.Get("wuqi_gezi");
       
        gz.transform.GetChild(0).GetComponent<TMP_Text>().text = name;
        gz.transform.SetParent(transform.parent.GetChild(0).GetChild(1).GetChild(0).GetChild(0));
        var cmp = gz.GetComponent<RectTransform>();
        cmp.offsetMax = new Vector2(0f, 0f);
        cmp.offsetMin = new Vector2(0f, 0f);
        return gz;
    }


    IEnumerator Del_all_gezi()
    {
        while (now != state.idle)
        {
            yield return null;
        }

        now = state.InDel;

        for (int i = 0; i < wuqi_gezi.Count; i++)
        {
            OP.instance.Del(wuqi_gezi[i]);
            wuqi_gezi[i] = null;
            yield return null;
        }

        wuqi_gezi.Clear();

        now = state.InLoad;

        yield break;
    }
    //回收所有武器格子对象

    void SetPos(int i,GameObject obj)
    {

    }
    //设置第i个格子的位置
}
