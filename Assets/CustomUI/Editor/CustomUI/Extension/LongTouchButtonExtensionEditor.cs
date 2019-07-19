using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
namespace Custom.UI
{
    [CustomEditor(typeof(LongTouchButtonExtension), true)]
    [CanEditMultipleObjects]
    public class LongTouchButtonExtensionEditor : LongTouchButtonEditor
    {
        SerializedProperty m_AssociatedObject;
        protected override void OnEnable()
        {
            base.OnEnable();
            m_AssociatedObject = serializedObject.FindProperty("m_AssociatedObject");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            GUILayout.Label("Link To Other UI");
            EditorGUILayout.PropertyField(m_AssociatedObject, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
