using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹监听粒子系统播放停止脚本
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
        Destroy(this.gameObject);
    }
}
