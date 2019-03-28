using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 数组助手类：封装开发中对数组的常用操作
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// 获取对象数组中，满足条件的最大元素。
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型 例如：Enemy</typeparam>
        /// <typeparam name="Q">条件的类型 例如：int</typeparam>
        /// <param name="array">对象数组 例如：Enemy[]</param>
        /// <param name="condition">条件 例如：HP</param>
        /// <returns></returns>
        public static T GetMax<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            var max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (condition(max).CompareTo(condition(array[i])) < 0)
                {
                    max = array[i];
                }
            }
            return max;
        }

        /// <summary>
        /// 获取对象数组中，满足条件的最小元素。
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型 例如：Enemy</typeparam>
        /// <typeparam name="Q">条件的类型 例如：int</typeparam>
        /// <param name="array">对象数组 例如：Enemy[]</param>
        /// <param name="condition">条件 例如：HP</param>
        /// <returns></returns>
        public static T GetMin<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            var min = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (condition(min).CompareTo(condition(array[i])) > 0)
                {
                    min = array[i];
                }
            }
            return min;
        }

        /// <summary>
        /// 在对象数组中查找满足条件的所有元素
        /// </summary>
        /// <typeparam name="T">对象数组元素类型</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T[] FindAll<T>(this T[] array, Func<T, bool> condition)
        {
            List<T> list = new List<T>(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 在对象数组中查找满足条件的单个元素
        /// </summary>
        /// <typeparam name="T">对象数组元素类型</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T Find<T>(this T[] array, Func<T, bool> condition)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                {
                    return array[i];
                }
            }
            return default(T); //返回该类型默认值
        }

        /// <summary>
        /// 对象数组升序排列
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型 例如：Enemy</typeparam>
        /// <typeparam name="Q">条件的类型 例如：int</typeparam>
        /// <param name="array">对象数组 例如：Enemy[]</param>
        /// <param name="condition">条件 例如：HP</param>
        public static void OrderBy<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            for (int r = 0; r < array.Length - 1; r++)
            {
                for (int c = r + 1; c < array.Length; c++)
                {
                    if (condition(array[r]).CompareTo(condition(array[c])) > 0)
                    {
                        var temp = array[r];
                        array[r] = array[c];
                        array[c] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 对象数组降序排列
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型 例如：Enemy</typeparam>
        /// <typeparam name="Q">条件的类型 例如：int</typeparam>
        /// <param name="array">对象数组 例如：Enemy[]</param>
        /// <param name="condition">条件 例如：HP</param>
        public static void OrderByDescending<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            for (int r = 0; r < array.Length - 1; r++)
            {
                for (int c = r + 1; c < array.Length; c++)
                {
                    if (condition(array[r]).CompareTo(condition(array[c])) < 0)
                    {
                        var temp = array[r];
                        array[r] = array[c];
                        array[c] = temp;
                    }
                }
            }
        }

        public static Q[] Select<T,Q>(this T[] array,Func<T,Q> handler)
        {
            Q[] result = new Q[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                result[i] = handler(array[i]);
            }
            return result;
        }
    }
}
