using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
namespace Custom.UI
{
    [CustomEditor(typeof(DoubleStateButton), true)]
    [CanEditMultipleObjects]
    public class DoubleStateButtonEditor : SelectableUIEditor
    {
        SerializedProperty m_AlternativeColorBlock;
        SerializedProperty m_AlternativeSpriteState;
        SerializedProperty m_AlternativeAnimTrigger;

        SerializedProperty m_CurState;
        SerializedProperty m_OnChangeToFistState;
        SerializedProperty m_OnChangeToSecondState;
        protected override void OnEnable()
        {
            base.OnEnable();

            m_CurState = serializedObject.FindProperty("m_CurState");
            m_OnChangeToFistState = serializedObject.FindProperty("m_OnChangeToFistState");
            m_OnChangeToSecondState = serializedObject.FindProperty("m_OnChangeToSecondState");

            m_AlternativeColorBlock = serializedObject.FindProperty("m_AlternativeColorBlock");
            m_AlternativeSpriteState = serializedObject.FindProperty("m_AlternativeSpriteState");
            m_AlternativeAnimTrigger = serializedObject.FindProperty("m_AlternativeAnimTrigger");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
            GUILayout.Label("The Second State");
            ++EditorGUI.indentLevel;
            {
                if (EditorGUILayout.BeginFadeGroup(m_ShowColorTint.faded))
                {
                    EditorGUILayout.PropertyField(m_AlternativeColorBlock);
                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowSpriteTrasition.faded))
                {
                    EditorGUILayout.PropertyField(m_AlternativeSpriteState);
                }
                EditorGUILayout.EndFadeGroup();

                var animator = (target as SelectableUI).GetComponent<Animator>();
                if (EditorGUILayout.BeginFadeGroup(m_ShowAnimTransition.faded))
                {
                    EditorGUILayout.PropertyField(m_AlternativeAnimTrigger);

                    if (animator == null || animator.runtimeAnimatorController == null)
                    {
                        Rect buttonRect = EditorGUILayout.GetControlRect();
                        buttonRect.xMin += EditorGUIUtility.labelWidth;
                        if (GUI.Button(buttonRect, "Auto Generate Animation", EditorStyles.miniButton))
                        {
                            var controller = GenerateSelectableAnimatorContoller((target as SelectableUI).animationTriggers, target as SelectableUI);
                            if (controller != null)
                            {
                                if (animator == null)
                                    animator = (target as SelectableUI).gameObject.AddComponent<Animator>();

                                UnityEditor.Animations.AnimatorController.SetAnimatorController(animator, controller);
                            }
                        }
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }
            --EditorGUI.indentLevel;
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_CurState);
            EditorGUILayout.PropertyField(m_OnChangeToFistState);
            EditorGUILayout.PropertyField(m_OnChangeToSecondState);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
