/*****************************************************************************
 * filename :  Utils_General.cs
 * author   :  Zhang Yunxing
 * date     :  2018/08/29 19:25
 * desc     :  General Utils
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Utils
{
    /// <summary>
    /// 向指定物体添加T类型组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T TryAddComponent<T>(GameObject obj) where T : Component
    {
        if (null == obj) return null;
        T curComponent = obj.GetComponent<T>();
        if (null == curComponent)
        {
            curComponent = obj.AddComponent<T>();
        }
        return curComponent;
    }
}
