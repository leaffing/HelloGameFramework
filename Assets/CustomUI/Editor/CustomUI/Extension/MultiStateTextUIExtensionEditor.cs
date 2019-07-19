using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;

namespace Custom.UI
{
    [CustomEditor(typeof(MultiStateTextUIExtension), true)]
    [CanEditMultipleObjects]
    public class MultiStateTextUIExtensionEditor : MultiStateTextUIEditor
    {
        SerializedProperty m_AlternativeColorBlockProperty;
        SerializedProperty m_AlternativeTextBlockProperty;
        private string[] m_PropertyPathToExcludeForChildClasses;

        private static List<MultiStateTextUIEditor> s_Editors = new List<MultiStateTextUIEditor>();
        protected override void OnEnable()
        {
            base.OnEnable();
            m_AlternativeColorBlockProperty = serializedObject.FindProperty("m_AlternativeColorBlock");
            m_AlternativeTextBlockProperty = serializedObject.FindProperty("m_AlternativeTexts");
            m_PropertyPathToExcludeForChildClasses = new[]
            {
                m_AlternativeColorBlockProperty.propertyPath,
                m_AlternativeTextBlockProperty.propertyPath,
            };

            s_Editors.Add(this);
        }
        static Transition GetTransition(SerializedProperty transition)
        {
            return (Transition)transition.enumValueIndex;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.Space();
            GUILayout.Label("The Second State");
            ++EditorGUI.indentLevel;
            EditorGUILayout.PropertyField(m_AlternativeColorBlockProperty);
            EditorGUILayout.PropertyField(m_AlternativeTextBlockProperty);
            --EditorGUI.indentLevel;
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
