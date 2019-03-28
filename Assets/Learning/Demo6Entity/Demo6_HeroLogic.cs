using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 英雄逻辑处理
/// </summary>
public class Demo6_HeroLogic : EntityLogic
{
    protected Demo6_HeroLogic()
    {
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        Log.Debug("显示英雄实体.");
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (Input.GetKey(KeyCode.Space))
        {
            transform.localEulerAngles += new Vector3(0, 10, 0);
        }
    }
}