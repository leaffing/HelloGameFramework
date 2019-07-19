using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyFramework : MonoBehaviour
{
    public static MySetting setting;


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DebugString();
        }
    }


    public static MySetting GetOrCreateSetting()
    {
        if(!setting)
            setting = new MySetting();
        return setting;
    }

    public static void DebugString()
    {
        if(setting)
            Debug.Log(setting.str + "--" + setting.number);
    }
}
