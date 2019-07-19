using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;

namespace Custom.UI
{
    [CustomEditor(typeof(MultiStateTextUI), true)]
    [CanEditMultipleObjects]
    public class MultiStateTextUIEditor : TextUIEditor
    {
        SerializedProperty m_TargetGraphicProperty;
        SerializedProperty m_ColorBlockProperty;
        SerializedProperty m_TextBlockProperty;
        private string[] m_PropertyPathToExcludeForChildClasses;

        private static List<MultiStateTextUIEditor> s_Editors = new List<MultiStateTextUIEditor>();
        protected override void OnEnable()
        {
            base.OnEnable();
            m_TargetGraphicProperty = serializedObject.FindProperty("m_TargetGraphic");
            m_ColorBlockProperty = serializedObject.FindProperty("m_Colors");
            m_TextBlockProperty = serializedObject.FindProperty("m_Texts");
            m_PropertyPathToExcludeForChildClasses = new[]
            {
                m_ColorBlockProperty.propertyPath,
                m_TextBlockProperty.propertyPath,
                m_TargetGraphicProperty.propertyPath,
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
            EditorGUILayout.Separator();

            var graphic = m_TargetGraphicProperty.objectReferenceValue as Graphic;
            if (graphic == null)
                graphic = (target as MultiStateTextUI).GetComponent<Graphic>();
            ++EditorGUI.indentLevel;
            EditorGUILayout.PropertyField(m_ColorBlockProperty);
            EditorGUILayout.PropertyField(m_TextBlockProperty);
            --EditorGUI.indentLevel;
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
