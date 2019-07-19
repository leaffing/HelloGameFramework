using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
namespace Custom.UI
{
    [CustomEditor(typeof(LongTouchButton), true)]
    [CanEditMultipleObjects]
    public class LongTouchButtonEditor : SelectableUIEditor
    {
        SerializedProperty m_OnLongTouchStart;
        SerializedProperty m_OnLongTouch;
        SerializedProperty m_OnLongTouchEnd;
        protected override void OnEnable()
        {
            base.OnEnable();

            m_OnLongTouchStart = serializedObject.FindProperty("m_OnLongTouchStart");
            m_OnLongTouch = serializedObject.FindProperty("m_OnLongTouch");
            m_OnLongTouchEnd = serializedObject.FindProperty("m_OnLongTouchEnd");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_OnLongTouchStart);
            EditorGUILayout.PropertyField(m_OnLongTouch);
            EditorGUILayout.PropertyField(m_OnLongTouchEnd);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

