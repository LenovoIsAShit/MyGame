using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public static UImanager uimanager;
    public enum state
    {
        quitUI,
        Beibao,
        offUI
    }

    [HideInInspector]
    public static state now;

    private void Awake()
    {
        now = state.offUI;
        uimanager = GetComponent<UImanager>();
    }

    private void Update()
    {
        Check_state();
    }

    void Check_state()
    {
        Escape_check();

        Beibao_check();
    }

    void Escape_check()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && now == state.offUI)
        {
            quit_to_mainpage.quitUI.Show();
            //��ʾ�˳�ȷ�ϴ��� 
            now = state.quitUI;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && now == state.quitUI)
        {
            quit_to_mainpage.quitUI.Cancel();
            //ȡ���ر�
            now = state.offUI;
        }
    }


    void Beibao_check()
    {
        if (Input.GetKeyDown(KeyCode.I) && now == state.offUI)
        {
            Beibao.beibao.Show();
            //��ʾ��������
            now = state.Beibao;
        }else if(Input.GetKeyDown(KeyCode.I) && now == state.Beibao)
        {
            Beibao.beibao.Cancel();
            //�رձ�������
            now = state.offUI;
        }
    }
}
