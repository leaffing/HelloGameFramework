using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Leaf.EditorTools
{
    public class UIEditorTools
    {
        [MenuItem("Leaf/UI/CustomAnchor", false, 0)]
        public static void CustomAnchor()
        {
            if (Selection.gameObjects != null)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    SetCustomAnchor(gameObject);
                }
            }
        }

        [MenuItem("Leaf/UI/CustomAllAnchor", false, 1)]
        public static void CustomAllAnchor()
        {
            if (Selection.gameObjects != null)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
                    {
                        SetCustomAnchor(child.gameObject);
                        Debug.Log(child.gameObject.name);
                    }
                }
            }
        }

        /// <summary>
        /// 设置UI物体的锚点为自适应分辨率比例大小
        /// </summary>
        /// <param name="gameObject">UI物体</param>
        private static void SetCustomAnchor(GameObject gameObject)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                //Debug.Log("You choosed a object that is not a UI!");
                EditorUtility.DisplayDialog("Tip", "You choosed a object that is not a UI!", "OK");
            }
            else
            {
                var Parent = rectTransform.parent as RectTransform;
                var ParentSize = new Vector2(1.0f / Parent.rect.size.x, 1.0f / Parent.rect.height);
                rectTransform.anchorMax += Vector2.Scale(rectTransform.offsetMax, ParentSize);
                rectTransform.anchorMin += Vector2.Scale(rectTransform.offsetMin, ParentSize);
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }
        }
    }
}