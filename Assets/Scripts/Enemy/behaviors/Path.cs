using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϊ����չ��������
/// 
/// 
/// 
/// </summary>
namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class Path:MonoBehaviour
    {

        public Vector3[] mypath;
        //·��

        public Vector3 Get_i_pos(int i)
        {
            return mypath[i];
        }
    }

    [System.Serializable]
    public class SharedPath : SharedVariable<SharedPath>
    {
        public static implicit operator SharedPath(Path value)
        {
            return new SharedPath { Value = value };
        }
    }
}

