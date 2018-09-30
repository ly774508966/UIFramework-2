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
    public class MessageBoxController
    {
        private static MessageBoxController Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public static void OpenOKBox(string textMsg, string titleMsg = "", DelOnOKClick delOnOkClick = null, string okBtnMsg = "")
        {

        }

        public static void OpenOKCancelBox(string textMsg, string titleMsg = "", DelOnOKClick delOnOkClick = null, DelOnCancelClick delOnCancelClick = null, string okBtnMsg = "", string cancelBBtnMsg = "")
        {

        }

        public static void OpenWaitBox(string textMsg, string titleMsg = "", float duration = 0.0f, float delay = 0.0f, DelOnWaitTimeOut delOnWaitTimeOut = null)
        {

        }

        public static void OpenOKCancelCountDownBox(string textMsg, string titleMsg = "", float countdown = 0.0f, DelOnOKClick delOnOkClick = null, DelOnCancelClick delOnCancelClick = null, DelOnCountDownOver delOnCountDownOver = null, string okBtnMsg = "", string cancelBBtnMsg = "")
        {

        }

        /// <summary>
        /// n秒倒计时后可点击Box，死亡复活界面等
        /// </summary>
        public static void OpenOKCancelCountDownEnableBox(string textMsg, string titleMsg = "", DelOnOKClick delOnOkClick = null, DelOnCancelClick delOnCancelClick = null, string okBtnMsg = "", string cancelBBtnMsg = "")
        {

        }

        public static void OpenOKCancelDynamicBox(string textMsg, string titleMsg = "", DelOnDynamicClick delOnDynamicClick = null, string dynamicMsg = "", DelOnOKClick delOnOkClick = null, DelOnCancelClick delOnCancelClick = null, string okBtnMsg = "", string cancelBBtnMsg = "")
        {

        }
    }
}