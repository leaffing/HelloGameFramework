using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Custom.UI
{
    public interface IStateTransition
    {
        void DoStateTransition(SelectionState state, bool instant);
    }
}
