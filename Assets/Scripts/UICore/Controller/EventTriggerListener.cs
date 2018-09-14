/*****************************************************************************
 * filename :  EventTriggerListener.cs
 * author   :  Zhang Yunxing
 * date     :  2018/08/30 15:28
 * desc     :  UGUI 事件监听类
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace Games.UICore
{
    public class EventTriggerListener : EventTrigger
    {
        /// <summary>
        /// 添加EventTriggerListener组件到go上
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static EventTriggerListener Get(GameObject go)
        {
            EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
            if (listener == null)
            {
                listener = go.AddComponent<EventTriggerListener>();
            }
            return listener;
        }

        /// <summary>
        /// 添加对应类型的UI监听事件
        /// </summary>
        /// <param name="eventTriggerType"></param>
        /// <param name="action"></param>
        public void AddEvent(EventTriggerType eventTriggerType, UnityAction<BaseEventData> action)
        {
            // 在EventSystem委托列表中进行登记  
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventTriggerType;
            entry.callback.AddListener(action);
            // 实例化delegates(listener.trigger是注册在EventTrigger组件上的所有功能)  
            if (null == triggers)
            {
                triggers = new List<EventTrigger.Entry>();
            }
            triggers.Add(entry);
        }

        /// <summary>
        /// 移除对应类型的监听事件
        /// </summary>
        /// <param name="eventTriggerType"></param>
        /// <param name="action"></param>
        public void RemoveEvent(EventTriggerType eventTriggerType, UnityAction<BaseEventData> action)
        {
            if (null == triggers)
            {
                return;
            }
            foreach (Entry entry in triggers)
            {
                if (entry.eventID == eventTriggerType)
                {
                    entry.callback.RemoveListener(action);
                    break;
                }
            }
        }
    }
}