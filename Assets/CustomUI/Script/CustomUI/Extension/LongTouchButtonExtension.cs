using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Custom.UI
{
    [AddComponentMenu("CustomUI/LongTouchButtonExtension", 52)]
    public class LongTouchButtonExtension : LongTouchButton
    {
        [FormerlySerializedAs("m_AssociatedObjects")]
        [SerializeField]
        public List<UIBehaviour> m_AssociatedObject = new List<UIBehaviour>();
        private List<IStateTransition> m_PointerStates = new List<IStateTransition>();
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
        }
        public void UpdateAssociatedObject()
        {
            m_PointerStates.Clear();
            int count = (m_AssociatedObject?.Count ?? 0);
            for (int i = 0; i < count; ++i)
            {
                var stateTransition = (m_AssociatedObject[i]?.GetComponent<IStateTransition>()??null);
                if (stateTransition == null)
                    continue;
                m_PointerStates.Add(stateTransition);
            }
        }
    }
}
