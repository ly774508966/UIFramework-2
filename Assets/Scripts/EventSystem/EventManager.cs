/*****************************************************************************
 * filename :  EventManager.cs
 * author   :  Zhang Yunxing
 * date     :  2018/10/08 17:52
 * desc     :  事件管理中心管理程序逻辑
 * changelog:  
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GameDefine;
using Games.CoreDefine;
namespace Games.Event
{
    public class EventManager
    {
        private Dictionary<int, Delegate> _dicEvents = new Dictionary<int, Delegate>();

        /// <summary>
        /// EventLog日志输出
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="handleType"></param>
        /// <param name="targetEventType"></param>
        /// <param name="listener"></param>
        private void LogTypeError(GameGlobeVar.EventId eventId, CoreGlobeVar.HandleType handleType, Delegate targetEventType, Delegate listener)
        {
            LogModule.LogError(string.Format("Event Id {0}, [{1}] Wrong Listener Type {2}, needed Type {3}.",
                eventId.ToString(),
                CoreGlobeVar.HandleTypeDic[(int)handleType],
                targetEventType.GetType(),
                listener.GetType()));
        }

        /// <summary>
        /// 事件添加条件检测
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        private bool CheckAddEventListener(GameGlobeVar.EventId eventId, Delegate listener)
        {
            if (!this._dicEvents.ContainsKey((int)eventId))
            {
                this._dicEvents.Add((int)eventId, null);
            }
            Delegate tmpDelegate = this._dicEvents[(int)eventId];
            if (null != tmpDelegate && listener.GetType() != tmpDelegate.GetType())
            {
                LogTypeError(eventId, CoreGlobeVar.HandleType.Add, _dicEvents[(int)eventId], listener);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 事件移除条件检测
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        private bool CheckRemoveEventListener(GameGlobeVar.EventId eventId, Delegate listener)
        {
            if (!_dicEvents.ContainsKey((int)eventId))
            {
                return false;
            }
            Delegate tmpDel = _dicEvents[(int)eventId];
            if (null != tmpDel && listener.GetType() != tmpDel.GetType())
            {
                LogTypeError(eventId, CoreGlobeVar.HandleType.Remove, _dicEvents[(int)eventId], listener);
                return false;
            }
            return true;
        }

        #region Void
        public void AddEventListener(GameGlobeVar.EventId eventId, Action listener)
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = this._dicEvents[(int)eventId];
                _dicEvents[(int)eventId] = (Action)Delegate.Combine((Action)del, listener);
            }
        }

        public void RemoveEventListener(GameGlobeVar.EventId eventId, Action listener)
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int)eventId];
                _dicEvents[(int)eventId] = Delegate.Remove(del, listener);
            }
        }

        public void TriggerEvent(GameGlobeVar.EventId eventId)
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int)eventId, out del))
            {
                if (null == del)
                {
                    return;
                }
                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; ++i)
                {
                    Action action = invocationList[i] as Action;
                    if (null == action)
                    {
                        LogModule.LogError(string.Format("## Trigger Event {0} Parameters type [void] are not match  target type : {1}.",
                            eventId.ToString(),
                            invocationList[i].GetType()));
                        return;
                    }
                    action();
                }
            }
        }
        #endregion


        #region One param
        public void AddEventListener<T>(GameGlobeVar.EventId eventId, Action<T> listener)
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int)eventId];
                _dicEvents[(int)eventId] = (Action<T>)Delegate.Combine((Action<T>)del, listener);
            }
        }

        public void RemoveEventListener<T>(GameGlobeVar.EventId eventId, Action<T> listener)
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int)eventId];
                _dicEvents[(int)eventId] = Delegate.Remove(del, listener);
            }
        }

        public void TriggerEvent<T>(GameGlobeVar.EventId eventId, T p)
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int)eventId, out del))
            {
                if (null == del)
                {
                    return;
                }
                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; ++i)
                {
                    Action<T> action = invocationList[i] as Action<T>;
                    if (null == action)
                    {
                        LogModule.LogError(string.Format("## Trigger Event {0} Parameters type [ {1} ] are not match  target type : {2}. ",
                            eventId.ToString(),
                            p.GetType(),
                            invocationList[i].GetType()));
                        return;
                    }
                    action(p);
                }
            }
        }
        #endregion


        #region Two params
        public void AddEventListener<T0, T1>(GameGlobeVar.EventId eventId, Action<T0, T1> listener)
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int)eventId];
                _dicEvents[(int)eventId] = (Action<T0, T1>)Delegate.Combine((Action<T0, T1>)del, listener);
            }
        }

        public void RemoveEventListener<T0, T1>(GameGlobeVar.EventId eventId, Action<T0, T1> listener)
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int)eventId];
                _dicEvents[(int)eventId] = Delegate.Remove(del, listener);
            }
        }

        public void TriggerEvent<T0, T1>(GameGlobeVar.EventId eventId, T0 p0, T1 p1)
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int)eventId, out del))
            {
                if (null == del)
                {
                    return;

                }
                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; ++i)
                {
                    Action<T0, T1> action = invocationList[i] as Action<T0, T1>;
                    if (null == action)
                    {
                        LogModule.LogError(string.Format("## Trigger Event {0} Parameters type [ {1}, {2}] are not match  target type : {3}.",
                            eventId.ToString(),
                            p0.GetType(),
                            p1.GetType(),
                            invocationList[i].GetType()));
                        return;
                    }
                    action(p0, p1);
                }
            }
        }
        #endregion


        #region Thress params
        public void AddEventListener<T0, T1, T2>(GameGlobeVar.EventId eventId, Action<T0, T1, T2> listener)
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int)eventId];
                _dicEvents[(int)eventId] = (Action<T0, T1, T2>)Delegate.Combine((Action<T0, T1, T2>)del, listener);
            }
        }

        public void RemoveEventListener<T0, T1, T2>(GameGlobeVar.EventId eventId, Action<T0, T1, T2> listener)
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int)eventId];
                _dicEvents[(int)eventId] = Delegate.Remove(del, listener);
            }
        }

        public void TriggerEvent<T0, T1, T2>(GameGlobeVar.EventId eventId, T0 p0, T1 p1, T2 p2)
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int)eventId, out del))
            {
                if (null == del)
                {
                    return;
                }
                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; ++i)
                {
                    Action<T0, T1, T2> action = invocationList[i] as Action<T0, T1, T2>;
                    if (null == action)
                    {
                        LogModule.LogError(string.Format("## Trigger Event {0} Parameters type [{1}, {2}, {3}] are not match  target type : {4}.",
                            eventId.ToString(),
                            p0.GetType(),
                            p1.GetType(),
                            p2.GetType(),
                            invocationList[i].GetType()));
                        return;
                    }
                    action(p0, p1, p2);
                }
            }
        }
        #endregion
    }
}