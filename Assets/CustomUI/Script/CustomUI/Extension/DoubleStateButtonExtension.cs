using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Custom.UI
{
    [AddComponentMenu("CustomUI/DoubleStateButtonExtension", 62)]
    public class DoubleStateButtonExtension : DoubleStateButton
    {
        [FormerlySerializedAs("m_AssociatedObjects")]
        [SerializeField]
        public List<UIBehaviour> m_AssociatedObject = new List<UIBehaviour>();
        private List<IStateTransition> m_PointerStates = new List<IStateTransition>();
        private List<IDoubleState> m_ButtonStates = new List<IDoubleState>();
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            int count = (m_PointerStates?.Count ?? 0);
            if (count <= 0)
                return;
            for (int i = 0; i < count; ++i)
                m_PointerStates[i].DoStateTransition(state, instant);
        }
        protected override void OnEnable()
        {
            UpdateAssociatedObject();
            onChangeToFistState.AddListener(ChangeToFirstStateMessage);
            onChangeToSecondState.AddListener(ChangeToSecondStateMessage);
        }
        protected override void OnDisable()
        {
            onChangeToFistState.RemoveListener(ChangeToFirstStateMessage);
            onChangeToSecondState.RemoveListener(ChangeToSecondStateMessage);
        }
        public void UpdateAssociatedObject()
        {
            m_PointerStates.Clear();
            m_ButtonStates.Clear();
            int count = (m_AssociatedObject?.Count ?? 0);
            for (int i = 0; i < count; ++i)
            {
                var stateTransition = (m_AssociatedObject[i]?.GetComponent<IStateTransition>()??null);
                if (stateTransition != null)
                    m_PointerStates.Add(stateTransition);
                var buttonState = (m_AssociatedObject[i]?.GetComponent<IDoubleState>()??null);
                if (buttonState != null)
                    m_ButtonStates.Add(buttonState);

            }
        }
        private void ChangeToFirstStateMessage()
        {
            int count = (m_ButtonStates?.Count ?? 0);
            if (count <= 0)
                return;
            for (int i = 0; i < count; ++i)
                m_ButtonStates[i].ChangeToFirstState();
        }
        private void ChangeToSecondStateMessage()
        {
            int count = (m_ButtonStates?.Count ?? 0);
            if (count <= 0)
                return;
            for (int i = 0; i < count; ++i)
                m_ButtonStates[i].ChangeToSecondState();
        }
    }
}
