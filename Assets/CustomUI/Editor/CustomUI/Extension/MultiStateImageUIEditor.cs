using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;

namespace Custom.UI
{
    [CustomEditor(typeof(MultiStateImageUI), true)]
    [CanEditMultipleObjects]
    public class MultiStateImageUIEditor : ImageUIEditor
    {
        SerializedProperty m_TargetGraphicProperty;
        SerializedProperty m_TransitionProperty;
        SerializedProperty m_ColorBlockProperty;
        SerializedProperty m_SpriteStateProperty;
        SerializedProperty m_AnimTriggerProperty;

        protected AnimBool m_ShowColorTint = new AnimBool();
        protected AnimBool m_ShowSpriteTrasition = new AnimBool();
        protected AnimBool m_ShowAnimTransition = new AnimBool();
        private string[] m_PropertyPathToExcludeForChildClasses;

        private static List<MultiStateImageUIEditor> s_Editors = new List<MultiStateImageUIEditor>();
        protected override void OnEnable()
        {
            base.OnEnable();
            m_TargetGraphicProperty = serializedObject.FindProperty("m_TargetGraphic");
            m_TransitionProperty = serializedObject.FindProperty("m_Transition");
            m_ColorBlockProperty = serializedObject.FindProperty("m_Colors");
            m_SpriteStateProperty = serializedObject.FindProperty("m_SpriteState");
            m_AnimTriggerProperty = serializedObject.FindProperty("m_AnimationTriggers");

            m_PropertyPathToExcludeForChildClasses = new[]
            {
                m_TransitionProperty.propertyPath,
                m_ColorBlockProperty.propertyPath,
                m_SpriteStateProperty.propertyPath,
                m_AnimTriggerProperty.propertyPath,
                m_TargetGraphicProperty.propertyPath,
            };

            var trans = GetTransition(m_TransitionProperty);
            m_ShowColorTint.value = (trans == Transition.ColorTint);
            m_ShowSpriteTrasition.value = (trans == Transition.SpriteSwap);
            m_ShowAnimTransition.value = (trans == Transition.Animation);

            m_ShowColorTint.valueChanged.AddListener(Repaint);
            m_ShowSpriteTrasition.valueChanged.AddListener(Repaint);

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

            var trans = GetTransition(m_TransitionProperty);

            var graphic = m_TargetGraphicProperty.objectReferenceValue as Graphic;
            if (graphic == null)
                graphic = (target as MultiStateImageUI).GetComponent<Graphic>();

            var animator = (target as MultiStateImageUI).GetComponent<Animator>();
            m_ShowColorTint.target = (!m_TransitionProperty.hasMultipleDifferentValues && trans == Transition.ColorTint);
            m_ShowSpriteTrasition.target = (!m_TransitionProperty.hasMultipleDifferentValues && trans == Transition.SpriteSwap);
            m_ShowAnimTransition.target = (!m_TransitionProperty.hasMultipleDifferentValues && trans == Transition.Animation);

            EditorGUILayout.PropertyField(m_TransitionProperty);
            ++EditorGUI.indentLevel;
            {
                if (trans == Transition.ColorTint || trans == Transition.SpriteSwap)
                {
                    EditorGUILayout.PropertyField(m_TargetGraphicProperty);
                }

                switch (trans)
                {
                    case Transition.ColorTint:
                        if (graphic == null)
                            EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.", MessageType.Warning);
                        break;

                    case Transition.SpriteSwap:
                        if (graphic as Image == null)
                            EditorGUILayout.HelpBox("You must have a Image target in order to use a sprite swap transition.", MessageType.Warning);
                        break;
                }

                if (EditorGUILayout.BeginFadeGroup(m_ShowColorTint.faded))
                {
                    EditorGUILayout.PropertyField(m_ColorBlockProperty);
                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowSpriteTrasition.faded))
                {
                    EditorGUILayout.PropertyField(m_SpriteStateProperty);
                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowAnimTransition.faded))
                {
                    EditorGUILayout.PropertyField(m_AnimTriggerProperty);

                    if (animator == null || animator.runtimeAnimatorController == null)
                    {
                        Rect buttonRect = EditorGUILayout.GetControlRect();
                        buttonRect.xMin += EditorGUIUtility.labelWidth;
                        if (GUI.Button(buttonRect, "Auto Generate Animation", EditorStyles.miniButton))
                        {
                            var controller = GenerateSelectableAnimatorContoller((target as MultiStateImageUI).animationTriggers, target as MultiStateImageUI);
                            if (controller != null)
                            {
                                if (animator == null)
                                    animator = (target as MultiStateImageUI).gameObject.AddComponent<Animator>();

                                UnityEditor.Animations.AnimatorController.SetAnimatorController(animator, controller);
                            }
                        }
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }
            --EditorGUI.indentLevel;

            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }

        protected static UnityEditor.Animations.AnimatorController GenerateSelectableAnimatorContoller(AnimationTriggers animationTriggers, MultiStateImageUI target)
        {
            if (target == null)
                return null;

            // Where should we create the controller?
            var path = GetSaveControllerPath(target);
            if (string.IsNullOrEmpty(path))
                return null;

            // figure out clip names
            var normalName = string.IsNullOrEmpty(animationTriggers.normalTrigger) ? "Normal" : animationTriggers.normalTrigger;
            var highlightedName = string.IsNullOrEmpty(animationTriggers.highlightedTrigger) ? "Highlighted" : animationTriggers.highlightedTrigger;
            var pressedName = string.IsNullOrEmpty(animationTriggers.pressedTrigger) ? "Pressed" : animationTriggers.pressedTrigger;
            var disabledName = string.IsNullOrEmpty(animationTriggers.disabledTrigger) ? "Disabled" : animationTriggers.disabledTrigger;

            // Create controller and hook up transitions.
            var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(path);
            GenerateTriggerableTransition(normalName, controller);
            GenerateTriggerableTransition(highlightedName, controller);
            GenerateTriggerableTransition(pressedName, controller);
            GenerateTriggerableTransition(disabledName, controller);

            AssetDatabase.ImportAsset(path);

            return controller;
        }

        private static string GetSaveControllerPath(MultiStateImageUI target)
        {
            var defaultName = target.gameObject.name;
            var message = string.Format("Create a new animator for the game object '{0}':", defaultName);
            return EditorUtility.SaveFilePanelInProject("New Animation Contoller", defaultName, "controller", message);
        }
        private static AnimationClip GenerateTriggerableTransition(string name, UnityEditor.Animations.AnimatorController controller)
        {
            // Create the clip
            var clip = UnityEditor.Animations.AnimatorController.AllocateAnimatorClip(name);
            AssetDatabase.AddObjectToAsset(clip, controller);

            // Create a state in the animatior controller for this clip
            var state = controller.AddMotion(clip);

            // Add a transition property
            controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

            // Add an any state transition
            var stateMachine = controller.layers[0].stateMachine;
            var transition = stateMachine.AddAnyStateTransition(state);
            transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, name);
            return clip;
        }
    }
}
