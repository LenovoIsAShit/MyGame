using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb_testFactory:factory
{
    public static bomb_testFactory bomb_fac;

    private void Awake()
    {
        bomb_fac = gameObject.GetComponent<bomb_testFactory>();
    }

    public override GameObject Create()
    {
        GameObject go = new GameObject();
        //�����
        
        go.AddComponent<bomb_test>();
        var cmp = go.GetComponent<bomb_test>();

        cmp.Init( 
            move.player.GetComponent<Attack_dir>().Get_dir(),
            3,
            "prefabs/Bomb/star_bomb/star_bomb",
            "images/beibao/wuqikuang/1",
            "fire_bomb",
            10
        );//�ӵ�Ԥ��


        return go;
    }

}
