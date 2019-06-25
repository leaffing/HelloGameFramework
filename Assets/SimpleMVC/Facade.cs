using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaf.SimpleMVC
{
    public static class Facade
    {
        //资源
        //名字 -- model(保存的是实例对象)
        public static Dictionary<string, Model> Models = new Dictionary<string, Model>();
        //名字 -- view(保存的是实例对象)
        public static Dictionary<string, View> Views = new Dictionary<string, View>();
        //事件名字 -- 类型 (保存的是类)
        public static Dictionary<string, Type> ComandMap = new Dictionary<string, Type>();

        //注册view,注意这里的view其实是由 继承于View的子类上溯而来(下同)
        public static void RegisterView(View view)
        {
            //防止view的重复注册
            if (Views.ContainsKey(view.Name))
            {
                Views.Remove(view.Name);
            }
            view.RegisterAttentionEvent();//调用视图方法,注册视图关心事件,存放在关心事件列表中
            Views[view.Name] = view;
        }
        //注册model
        public static void RegisterModel(Model model)
        {
            Models[model.Name] = model;
        }
        //注册controller 将一个事件执行器放入字典，以eventName为键,Type类型是类,就是放入一个需要被实例化的类,使用的时候必须要实例化再使用
        public static void RegisterController(string eventName, Type controllerType)
        {
            ComandMap[eventName] = controllerType;
        }

        //获取model,T是外部传进来的模型脚本,该脚本必须继承自Model
        public static T GetModel<T>()
            where T : Model
        {
            foreach (var m in Models.Values)
            {
                //m肯定是Model类型，但是这个m 是由 继承于Model的脚本上溯而来的,这里会进行下溯 看看m是否等于T
                if (m is T)
                {
                    //若等于，则先强转为T脚本 再返回.
                    return (T)m;
                }
            }
            return null;
        }

        //获取view
        public static T GetView<T>()
            where T : View
        {
            foreach (var v in Views.Values)
            {
                if (v is T)
                {
                    return (T)v;
                }
            }
            return null;
        }

        //发送事件(对于外部调用者来说该方法是发送事件，对于内部方法来说是不同的控制器和视图处理事件)
        public static void SendEvent(string eventName, object data = null)
        {
            //在这里可发现一个事件对应一个Controller处理,具体事件继承于抽象事件，一个具体事件的诞生首先要进行继承于Controller 重写Execute 注册入CommandMap字典三步骤
            //controller 执行,eventName是事件名称,若在控制器字典内存在该事件名称，则肯定会有一个控制器去处理该事件
            if (ComandMap.ContainsKey(eventName))
            {
                //t脚本类是继承于Controller类的,不然下面无法转换为Controller
                Type t = ComandMap[eventName];
                //根据字典取出来的t,去实例化一个对象，并且object转化为Controller类型,因为t对象继承于Controller所以可以转化
                Controller c = Activator.CreateInstance(t) as Controller;
                //执行被t所重写的Execute方法,data是传入的数据(object类型)
                c.Execute(data);
            }
            //view处理
            //遍历所有视图,注意:一个视图允许有多个事件，而且一个事件可能会在不同的视图触发，而事件的内容不确定（事件可理解为触发消息）
            foreach (var v in Views.Values)
            {
                //视图v的关心事件列表中存在该事件
                if (v.AttentionList.Contains(eventName))
                {
                    //让视图v执行该事件eventName,附带参数data
                    //HandleEvent方法是通过switch case的形式处理不同的事件
                    v.HandleEvent(eventName, data);
                }
            }
        }
    }
}