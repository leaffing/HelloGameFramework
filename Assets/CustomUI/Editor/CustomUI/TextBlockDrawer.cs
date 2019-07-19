using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Custom.UI
{
    [CustomPropertyDrawer(typeof(TextBlock), true)]
    public class TextBlockDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = rect;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty normalText = prop.FindPropertyRelative("m_NormalText");
            SerializedProperty highlightedText = prop.FindPropertyRelative("m_HighlightedText");
            SerializedProperty pressedText = prop.FindPropertyRelative("m_PressedText");
            SerializedProperty disabledText = prop.FindPropertyRelative("m_DisabledText");

            EditorGUI.PropertyField(drawRect, normalText);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, highlightedText);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, pressedText);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, disabledText);
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
