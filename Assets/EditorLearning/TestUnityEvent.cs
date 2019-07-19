using System;
using UnityEngine;
using UnityEngine.Events;

public class TestUnityEvent:MonoBehaviour
{
    public int arg01 = 101;
    public int arg02 = 201;
    public string arg03 = "参数来自暴露的字段";
    [Serializable]
    public class MyEventTest0 : UnityEvent<EventInfo> { }//Inspector面板可见

    [Serializable]
    public class MyEventTest1 : UnityEvent<int,int,string> { } //Inspector面板可见

    public class MyEventTest<T0, T1> : UnityEvent<T0, T1> { }//泛型参数事件，Inspector面板不可见

    public UnityEvent TestEvent0; //原始UnityEvent
    public MyEventTest1 TestEvent1;
    public MyEventTest0 TestEvent2;//正常情况下我们这么写public MyEventTest0 TestEvent0= new MyEventTest0();但是这个事件类会自动在面板上绘制而被自动实例化，所以可以不new。

    [SerializeField]
    public MyEventTest<int, int> m_TimeEvent0 = new MyEventTest<int, int>();      //int+int  ；区别于上面情况，这个不能在面板绘制，所以必须new对象，否则对象为空异常。
    public MyEventTest <string ,string> m_TimeEvent1=new MyEventTest<string, string> ();//string+string
    public MyEventTest <int ,string>    m_TimeEvent2=new MyEventTest<int, string> ();   //int+string
    public MyEventTest <string ,int>    m_TimeEvent3=new MyEventTest<string, int> ();   //string+int
    private void Start()
    {
        TestEvent0.AddListener(OrginActionFromScript); 
        TestEvent0.AddListener(() => DoAction("参数来自于脚本添加"));    //弊端：订阅时必须给实参，但是如果将该参数在外界声明，也就又变成动态参数啦 
        TestEvent0.AddListener(() => DoMultAction(arg01, arg02, arg03)); //原始的无参事件也可以订阅多参数方法（lambda）
        
        TestEvent1.AddListener((a, b, c) => DoMultAction(a, b, c));
        TestEvent2.AddListener((v)=>DoObjAction(v));
        
        //泛型事件使用脚本订阅（也只能脚本订阅）
        m_TimeEvent0.AddListener((a,b)=>DoAction01(a,b));           //优点：订阅时不必给实参，在执行时才需要给实参（方便动态更新实参）
        m_TimeEvent1.AddListener((a, b) => DoAction01(a, b));
        m_TimeEvent2.AddListener((a, b) => DoAction01(a, b));
        m_TimeEvent3.AddListener((a, b) => DoAction01(a, b));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestEvent0.Invoke(); //这个里面包含了无参，单个参数，组合参数，当然也能用lambda给对象参数，其他的也能举一反三哦
            TestEvent1.Invoke(11,22,"参数来自脚本");//多参数事件需要 组合参数
            TestEvent2.Invoke(new EventInfo {age=10,name="翟欣欣",sex="女" }); //给的对象参数
            m_TimeEvent0.Invoke(1,1);                    //int+int 
            m_TimeEvent1.Invoke("fEvent1", "bEvent1");   //string+string
            m_TimeEvent2.Invoke(122,"bEvent2");          //int+string
            m_TimeEvent3.Invoke("Event3",9527);          //string+int
        }
    }
    public void OrginActionFromScript()
    {
        Debug.Log("么么哒-脚本订阅");
    }
    public void OrginActionFromInspector()
    {
        Debug.Log("么么哒-脚本订阅");
    }
    public void DoAction()
    {
        Debug.Log("萌萌哒！");
    }
    public void DoAction(string msg)
    {
        Debug.Log("萌萌哒！+"+msg);
    }
    public void DoMultAction(int a,int b,string c)
    {
        Debug.Log(a+":"+b+":"+c);
    }
    public void DoObjAction(EventInfo info)
    {
        Debug.LogFormat("姓名：{0}，年龄：{1}，性别：{2}。",info.name,info.age,info.sex);
    }

    public void DoAction01<TO,T1>(TO a,T1 b)
    {
        string type0 = a.GetType().ToString().Split('.')[1];
        string type1 = b.GetType().ToString().Split('.')[1];
        Debug.LogFormat("参数1→“{0}”：“{1}”，参数2→“{2}”：“{3}”", a,type0,b,type1);
    }
}
public class EventInfo
{
    public string name;
    public int age;
    public string sex;
}