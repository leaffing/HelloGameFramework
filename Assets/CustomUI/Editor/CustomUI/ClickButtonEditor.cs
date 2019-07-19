using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
namespace Custom.UI
{
    [CustomEditor(typeof(ClickButton), true)]
    [CanEditMultipleObjects]
    public class ClickButtonEditor : SelectableUIEditor
    {
        SerializedProperty m_OnClickProperty;
        private ClickButton _target { get { return target as ClickButton; } }
        protected override void OnEnable()
        {
            base.OnEnable();

            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            _target.navigation =new NavigationUI(){ mode = (NavigationUI.Mode)EditorGUILayout.EnumPopup("Navigation:", _target.navigation.mode)};
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_OnClickProperty);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ButtonName"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
