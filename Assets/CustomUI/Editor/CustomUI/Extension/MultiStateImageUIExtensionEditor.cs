using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
namespace Custom.UI
{
    [CustomEditor(typeof(MultiStateImageUIExtension), true)]
    [CanEditMultipleObjects]
    public class MultiStateImageUIExtensionEditor : MultiStateImageUIEditor
    {
        SerializedProperty m_AlternativeColorBlock;
        SerializedProperty m_AlternativeSpriteState;
        SerializedProperty m_AlternativeAnimTrigger;

        SerializedProperty m_CurState;
        protected override void OnEnable()
        {
            base.OnEnable();

            m_CurState = serializedObject.FindProperty("m_CurState");

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

                var animator = (target as MultiStateImageUIExtension).GetComponent<Animator>();
                if (EditorGUILayout.BeginFadeGroup(m_ShowAnimTransition.faded))
                {
                    EditorGUILayout.PropertyField(m_AlternativeAnimTrigger);

                    if (animator == null || animator.runtimeAnimatorController == null)
                    {
                        Rect buttonRect = EditorGUILayout.GetControlRect();
                        buttonRect.xMin += EditorGUIUtility.labelWidth;
                        if (GUI.Button(buttonRect, "Auto Generate Animation", EditorStyles.miniButton))
                        {
                            var controller = GenerateSelectableAnimatorContoller((target as MultiStateImageUIExtension).animationTriggers, target as MultiStateImageUIExtension);
                            if (controller != null)
                            {
                                if (animator == null)
                                    animator = (target as MultiStateImageUIExtension).gameObject.AddComponent<Animator>();

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
            serializedObject.ApplyModifiedProperties();
        }
    }
}
