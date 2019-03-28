using UnityEngine;

public class Demo2_Controller : MonoBehaviour {

    public void EnterGame()
    {
        // 卸载所有场景
        string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
        for (int i = 0; i < loadedSceneAssetNames.Length; i++)
        {
            GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
        }
        // 加载游戏场景
        GameEntry.Scene.LoadScene("Assets/Learning/Demo2_Game.unity", this);
    }
}
