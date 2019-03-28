using GameFramework.ObjectPool;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class Demo2_UIMenu : UIFormLogic
{
    public GameObject prefab;

    private IObjectPool<TestObject> testPool;
    private Button button;
    private Text text;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        button = GetComponentInChildren<Button>();
        text = GetComponentInChildren<Text>();
        RectTransform buttonRT = button.GetComponent<RectTransform>();
        RectTransform textRT = text.GetComponent<RectTransform>();
        buttonRT.SetPositionX(0);
        buttonRT.SetPositionY(Screen.height - 0);
        buttonRT.sizeDelta = new Vector2(Screen.width / 2, Screen.height / 2);
        textRT.anchoredPosition = buttonRT.anchoredPosition + new Vector2(buttonRT.sizeDelta.x / 2, -buttonRT.sizeDelta.y / 2);
        textRT.sizeDelta = new Vector2(Screen.width / 4, Screen.height / 4);
        text.fontSize = 20;
        button.onClick.AddListener(OnStarButtonClick);

        testPool = GameEntry.ObjectPool.CreateMultiSpawnObjectPool<TestObject>();
        TestObject obj = new TestObject(prefab, "poolOBJ");
        testPool.Register(obj, false);
    }

    public void OnStarButtonClick()
    {
        // 卸载所有场景
        string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
        for (int i = 0; i < loadedSceneAssetNames.Length; i++)
        {
            GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
        }
        // 加载游戏场景
        GameEntry.Scene.LoadScene("Assets/Learning/Demo2_Game.unity", this);
        GameEntry.Event.Fire(this, new GameBeganEventArgs());
    }
}


public class TestObject : ObjectBase
{
    public TestObject(object target, string name = "") : base(name, target)
    {
    }

    protected override void Release(bool isShutdown)
    {
    }
}
