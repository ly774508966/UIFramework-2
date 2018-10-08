/*****************************************************************************
 * filename :  EventDispatcher.cs
 * author   :  Zhang Yunxing
 * date     :  2018/10/08 17:55
 * desc     :  事件中心逻辑驱动器
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Games.Event
{
    public class EventDispatcher : Singleton<EventDispatcher>
    {
        private EventManager eventCommonManager = new EventManager();

        public static EventManager MainEventManager
        {
            get { return Instance.eventCommonManager; }
            private set { }
        }
    }
}