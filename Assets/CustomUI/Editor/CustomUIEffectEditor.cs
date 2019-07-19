using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Leaf.CustomUI
{
    [CustomEditor(typeof(CustomUIEffect))]
    public class CustomUIEffectEditor : Editor
    {

        void OnEable()
        {

        }

       public override void OnInspectorGUI()
       {
           serializedObject.Update();
           GUILayout.Label("自定义动画",new GUIStyle(){alignment = TextAnchor.MiddleCenter, normal = new GUIStyleState(){textColor = Color.white}});
           PropertyField("effectType");
           PropertyField("startTime");
           PropertyField("duration");
           PropertyField("effectEase");
           PropertyTargetValueField();

           serializedObject.ApplyModifiedProperties();
       }

        private void PropertyTargetValueField()
        {
            var effectType =  serializedObject.FindProperty("effectType");
            switch (effectType.intValue)
            {
                case 3:
                case 5:
                    PropertyField("targetFloatValue");
                    break;
                case 4:
                    PropertyField("targetIntValue");
                    break;
                default:
                    PropertyField("targetVectorValue");
                    break;
            }
        }

        private void PropertyField(string fieldName)
       {
           var spShadowMode = serializedObject.FindProperty(fieldName);
           EditorGUILayout.PropertyField(spShadowMode);
       }

    }
}