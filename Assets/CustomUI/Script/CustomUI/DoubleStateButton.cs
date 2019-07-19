using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Custom.UI
{
    [Serializable]
    public enum State
    {
        First,
        Second
    }

    [AddComponentMenu("CustomUI/DoubleStateButton", 61)]
    public class DoubleStateButton : SelectableUI
    {
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

        public ColorBlock alternativeColors { get { return m_AlternativeColorBlock; } set { if (SetPropertyUtility.SetStruct(ref m_AlternativeColorBlock, value)) OnSetProperty(); } }
        public SpriteState alternativeSpriteState { get { return m_AlternativeSpriteState; } set { if (SetPropertyUtility.SetStruct(ref m_AlternativeSpriteState, value)) OnSetProperty(); } }
        public AnimationTriggers alternativeAnimationTriggers { get { return m_AlternativeAnimTrigger; } set { if (SetPropertyUtility.SetClass(ref m_AlternativeAnimTrigger, value)) OnSetProperty(); } }

        [FormerlySerializedAs("curState")]
        [SerializeField]
        public State m_CurState = new State();
        [FormerlySerializedAs("onChangeToFistState")]
        [SerializeField]
        public UnityEvent m_OnChangeToFistState = new UnityEvent();
        [FormerlySerializedAs("onChangeToSecondState")]
        [SerializeField]
        public UnityEvent m_OnChangeToSecondState = new UnityEvent();

        private bool HasHold = false;
        protected DoubleStateButton()
        { }

        public UnityEvent onChangeToFistState
        {
            get { return m_OnChangeToFistState; }
            set { m_OnChangeToFistState = value; }
        }
        public UnityEvent onChangeToSecondState
        {
            get { return m_OnChangeToSecondState; }
            set { m_OnChangeToSecondState = value; }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            if (!IsActive() || !IsInteractable())
                return;
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            DoStateTransition(SelectionState.Normal, false);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            if (!IsActive() || !IsInteractable())
                return;
            switch(m_CurState)
            {
                case State.First:
                    {
                        m_CurState = State.Second;
                        m_OnChangeToSecondState.Invoke();
                        break;
                    }
                case State.Second:
                    {
                        m_CurState = State.First;
                        m_OnChangeToFistState.Invoke();
                        break;
                    }
            }
            DoStateTransition(SelectionState.Normal, false);
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            m_AlternativeColorBlock.fadeDuration = Mathf.Max(m_AlternativeColorBlock.fadeDuration, 0.0f);
            base.OnValidate();
        }
#endif // if UNITY_EDITOR
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            Color tintColor = Color.black;
            Sprite transitionSprite = null;
            string triggerName = string.Empty;

            switch (state)
            {
                case SelectionState.Normal:
                    switch(m_CurState)
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
    }
}
