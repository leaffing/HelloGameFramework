#if UNITY_2018_3_OR_NEWER
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

static class MyProjectSetting
{

    private static MySetting mySetting;
    private static SerializedObject mS_Setting;

    private static SerializedProperty m_Str;
    private static SerializedProperty m_Num;

    [SettingsProvider]
    public static SettingsProvider MySetting()
    {
        return new SettingsProvider("Project/MySetting", SettingsScope.Project)
        {
            activateHandler = (s, element) =>
            {
                mySetting = MyFramework.GetOrCreateSetting(); 
                mS_Setting = new SerializedObject(mySetting);
                if (mS_Setting != null)
                {
                    m_Str = mS_Setting.FindProperty("str");
                    m_Num = mS_Setting.FindProperty("number");
                }
            },
            guiHandler = (s) =>
            {
                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(500));
                EditorGUILayout.PropertyField(m_Str, new GUIContent("字符串测试"));
                EditorGUILayout.PropertyField(m_Num, new GUIContent("数字测试"));
                EditorGUILayout.EndVertical();
                mS_Setting.ApplyModifiedProperties();
            },
            deactivateHandler = () => {},
        };
    }
}
#endif