using System;
using UnityEngine;

namespace Game {
    public class ManagerAsMonoBehaviour : MonoBehaviour, ISomeManager
    {

        [Inject] public IHelloWorldModel model { get; set; }

        void Awake()
        {

        }

        #region ISomeManager implementation
        public void DoManagement() {
            model.data = "Hello World form Manager!!!";

            Debug.Log(model.data);
        }
        #endregion
    }
}