using System;

using UnityEngine;

using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Game {
    public class HelloWorldView : View {

        public Signal buttonClicked = new Signal();

        private Rect buttonRect = new Rect(0, 0, 200, 50);

        [Inject]
        public DoManagementSignal doManagement {get;set;}

        public void OnGUI() {
            if(GUI.Button(buttonRect,"Manage")) {
                buttonClicked.Dispatch();
                //doManagement.Dispatch();
            }
        }

    }
}