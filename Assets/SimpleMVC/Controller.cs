using System;

namespace Leaf.SimpleMVC
{
    public abstract class Controller
    {
        //执行事件
        public abstract void Execute(object data);

        //获取模型
        protected T GetModel<T>()
            where T : Model
        {
            return Facade.GetModel<T>() as T;
        }
        //获取视图
        protected T GetView<T>()
            where T : View
        {
            return Facade.GetView<T>() as T;
        }
        //注册模型
        protected void RegisterModel(Model model)
        {
            Facade.RegisterModel(model);
        }
        //注册视图
        protected void RegisterView(View view)
        {
            Facade.RegisterView(view);
        }
        //注册控制器
        protected void RegisterController(string eventName, Type controllerType)
        {
            Facade.RegisterController(eventName, controllerType);
        }
    }
}