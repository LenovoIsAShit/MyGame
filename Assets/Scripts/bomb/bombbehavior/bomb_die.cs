using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ӵ���������ϵͳ����ֹͣ�ű�
/// 
/// 
/// 
/// </summary>


public class bomb_die : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.MainModule psm;


    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        psm = ps.main;
        psm.stopAction= ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        OP.instance.Del(this.gameObject);
    }
}
