/*****************************************************************************
 * filename :  MessageBoxController.cs
 * author   :  Zhang Yunxing
 * date     :  2018/09/30 10:56
 * desc     :  MessageBox 控制逻辑
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.UICore
{
    public delegate void DelOnOKClick();
    public delegate void DelOnCancelClick();
    public delegate void DelOnCountDownOver();
    public delegate void DelOnDynamicClick();
    public delegate void DelOnWaitTimeOut();
    public enum MESSAGEBOX_TYPE
    {
        TYPE_INVALID = -1,
        TYPE_OK = 0,
        TYPE_OKCANCEL = 1,
        TYPE_WAIT = 2,
        TYPE_OKCANCELCOUNTDOWN = 3,
        TYPE_OKCANCELCOUNTDOWNENABLE = 4,
        TYPE_OKCANCELDYNAMIC = 5,
        TYPE_MAX,
    }
    public class MessageBoxInfo
    {
        public MESSAGEBOX_TYPE MessageType;
        public string TextMsg;
        public string TitleMsg;
        public string OKBtnMsg;
        public string CancelBtnMsg;
        public string DynamicBtnMsg;
        public float DuratonTime;
        public float DelayTime;
        public DelOnOKClick OnOKClickEvent;
        public DelOnCancelClick OnCancelClickEvent;
        public DelOnCountDownOver OnCountDownOverEvent;
        public DelOnDynamicClick OnDynamicClickEvent;
        public DelOnWaitTimeOut OnWaitTimeOutEvent;
        public MessageBoxInfo(MESSAGEBOX_TYPE type, string textMsg, string titleMsg, string okBtnMsg, string cancelBtnMsg,
            string dynamicBtnMsg, float durationTime, float delayTime, DelOnOKClick onOKClickEvent, DelOnCancelClick onCancelClickEvent,
            DelOnCountDownOver onCountDownOverEvent, DelOnDynamicClick onDynamicClickEvent, DelOnWaitTimeOut onWaitTimeOutEvent)
        {
            MessageType = type;
            TextMsg = textMsg;
            TitleMsg = titleMsg;
            OKBtnMsg = okBtnMsg;
            CancelBtnMsg = cancelBtnMsg;
            DynamicBtnMsg = dynamicBtnMsg;
            DuratonTime = durationTime;
            DelayTime = delayTime;
            OnOKClickEvent = onOKClickEvent;
            OnCancelClickEvent = onCancelClickEvent;
            OnCountDownOverEvent = onCountDownOverEvent;
            OnDynamicClickEvent = onDynamicClickEvent;
            OnWaitTimeOutEvent = onWaitTimeOutEvent;
        }
    }

    public class MessageBoxController
    {
        public static void OpenOKBox(string textMsg, string titleMsg = "", DelOnOKClick delOnOkClick = null, string okBtnMsg = "OK")
        {
            MessageBoxInfo curInfo = new MessageBoxInfo(MESSAGEBOX_TYPE.TYPE_OK, textMsg, titleMsg, okBtnMsg, "", "", -1.0f, -1.0f, delOnOkClick, null, null, null, null);
            UIManager.ShowUI(UIInfos.MessageBoxUI, null, curInfo);
        }

        public static void OpenOKCancelBox(string textMsg, string titleMsg = "", DelOnOKClick delOnOkClick = null, DelOnCancelClick delOnCancelClick = null, string okBtnMsg = "OK", string cancelBtnMsg = "CANCEL")
        {
            MessageBoxInfo curInfo = new MessageBoxInfo(MESSAGEBOX_TYPE.TYPE_OKCANCEL, textMsg, titleMsg, okBtnMsg, cancelBtnMsg, "", -1.0f, -1.0f, delOnOkClick, delOnCancelClick, null, null, null);
            UIManager.ShowUI(UIInfos.MessageBoxUI, null, curInfo);
        }

        public static void OpenWaitBox(string textMsg, string titleMsg = "", float duration = 0.0f, float delay = 0.0f, DelOnWaitTimeOut delOnWaitTimeOut = null)
        {
            MessageBoxInfo curInfo = new MessageBoxInfo(MESSAGEBOX_TYPE.TYPE_WAIT, textMsg, titleMsg, "", "", "", duration, delay, null, null, null, null, delOnWaitTimeOut);
            UIManager.ShowUI(UIInfos.MessageBoxUI, null, curInfo);
        }

        public static void OpenOKCancelCountDownBox(string textMsg, string titleMsg = "", float countdown = 0.0f, DelOnOKClick delOnOkClick = null, DelOnCancelClick delOnCancelClick = null, DelOnCountDownOver delOnCountDownOver = null, string okBtnMsg = "OK", string cancelBtnMsg = "CANCEL")
        {
            MessageBoxInfo curInfo = new MessageBoxInfo(MESSAGEBOX_TYPE.TYPE_OKCANCELCOUNTDOWN, textMsg, titleMsg, okBtnMsg, cancelBtnMsg, "", countdown, -1.0f, delOnOkClick, delOnCancelClick, delOnCountDownOver, null, null);
            UIManager.ShowUI(UIInfos.MessageBoxUI, null, curInfo);
        }

        /// <summary>
        /// n秒倒计时后可点击Box，死亡复活界面等
        /// </summary>
        public static void OpenOKCancelCountDownEnableBox(string textMsg, string titleMsg = "", float countdown = 0.0f, DelOnOKClick delOnOkClick = null, DelOnCancelClick delOnCancelClick = null, DelOnCountDownOver delOnCountDownOver = null, string okBtnMsg = "OK", string cancelBtnMsg = "CANCEL")
        {
            MessageBoxInfo curInfo = new MessageBoxInfo(MESSAGEBOX_TYPE.TYPE_OKCANCELCOUNTDOWNENABLE, textMsg, titleMsg, okBtnMsg, cancelBtnMsg, "", countdown, -1.0f, delOnOkClick, delOnCancelClick, delOnCountDownOver, null, null);
            UIManager.ShowUI(UIInfos.MessageBoxUI, null, curInfo);
        }

        public static void OpenOKCancelDynamicBox(string textMsg, string titleMsg = "", DelOnDynamicClick delOnDynamicClick = null, string dynamicMsg = "", DelOnOKClick delOnOkClick = null, DelOnCancelClick delOnCancelClick = null, string okBtnMsg = "OK", string cancelBtnMsg = "CANCEL")
        {
            MessageBoxInfo curInfo = new MessageBoxInfo(MESSAGEBOX_TYPE.TYPE_OKCANCELDYNAMIC, textMsg, titleMsg, okBtnMsg, cancelBtnMsg, dynamicMsg, -1.0f, -1.0f, delOnOkClick, delOnCancelClick, null, delOnDynamicClick, null);
            UIManager.ShowUI(UIInfos.MessageBoxUI, null, curInfo);
        }
    }
}