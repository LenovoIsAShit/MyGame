using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb_testFactory:factory
{
    public override GameObject Create(GameObject bh)
    {
        GameObject go = new GameObject();
        //父组件
        
        go.AddComponent<bomb_test>();
        var cmp = go.GetComponent<bomb_test>();
        cmp.Init(bh.transform.position+new Vector3(0f,0.5f,0f), move.player.GetComponent<Attack_dir>().Get_dir(), 3, "prefabs/Bomb/star_bomb/star_bomb", 10);
        //子弹预设


        return go;
    }
}
