using UnityEngine;
using UnityEngine.UI;
using Custom.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Custom.UI
{
    [AddComponentMenu("CustomUI/MultiStateImageUIExtension", 13)]
    [RequireComponent(typeof(CanvasRenderer))]
    public class MultiStateImageUIExtension : MultiStateImageUI, IDoubleState
    {
        [FormerlySerializedAs("curState")]
        [SerializeField]
        public State m_CurState = new State();

        // Colors used for a color tint-based transition.
        [FormerlySerializedAs("colors")]
        [SerializeField]
        public ColorBlock m_AlternativeColorBlock = ColorBlock.defaultColorBlock;

        // Sprites used for a Image swap-based transition.
        [FormerlySerializedAs("spriteState")]
        [SerializeField]
        public SpriteState m_AlternativeSpriteState;

        [FormerlySerializedAs("animationTriggers")]
        [SerializeField]
        public AnimationTriggers m_AlternativeAnimTrigger = new AnimationTriggers();

        public ColorBlock alternativeColors { get { return m_AlternativeColorBlock; } set { SetPropertyUtility.SetStruct(ref m_AlternativeColorBlock, value); } }
        public SpriteState alternativeSpriteState { get { return m_AlternativeSpriteState; } set { SetPropertyUtility.SetStruct(ref m_AlternativeSpriteState, value); } }
        public AnimationTriggers alternativeAnimationTriggers { get { return m_AlternativeAnimTrigger; } set { SetPropertyUtility.SetClass(ref m_AlternativeAnimTrigger, value); } }


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
            Sprite transitionSprite = null;
            string triggerName = string.Empty;

            switch (state)
            {
                case SelectionState.Normal:
                    switch (m_CurState)
                    {
                        case State.First:
                            {
                                tintColor = m_Colors.normalColor;
                                transitionSprite = m_SpriteState.pressedSprite;
                                triggerName = m_AnimationTriggers.normalTrigger;
                                break;
                            }
                        case State.Second:
                            {
                                tintColor = m_AlternativeColorBlock.normalColor;
                                transitionSprite = m_AlternativeSpriteState.pressedSprite;
                                triggerName = m_AlternativeAnimTrigger.normalTrigger;
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
                                transitionSprite = m_SpriteState.highlightedSprite;
                                triggerName = m_AnimationTriggers.highlightedTrigger;
                                break;
                            }
                        case State.Second:
                            {
                                tintColor = m_AlternativeColorBlock.highlightedColor;
                                transitionSprite = m_AlternativeSpriteState.highlightedSprite;
                                triggerName = m_AlternativeAnimTrigger.highlightedTrigger;
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
                                transitionSprite = m_SpriteState.pressedSprite;
                                triggerName = m_AnimationTriggers.pressedTrigger;
                                break;
                            }
                        case State.Second:
                            {
                                tintColor = m_AlternativeColorBlock.pressedColor;
                                transitionSprite = m_AlternativeSpriteState.pressedSprite;
                                triggerName = m_AlternativeAnimTrigger.pressedTrigger;
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
                                transitionSprite = m_SpriteState.disabledSprite;
                                triggerName = m_AnimationTriggers.disabledTrigger;
                                break;
                            }
                        case State.Second:
                            {
                                tintColor = m_AlternativeColorBlock.disabledColor;
                                transitionSprite = m_AlternativeSpriteState.disabledSprite;
                                triggerName = m_AlternativeAnimTrigger.disabledTrigger;
                                break;
                            }
                    }
                    break;
                default:
                    tintColor = Color.black;
                    transitionSprite = null;
                    triggerName = string.Empty;
                    break;
            }

            if (gameObject.activeInHierarchy)
            {
                switch (m_Transition)
                {
                    case Transition.ColorTint:
                        StartColorTween(tintColor * m_Colors.colorMultiplier, instant);
                        break;
                    case Transition.SpriteSwap:
                        DoSpriteSwap(transitionSprite);
                        break;
                    case Transition.Animation:
                        TriggerAnimation(triggerName);
                        break;
                }
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
