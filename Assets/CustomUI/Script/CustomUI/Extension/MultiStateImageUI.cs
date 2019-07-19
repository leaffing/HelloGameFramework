using UnityEngine;
using UnityEngine.UI;
using Custom.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Custom.UI
{
    [AddComponentMenu("CustomUI/MultiStateImageUI", 12)]
    [RequireComponent(typeof(CanvasRenderer))]
    public class MultiStateImageUI : ImageUI, IStateTransition
    {
        // Type of the transition that occurs when the button state changes.
        [FormerlySerializedAs("transition")]
        [SerializeField]
        protected Transition m_Transition = Transition.ColorTint;

        // Colors used for a color tint-based transition.
        [FormerlySerializedAs("colors")]
        [SerializeField]
        protected ColorBlock m_Colors = ColorBlock.defaultColorBlock;

        // Sprites used for a Image swap-based transition.
        [FormerlySerializedAs("spriteState")]
        [SerializeField]
        protected SpriteState m_SpriteState;

        [FormerlySerializedAs("animationTriggers")]
        [SerializeField]
        protected AnimationTriggers m_AnimationTriggers = new AnimationTriggers();

        // Graphic that will be colored.
        [FormerlySerializedAs("highlightGraphic")]
        [FormerlySerializedAs("m_HighlightGraphic")]
        [SerializeField]
        private Graphic m_TargetGraphic;

        // Get the animator
        public Animator animator
        {
            get { return GetComponent<Animator>(); }
        }
        public Transition transition { get { return m_Transition; } set { SetPropertyUtility.SetStruct(ref m_Transition, value); } }
        public ColorBlock colors { get { return m_Colors; } set { SetPropertyUtility.SetStruct(ref m_Colors, value); } }
        public SpriteState spriteState { get { return m_SpriteState; } set { SetPropertyUtility.SetStruct(ref m_SpriteState, value); } }
        public AnimationTriggers animationTriggers { get { return m_AnimationTriggers; } set { SetPropertyUtility.SetClass(ref m_AnimationTriggers, value); } }
        public Graphic targetGraphic { get { return m_TargetGraphic; } set { SetPropertyUtility.SetClass(ref m_TargetGraphic, value); } }
       
        // Convenience function that converts the Graphic to a Image, if possible
        public ImageUI image
        {
            get { return m_TargetGraphic as ImageUI; }
            set { m_TargetGraphic = value; }
        }
        protected override void Awake()
        {
            if (m_TargetGraphic == null)
                m_TargetGraphic = GetComponent<Graphic>();
        }
        //protected override void Reset()
        //{
        //    m_TargetGraphic = GetComponent<Graphic>();
        //}

        protected override void OnEnable()
        {
            base.OnEnable();
            DoStateTransition(SelectionState.Normal, false);
        }
        protected virtual void StartColorTween(Color targetColor, bool instant)
        {
            if (m_TargetGraphic == null)
                return;

            m_TargetGraphic.CrossFadeColor(targetColor, instant ? 0f : m_Colors.fadeDuration, true, true);
        }
        public virtual void DoStateTransition(SelectionState state, bool instant)
        {
            Color tintColor;
            Sprite transitionSprite;
            string triggerName;

            switch (state)
            {
                case SelectionState.Normal:
                    tintColor = m_Colors.normalColor;
                    transitionSprite = null;
                    triggerName = m_AnimationTriggers.normalTrigger;
                    break;
                case SelectionState.Highlighted:
                    tintColor = m_Colors.highlightedColor;
                    transitionSprite = m_SpriteState.highlightedSprite;
                    triggerName = m_AnimationTriggers.highlightedTrigger;
                    break;
                case SelectionState.Pressed:
                    tintColor = m_Colors.pressedColor;
                    transitionSprite = m_SpriteState.pressedSprite;
                    triggerName = m_AnimationTriggers.pressedTrigger;
                    break;
                case SelectionState.Disabled:
                    tintColor = m_Colors.disabledColor;
                    transitionSprite = m_SpriteState.disabledSprite;
                    triggerName = m_AnimationTriggers.disabledTrigger;
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

        protected virtual void DoSpriteSwap(Sprite newSprite)
        {
            if (image == null)
                return;
            image.overrideSprite = newSprite;
        }

        protected void TriggerAnimation(string triggername)
        {
            if (transition != Transition.Animation || animator == null || !animator.isActiveAndEnabled || !animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
                return;

            animator.ResetTrigger(m_AnimationTriggers.normalTrigger);
            animator.ResetTrigger(m_AnimationTriggers.pressedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.highlightedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.disabledTrigger);

            animator.SetTrigger(triggername);
        }
    }
}