using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class botton_Wq : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowWuqiPage);
    }

    void ShowWuqiPage()
    {
        if (BeibaoManager.Beibao_Manager.GetComponent<BeibaoManager>().Wuqi_cangku == null)
        {
            Debug.Log("我操死你妈");
        }
        else
        {
            StartCoroutine(Reload_wq());

            transform.parent.GetChild(0).gameObject.SetActive(true);
            transform.parent.GetChild(1).gameObject.SetActive(false);
        }
    }

    IEnumerator Reload_wq()
    {
        var list = BeibaoManager.Beibao_Manager.GetComponent<BeibaoManager>().Wuqi_cangku;
        
        foreach(var item in list)
        {
            Get_wuqi_gezi(item.bomb_name);
            yield return null;
        }

        yield break;
    }
    //重新加载武器界面

    GameObject Get_wuqi_gezi(string name,string path="prefabs/Beibao/wuqi_gezi")
    {
        GameObject gz = Instantiate(Resources.Load<GameObject>(path));
        gz.transform.GetChild(0).GetComponent<TMP_Text>().text = name;
        gz.transform.SetParent(transform.parent.GetChild(0).GetChild(1).GetChild(0).GetChild(0));
        var cmp = gz.GetComponent<RectTransform>();
        cmp.offsetMax = new Vector2(0f, 0f);
        cmp.offsetMin = new Vector2(0f, 0f);
        return gz;
    }
}
