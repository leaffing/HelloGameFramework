using System;
using UnityEngine;
using strange.extensions.context.impl;

namespace Game {
    public class HelloWorldContext : SignalContext {
        public HelloWorldContext(MonoBehaviour contextView) : base(contextView) {
        }

        protected override void mapBindings() {
            base.mapBindings();
            // we bind a command to StartSignal since it is invoked by SignalContext (the parent class) on Launch()
            commandBinder.Bind<StartSignal>().To<HelloWorldStartCommand>().Once(); //只能触发一次
            commandBinder.Bind<DoManagementSignal>().To<DoManagementCommand>();

            // bind our view to its mediator
            mediationBinder.Bind<HelloWorldView>().To<HelloWorldMediator>();

            // bind the manager implemented as a MonoBehaviour
            var manager = GameObject.Find("Manager").GetComponent<ManagerAsMonoBehaviour>();
            injectionBinder.Bind<ISomeManager>().ToValue(manager);
            //inject model
            injectionBinder.Bind<IHelloWorldModel>().To<HelloWorldModel>();


        }

    }
}