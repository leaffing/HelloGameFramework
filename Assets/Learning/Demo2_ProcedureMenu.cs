using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

public class Demo2_ProcedureMenu : ProcedureBase
{
    private SceneComponent scene;
    private ProcedureOwner owner;
    private int MenuUI_ID;
    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        owner = procedureOwner;
        Debug.Log("进入菜单流程，可以在这里加载菜单UI。");

        // 加载框架Event组件
        // 加载框架UI组件

        scene = UnityGameFramework.Runtime.GameEntry.GetComponent<SceneComponent>();
        // 订阅UI加载成功事件
        GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
        GameEntry.Event.Subscribe(GameBeganEventArgs.EventId, BeginGame);
        // 加载UI
        MenuUI_ID = GameEntry.UI.OpenUIForm("Assets/Learning/UI_Menu.prefab", "DefaultGroup", this);
    }

    private void BeginGame(object sender, GameEventArgs e)
    {
        Debug.Log("收到开始游戏指令！！！");
        GameEntry.UI.CloseUIForm(MenuUI_ID);
        ChangeState<Demo2_ProcedureGame>(owner);
    }

    private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
    {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
        // 判断userData是否为自己
        if (ne.UserData != this)
        {
            return;
        }
        Debug.Log("UI_Menu：恭喜你，成功地召唤了我。");
    }
}