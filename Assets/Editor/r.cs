using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LitJson;

public class r : Editor
{
    [MenuItem("CONTEXT/move/sbss")]
    public static bool sb()
    {
        if (Input.GetKey(KeyCode.J)) return true;
        return false;
    }

    [MenuItem("sb/r2", false)]
    public static void sb2()
    {
        string s = @"
        {
            ""name"":""ÎÒ²ÝÄàÂí"",
            ""age"": 123
        }";

        JsonData jd = JsonMapper.ToObject(s);
        Debug.Log((string)jd["name"]);
    }
}
