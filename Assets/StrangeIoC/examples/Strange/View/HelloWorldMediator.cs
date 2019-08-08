using System;

using UnityEngine;

using strange.extensions.mediation.impl;

namespace Game {
    public class HelloWorldMediator : Mediator
    {

        [Inject] public HelloWorldView view { get; set; }

        [Inject] public DoManagementSignal doManagement { get; set; }

        [Inject]
        public IHelloWorldModel model { get; set; }

        public override void OnRegister() {
            
            model.data = "Hello world from HelloWorldView!";
            
            view.buttonClicked.AddListener(()=>
            {
                Debug.Log(model.data);
                doManagement.Dispatch();
            });
        }

    }
}