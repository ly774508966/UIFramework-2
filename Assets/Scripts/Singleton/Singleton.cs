/*****************************************************************************
 * filename :  Singleton.cs
 * author   :  Zhang Yunxing
 * date     :  2018/10/08 19:54
 * desc     :  非mono脚本单例基类, dcl
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new()
{
    private static volatile T instance = default(T);
    private static object syncRoot = new Object();
    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                lock (syncRoot)
                {
                    if (null == instance)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}