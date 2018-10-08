/*****************************************************************************
 * filename :  CoreDefine_Event.cs
 * author   :  Zhang Yunxing
 * date     :  2018/10/08 17:53
 * desc     :  事件管理相关定义
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.CoreDefine
{
    public partial class CoreGlobeVar
    {
        public enum HandleType
        {
            Add = 0,
            Remove = 1,
        }

        public static Dictionary<int, string> HandleTypeDic = new Dictionary<int, string>()
        {
            { (int)HandleType.Add, "Add"},
            { (int)HandleType.Remove, "Remove"},
        };
    }
}
