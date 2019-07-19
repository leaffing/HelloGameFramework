using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/Create ExampleAsset Instance")]
public class MySetting : ScriptableObject
{
    [SerializeField]
    public string str;

    [SerializeField, Range (0, 10)]
    public int number;

    [MenuItem ("Example/Create ExampleAsset")]
    static void CreateExampleAsset ()
    {
        var exampleAsset = CreateInstance<MySetting> ();

        AssetDatabase.CreateAsset (exampleAsset, "Assets/EditorLearning/ExampleAsset.asset");
        AssetDatabase.Refresh ();
    }
}
