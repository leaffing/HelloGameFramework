using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.DataTable;

public class Demo2_ProcedureGame : ProcedureBase
{
    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        Debug.Log("进入游戏流程，可以在这里处理游戏逻辑。");
        // 订阅加载成功事件
        GameEntry.Event.Subscribe(UnityGameFramework.Runtime.LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        GameEntry.Event.Subscribe(UnityGameFramework.Runtime.LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
        GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
        // 加载配置表
        GameEntry.DataTable.LoadDataTable<DRHero>("DRHero", "Assets/Learning/Demo2/heros.txt", GameFramework.LoadType.Text);
        GameEntry.Localization.LoadDictionary("Default", "Assets/Learning/Demo2/Default.xml", GameFramework.LoadType.Text);

        //测试绘制线和mesh
        //List<Vector3> points = new List<Vector3>();
        //for (int i = 0; i < 5; i++)
        //{
        //    points.Add(new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), 0));
        //}
        //DrawMeshUtil.DrawLine(Color.red, points.ToArray(), Resources.Load<Material>("drawMat"), 0.2f, Camera.main.transform);
        //DrawMeshUtil.DrawPolygon(Color.blue, points, Resources.Load<Material>("drawMat"), Camera.main.transform);
    }

    private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
    {
        Log.Error(GameEntry.Localization.HasRawString("Default") + GameEntry.Localization.GetString("Game.Name"));
    }

    private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
    {
        // 数据表加载成功事件
        UnityGameFramework.Runtime.LoadDataTableSuccessEventArgs ne = e as UnityGameFramework.Runtime.LoadDataTableSuccessEventArgs;
        Debug.Log(string.Format("Load data table '{0}' success.", ne.DataTableName));
        
        // 获取框架数据表组件
        // 获得数据表
        IDataTable<DRHero> dtScene = GameEntry.DataTable.GetDataTable<DRHero>();

        // 获得所有行
        DRHero[] drHeros = dtScene.GetAllDataRows();
        Debug.Log("drHeros:" + drHeros.Length);
        // 根据行号获得某一行,即IDataRow中的id，自定义，而不是根据顺序
        DRHero drScene = dtScene.GetDataRow(1); //或直接使用 dtScene[1]
        if (drScene != null)
        {
            // 此行存在，可以获取内容了
            string name = drScene.Name;
            int HP = drScene.HP;
            Debug.Log("name:" + name + ", HP:" + HP);
        }
        else
        {
            // 此行不存在
        }
        // 获得满足条件的所有行
        DRHero[] drScenesWithCondition = dtScene.GetDataRows(x =>x.ID > 1);

        // 获得满足条件的第一行
        DRHero drSceneWithCondition = dtScene.GetDataRow(x => x.Name == "enemy");
        if (drSceneWithCondition != null)
        {
            // 此行存在，可以获取内容了
            string name = drSceneWithCondition.Name;
            int HP = drSceneWithCondition.HP;
            Debug.Log("name:" + name + ", HP:" + HP);
        }
    }

    private void OnLoadDataTableFailure(object sender, GameEventArgs e)
    {
        // 数据表加载失败事件
        UnityGameFramework.Runtime.LoadDataTableFailureEventArgs ne = e as UnityGameFramework.Runtime.LoadDataTableFailureEventArgs;
        Debug.Log(string.Format("Load data table '{0}' failure.", ne.DataTableName) + ne.ErrorMessage);
    }
}