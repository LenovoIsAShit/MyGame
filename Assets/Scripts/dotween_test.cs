using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class dotween_test : MonoBehaviour
{
    
    void Start()
    {
        test();
    }
    
    void test()
    {  
        Tweener t = transform.DODynamicLookAt(move.player.transform.position, 1f, AxisConstraint.None, Vector3.up);
        t.SetDelay(2f);
        t.SetLoops(3);
        t.SetEase(Ease.OutQuad);
        t.SetAutoKill(true);
        t.OnKill(e);

        t.Play();

    }

    static void e()
    {
        Debug.Log("¹þ¹þ");
    }
}
