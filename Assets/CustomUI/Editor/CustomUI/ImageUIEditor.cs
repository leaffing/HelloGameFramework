﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.AnimatedValues;
namespace Custom.UI
{
    [CustomEditor(typeof(ImageUI), true)]
    public class ImageUIEditor : GraphicEditor
    {
        private ImageUI image;

        SerializedProperty m_FillMethod;
        SerializedProperty m_FillOrigin;
        SerializedProperty m_FillAmount;
        SerializedProperty m_FillClockwise;
        SerializedProperty m_Type;
        SerializedProperty m_FillCenter;
        SerializedProperty m_Sprite;
        SerializedProperty m_PreserveAspect;
        //GUIContent m_SpriteContent;
        //GUIContent m_SpriteTypeContent;
        //GUIContent m_ClockwiseContent;
        AnimBool m_ShowSlicedOrTiled;
        AnimBool m_ShowSliced;
        AnimBool m_ShowTiled;
        AnimBool m_ShowFilled;
        AnimBool m_ShowType;

        SerializedProperty m_AlphaHitTestMinimumThreshold;

        protected override void OnEnable()
        {
            base.OnEnable();

            image = target as ImageUI;

            //m_SpriteContent = EditorGUIUtility.IconContent("Source Image");
            //m_SpriteTypeContent = EditorGUIUtility.IconContent("Image Type");
            //m_ClockwiseContent = EditorGUIUtility.IconContent("Clockwise");

            m_Sprite = serializedObject.FindProperty("m_Sprite");
            m_Type = serializedObject.FindProperty("m_Type");
            m_FillCenter = serializedObject.FindProperty("m_FillCenter");
            m_FillMethod = serializedObject.FindProperty("m_FillMethod");
            m_FillOrigin = serializedObject.FindProperty("m_FillOrigin");
            m_FillClockwise = serializedObject.FindProperty("m_FillClockwise");
            m_FillAmount = serializedObject.FindProperty("m_FillAmount");
            m_PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");

            m_ShowType = new AnimBool(m_Sprite.objectReferenceValue != null);
            m_ShowType.valueChanged.AddListener(Repaint);

            var typeEnum = (ImageUI.Type)m_Type.enumValueIndex;

            m_ShowSlicedOrTiled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == ImageUI.Type.Sliced);
            m_ShowSliced = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == ImageUI.Type.Sliced);
            m_ShowTiled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == ImageUI.Type.Tiled);
            m_ShowFilled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == ImageUI.Type.Filled);
            m_ShowSlicedOrTiled.valueChanged.AddListener(Repaint);
            m_ShowSliced.valueChanged.AddListener(Repaint);
            m_ShowTiled.valueChanged.AddListener(Repaint);
            m_ShowFilled.valueChanged.AddListener(Repaint);

            m_AlphaHitTestMinimumThreshold = serializedObject.FindProperty("m_AlphaHitTestMinimumThreshold");

            SetShowNativeSize(true);
        }

        protected override void OnDisable()
        {
            m_ShowType.valueChanged.RemoveListener(Repaint);
            m_ShowSlicedOrTiled.valueChanged.RemoveListener(Repaint);
            m_ShowSliced.valueChanged.RemoveListener(Repaint);
            m_ShowTiled.valueChanged.RemoveListener(Repaint);
            m_ShowFilled.valueChanged.RemoveListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SpriteGUI();
            AppearanceControlsGUI();
            RaycastControlsGUI();

            m_ShowType.target = m_Sprite.objectReferenceValue != null;
            if (EditorGUILayout.BeginFadeGroup(m_ShowType.faded))
                TypeGUI();
            EditorGUILayout.EndFadeGroup();

            SetShowNativeSize(false);
            if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_PreserveAspect);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("CustomAnchor"))
            {
                image.CustomAnchor();
            }
            if (GUILayout.Button("CustomAllAnchor"))
            {
                image.CustomAllAnchor();
            }
            EditorGUILayout.PropertyField(m_AlphaHitTestMinimumThreshold);
            EditorGUILayout.EndFadeGroup();
            NativeSizeButtonGUI();
            serializedObject.ApplyModifiedProperties();
        }

        void SetShowNativeSize(bool instant)
        {
            ImageUI.Type type = (ImageUI.Type)m_Type.enumValueIndex;
            bool showNativeSize = (type == ImageUI.Type.Simple || type == ImageUI.Type.Filled) && m_Sprite.objectReferenceValue != null;
            base.SetShowNativeSize(showNativeSize, instant);
        }

        /// <summary>
        /// Draw the atlas and Image selection fields.
        /// </summary>

        protected void SpriteGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_Sprite);
            if (EditorGUI.EndChangeCheck())
            {
                var newSprite = m_Sprite.objectReferenceValue as Sprite;
                if (newSprite)
                {
                    ImageUI.Type oldType = (ImageUI.Type)m_Type.enumValueIndex;
                    if (newSprite.border.SqrMagnitude() > 0)
                    {
                        m_Type.enumValueIndex = (int)ImageUI.Type.Sliced;
                    }
                    else if (oldType == ImageUI.Type.Sliced)
                    {
                        m_Type.enumValueIndex = (int)ImageUI.Type.Simple;
                    }
                }
            }
        }

        /// <summary>
        /// Sprites's custom properties based on the type.
        /// </summary>

        protected void TypeGUI()
        {
            EditorGUILayout.PropertyField(m_Type);

            ++EditorGUI.indentLevel;
            {
                ImageUI.Type typeEnum = (ImageUI.Type)m_Type.enumValueIndex;

                bool showSlicedOrTiled = (!m_Type.hasMultipleDifferentValues && (typeEnum == ImageUI.Type.Sliced || typeEnum == ImageUI.Type.Tiled));
                if (showSlicedOrTiled && targets.Length > 1)
                    showSlicedOrTiled = targets.Select(obj => obj as ImageUI).All(img => img.hasBorder);

                m_ShowSlicedOrTiled.target = showSlicedOrTiled;
                m_ShowSliced.target = (showSlicedOrTiled && !m_Type.hasMultipleDifferentValues && typeEnum == ImageUI.Type.Sliced);
                m_ShowTiled.target = (showSlicedOrTiled && !m_Type.hasMultipleDifferentValues && typeEnum == ImageUI.Type.Tiled);
                m_ShowFilled.target = (!m_Type.hasMultipleDifferentValues && typeEnum == ImageUI.Type.Filled);

                ImageUI image = target as ImageUI;
                if (EditorGUILayout.BeginFadeGroup(m_ShowSlicedOrTiled.faded))
                {
                    if (image.hasBorder)
                        EditorGUILayout.PropertyField(m_FillCenter);
                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowSliced.faded))
                {
                    if (image.sprite != null && !image.hasBorder)
                        EditorGUILayout.HelpBox("This Image doesn't have a border.", MessageType.Warning);
                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowTiled.faded))
                {
                    if (image.sprite != null && !image.hasBorder && (image.sprite.texture.wrapMode != TextureWrapMode.Repeat || image.sprite.packed))
                        EditorGUILayout.HelpBox("It looks like you want to tile a sprite with no border. It would be more efficient to modify the Sprite properties, clear the Packing tag and set the Wrap mode to Repeat.", MessageType.Warning);
                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowFilled.faded))
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(m_FillMethod);
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_FillOrigin.intValue = 0;
                    }
                    switch ((ImageUI.FillMethod)m_FillMethod.enumValueIndex)
                    {
                        case ImageUI.FillMethod.Horizontal:
                            m_FillOrigin.intValue = (int)(ImageUI.OriginHorizontal)EditorGUILayout.EnumPopup("Fill Origin", (ImageUI.OriginHorizontal)m_FillOrigin.intValue);
                            break;
                        case ImageUI.FillMethod.Vertical:
                            m_FillOrigin.intValue = (int)(ImageUI.OriginVertical)EditorGUILayout.EnumPopup("Fill Origin", (ImageUI.OriginVertical)m_FillOrigin.intValue);
                            break;
                        case ImageUI.FillMethod.Radial90:
                            m_FillOrigin.intValue = (int)(ImageUI.Origin90)EditorGUILayout.EnumPopup("Fill Origin", (ImageUI.Origin90)m_FillOrigin.intValue);
                            break;
                        case ImageUI.FillMethod.Radial180:
                            m_FillOrigin.intValue = (int)(ImageUI.Origin180)EditorGUILayout.EnumPopup("Fill Origin", (ImageUI.Origin180)m_FillOrigin.intValue);
                            break;
                        case ImageUI.FillMethod.Radial360:
                            m_FillOrigin.intValue = (int)(ImageUI.Origin360)EditorGUILayout.EnumPopup("Fill Origin", (ImageUI.Origin360)m_FillOrigin.intValue);
                            break;
                    }
                    EditorGUILayout.PropertyField(m_FillAmount);
                    if ((ImageUI.FillMethod)m_FillMethod.enumValueIndex > ImageUI.FillMethod.Vertical)
                    {
                        EditorGUILayout.PropertyField(m_FillClockwise);
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }
            --EditorGUI.indentLevel;
        }

        /// <summary>
        /// All graphics have a preview.
        /// </summary>

        public override bool HasPreviewGUI() { return true; }

        /// <summary>
        /// Draw the Image preview.
        /// </summary>

        public override void OnPreviewGUI(Rect rect, GUIStyle background)
        {
            ImageUI image = target as ImageUI;
            if (image == null) return;

            Sprite sf = image.sprite;
            if (sf == null) return;

            SpriteDrawUtility.DrawSprite(sf, rect, image.canvasRenderer.GetColor());
        }

        /// <summary>
        /// Info String drawn at the bottom of the Preview
        /// </summary>

        public override string GetInfoString()
        {
            ImageUI image = target as ImageUI;
            Sprite sprite = image.sprite;

            int x = (sprite != null) ? Mathf.RoundToInt(sprite.rect.width) : 0;
            int y = (sprite != null) ? Mathf.RoundToInt(sprite.rect.height) : 0;

            return string.Format("Image Size: {0}x{1}", x, y);
        }
    }
}

