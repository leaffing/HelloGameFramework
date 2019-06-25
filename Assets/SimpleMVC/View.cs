using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leaf.SimpleMVC
{
    public abstract class View : MonoBehaviour
    {
        //名字标识
        public abstract string Name { get; }

        //事件关心列表
        [HideInInspector]
        public List<string> AttentionList = new List<string>();

        //初始化事件关心列表,在注册视图的时候被调用
        public virtual void RegisterAttentionEvent() { }

        //处理事件
        public abstract void HandleEvent(string name, object data);

        //发送事件
        protected void SendEvent(string eventName, object data = null)
        {
            Facade.SendEvent(eventName, data);
        }

        //获取模型
        protected T GetModel<T>()
            where T : Model
        {
            return Facade.GetModel<T>() as T;
        }
    }
}