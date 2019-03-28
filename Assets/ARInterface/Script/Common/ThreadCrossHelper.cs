using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{ 
    /// <summary>
    ///  线程交叉访问助手
    /// </summary>
    public class ThreadCrossHelper : MonoSingleton<ThreadCrossHelper> 
	{
        //private List<Action> actionList;
        //private List<float> timeList; 
        class DelayedItem
        {
            public Action CurrentAction { get; set; }

            public DateTime Time { get; set; }
        }

        private List<DelayedItem> list;

        protected override void Init()
        {
            base.Init();
            list = new List<DelayedItem>();
        }

        private void Update()
        {
            //倒序删除数组元素
            for (int i = list.Count - 1; i >=0 ; i--)
            {
                if (list[i].Time <= DateTime.Now)
                {
                    //添加元素 与 删除元素 排队执行。
                    lock (list)
                    {
                        list[i].CurrentAction();
                        list.RemoveAt(i);
                    }
                }
            }
        }
        /// <summary>
        ///  在主线程执行方法
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="delay"></param>
        public void ExecuteOnMainThread(Action handle, float delay = 0)
        {
            //添加元素 与 删除元素 排队执行。
            lock (list)
            {
                var item = new DelayedItem()
                {
                    CurrentAction = handle,
                    Time = DateTime.Now.AddSeconds(delay)
                };
                list.Add(item);
            }
        }
	}
}
