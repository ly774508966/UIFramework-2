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
        if (null == obj)
        {
            return null;
        }
        T curComponent = obj.GetComponent<T>();
        if (null == curComponent)
        {
            curComponent = obj.AddComponent<T>();
        }
        return curComponent;
    }


    /// <summary>
    /// 查找子物体
    /// </summary>
    /// <param name="goParent"></param>
    /// <param name="childNama"></param>
    /// <returns></returns>
    public static Transform FindTheChild(GameObject goParent, string childNama)
    {
        Transform searchTrans = goParent.transform.Find(childNama);
        if (null == searchTrans)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childNama);
                if (null != searchTrans)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }

    /// <summary>
    /// 获取子物体上面的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="goParent"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (null != searchTrans)
        {
            return searchTrans.gameObject.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 给物体添加子物体
    /// </summary>
    /// <param name="parentTrs"></param>
    /// <param name="childTrs"></param>
    public static void AddChildToParent(Transform parentTrs, Transform childTrs)
    {
        if (null == parentTrs || null == childTrs)
        {
            return;
        }
        //设置局部坐标和局部尺寸以及欧拉角
        childTrs.SetParent(parentTrs);
        childTrs.localPosition = Vector3.zero;
        childTrs.localScale = Vector3.one;
        childTrs.localEulerAngles = Vector3.zero;
        SetLayer(parentTrs.gameObject.layer, childTrs);
    }

    /// <summary>
    /// 改变游戏物体的层级（通过递归同时设置子物体的子物体）
    /// </summary>
    /// <param name="parentLayer"></param>
    /// <param name="childTrs"></param>
    public static void SetLayer(int parentLayer, Transform childTrs)
    {
        childTrs.gameObject.layer = parentLayer;
        for (int i = 0; i < childTrs.childCount; ++i)
        {
            Transform child = childTrs.GetChild(i);
            child.gameObject.layer = parentLayer;
            SetLayer(parentLayer, child);
        }
    }
}
