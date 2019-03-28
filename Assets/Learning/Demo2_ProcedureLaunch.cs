using GameFramework.Event;
using GameFramework.Procedure;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

public class Demo2_ProcedureLaunch : ProcedureBase
{
    private Dictionary<string, bool> loadedFlag = new Dictionary<string, bool>();

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);

        Debug.Log("初始！！");
        loadedFlag.Clear();

        loadedFlag.Add(string.Format("Dictionary.{0}", "Default"), false);
        GameEntry.Localization.LoadDictionary("Default", "Assets/Learning/Default.xml", GameFramework.LoadType.Text, this);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        IEnumerator<bool> iter = loadedFlag.Values.GetEnumerator();
        while (iter.MoveNext())
        {
            if (!iter.Current)
            {
                return;
            }
        }
        GameEntry.Scene.LoadScene("Assets/Learning/Demo2_Menu.unity", this);
        // 切换流程
        ChangeState<Demo2_ProcedureMenu>(procedureOwner);
    }

    private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
    {
        LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }
        loadedFlag[string.Format("Dictionary.{0}", ne.DictionaryName)] = true;
        Debug.Log(string.Format("Load dictionary '{0}' OK.", ne.DictionaryName));
    }
}