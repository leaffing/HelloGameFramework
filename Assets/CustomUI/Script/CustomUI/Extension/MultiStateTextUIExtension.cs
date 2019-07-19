using UnityEngine;
using UnityEngine.UI;
using Custom.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;

namespace Custom.UI
{
    [AddComponentMenu("CustomUI/MultiStateTextUIExtension", 23)]
    [RequireComponent(typeof(CanvasRenderer))]
    public class MultiStateTextUIExtension : MultiStateTextUI,IDoubleState
    {

        [FormerlySerializedAs("curState")]
        [SerializeField]
        public State m_CurState = new State();

        // Colors used for a color tint-based transition.
        [FormerlySerializedAs("colors")]
        [SerializeField]
        public ColorBlock m_AlternativeColorBlock = ColorBlock.defaultColorBlock;
        // Colors used for a color tint-based transition.
        [FormerlySerializedAs("texts")]
        [SerializeField]
        protected TextBlock m_AlternativeTexts = TextBlock.defaultTextBlock;
        public ColorBlock alternativeColors { get { return m_AlternativeColorBlock; } set { SetPropertyUtility.SetStruct(ref m_AlternativeColorBlock, value); } }
        public TextBlock alternativeTexts { get { return m_AlternativeTexts; } set { SetPropertyUtility.SetStruct(ref m_AlternativeTexts, value); } }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            m_AlternativeColorBlock.fadeDuration = Mathf.Max(m_AlternativeColorBlock.fadeDuration, 0.0f);
            base.OnValidate();
        }
#endif // if UNITY_EDITOR
        public override void DoStateTransition(SelectionState state, bool instant)
        {
            Color tintColor = Color.black;
            string tintText = string.Empty;

            switch (state)
            {
                case SelectionState.Normal:
                    switch (m_CurState)
                    {
                        case State.First:
                            {
                                tintColor = m_Colors.normalColor;
                                tintText = m_Texts.normalText;
                                break;
                            }
                        case State.Second:
                            {
                                tintColor = m_AlternativeColorBlock.normalColor;
                                tintText = m_AlternativeTexts.normalText;
                                break;
                            }
                    }
                    break;
                case SelectionState.Highlighted:
                    switch (m_CurState)
                    {
                        case State.First:
                            {
                                tintColor = m_Colors.highlightedColor;
                                tintText = m_Texts.highlightedText;
                                break;
                            }
                        case State.Second:
                            {
                                tintColor = m_AlternativeColorBlock.highlightedColor;
                                tintText = m_AlternativeTexts.highlightedText;
                                break;
                            }
                    }
                    break;
                case SelectionState.Pressed:
                    switch (m_CurState)
                    {
                        case State.First:
                            {
                                tintColor = m_Colors.pressedColor;
                                tintText = m_Texts.pressedText;
                                break;
                            }
                        case State.Second:
                            {
                                tintColor = m_AlternativeColorBlock.pressedColor;
                                tintText = m_AlternativeTexts.pressedText;
                                break;
                            }
                    }
                    break;
                case SelectionState.Disabled:
                    switch (m_CurState)
                    {
                        case State.First:
                            {
                                tintColor = m_Colors.disabledColor;
                                tintText = m_Texts.disabledText;
                                break;
                            }
                        case State.Second:
                            {
                                tintColor = m_AlternativeColorBlock.disabledColor;
                                tintText = m_AlternativeTexts.disabledText;
                                break;
                            }
                    }
                    break;
                default:
                    tintColor = Color.black;
                    tintText = string.Empty;
                    break;
            }

            if (gameObject.activeInHierarchy)
            {
                text = tintText;
                StartColorTween(tintColor * m_Colors.colorMultiplier, instant);
            }
        }
        public void ChangeToFirstState()
        {
            m_CurState = State.First;
        }

        public void ChangeToSecondState()
        {
            m_CurState = State.Second;
        }
    }
}