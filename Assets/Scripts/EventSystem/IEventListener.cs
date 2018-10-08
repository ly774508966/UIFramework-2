/*****************************************************************************
 * filename :  IEventListener.cs
 * author   :  Zhang Yunxing
 * date     :  2018/10/08 17:52
 * desc     :  事件管理接口
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Event
{
    interface IEventListener
    {
        void AddEvent();
        void RemoveEvent();
    }
}