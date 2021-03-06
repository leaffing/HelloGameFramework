﻿namespace Leaf.SimpleMVC
{
    public abstract class Model
    {
        //名字标识
        public abstract string Name { get; }

        //发送事件
        protected void SendEvent(string eventName, object data = null)
        {
            Facade.SendEvent(eventName, data);
        }
    }
}