using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
namespace Custom.UI
{
    [AddComponentMenu("CustomUI/LongTouchButton", 51)]
    public class LongTouchButton : SelectableUI
    {
        [FormerlySerializedAs("onLongTouchStart")]
        [SerializeField]
        private UnityEvent m_OnLongTouchStart = new UnityEvent();
        [FormerlySerializedAs("onLongTouch")]
        [SerializeField]
        private UnityEvent m_OnLongTouch = new UnityEvent();
        [FormerlySerializedAs("onLongTouchEnd")]
        [SerializeField]
        private UnityEvent m_OnLongTouchEnd = new UnityEvent();

        private bool HasHold = false;
        protected LongTouchButton()
        { }

        public UnityEvent onLongTouchStart
        {
            get { return m_OnLongTouchStart; }
            set { m_OnLongTouchStart = value; }
        }
        public UnityEvent onLongTouch
        {
            get { return m_OnLongTouch; }
            set { m_OnLongTouch = value; }
        }
        public UnityEvent onLongTouchEnd
        {
            get { return m_OnLongTouchEnd; }
            set { m_OnLongTouchEnd = value; }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            if (!IsActive() || !IsInteractable())
                return;
            m_OnLongTouchStart.Invoke();
            HasHold = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            if (!IsActive() || !IsInteractable())
                return;
            m_OnLongTouchEnd.Invoke();
            HasHold = false;
        }
        private void Update()
        {
            if (!IsActive() || !IsInteractable())
                return;
            if(HasHold)
                m_OnLongTouch.Invoke();
        }
    }
}
