using UnityEngine;
using UnityEngine.UI;
using Custom.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Custom.UI
{
    [AddComponentMenu("CustomUI/MultiStateTextUI", 22)]
    [RequireComponent(typeof(CanvasRenderer))]
    public class MultiStateTextUI : TextUI, IStateTransition
    {
        // Colors used for a color tint-based transition.
        [FormerlySerializedAs("colors")]
        [SerializeField]
        protected ColorBlock m_Colors = ColorBlock.defaultColorBlock;
        // Colors used for a color tint-based transition.
        [FormerlySerializedAs("texts")]
        [SerializeField]
        protected TextBlock m_Texts = TextBlock.defaultTextBlock;


        // Graphic that will be colored.
        [FormerlySerializedAs("highlightGraphic")]
        [FormerlySerializedAs("m_HighlightGraphic")]
        [SerializeField]
        private Graphic m_TargetGraphic;

        public ColorBlock colors { get { return m_Colors; } set { SetPropertyUtility.SetStruct(ref m_Colors, value); } }
        public TextBlock texts { get { return m_Texts; } set { SetPropertyUtility.SetStruct(ref m_Texts, value); } }
        public Graphic targetGraphic { get { return m_TargetGraphic; } set { SetPropertyUtility.SetClass(ref m_TargetGraphic, value); } }
        
        protected override void Awake()
        {
            base.Awake();
            if (m_TargetGraphic == null)
                m_TargetGraphic = GetComponent<Graphic>();
        }
        //protected override void Reset()
        //{
        //    m_TargetGraphic = GetComponent<Graphic>();
        //}
        protected virtual void StartColorTween(Color targetColor, bool instant)
        {
            if (m_TargetGraphic == null)
                return;

            m_TargetGraphic.CrossFadeColor(targetColor, instant ? 0f : m_Colors.fadeDuration, true, true);
        }
        public virtual void DoStateTransition(SelectionState state, bool instant)
        {
            Color tintColor;
            string tintText;
            switch (state)
            {
                case SelectionState.Normal:
                    tintColor = m_Colors.normalColor;
                    tintText = m_Texts.normalText;
                    break;
                case SelectionState.Highlighted:
                    tintColor = m_Colors.highlightedColor;
                    tintText = m_Texts.highlightedText;
                    break;
                case SelectionState.Pressed:
                    tintColor = m_Colors.pressedColor;
                    tintText = m_Texts.pressedText;
                    break;
                case SelectionState.Disabled:
                    tintColor = m_Colors.disabledColor;
                    tintText = m_Texts.disabledText;
                    break;
                default:
                    tintColor = Color.black;
                    tintText = "";
                    break;
            }

            if (gameObject.activeInHierarchy)
            {
                text = tintText;
                StartColorTween(tintColor * m_Colors.colorMultiplier, instant);
            }
        }
    }
}