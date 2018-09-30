using Games.UICore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MessageBoxUILogic : UIBase
{
    public Text TitleText;
    public Text ContextText;
    public Text CancelBtnText;
    public Text OkBtnText;
    public Text DynamicBtnText;

    // btn
    public GameObject OKBtn;
    public GameObject CancelBtn;
    public GameObject DynamicBtn;
    public HorizontalLayoutGroup BtnLayoutGroup;

    // delegate
    private DelOnOKClick deleOK;
    private DelOnCancelClick deleCancel;
    private DelOnCountDownOver deleCountDownOver;
    private DelOnDynamicClick deleDynamic;
    private DelOnWaitTimeOut delWaitTimeOut;

    protected override void Awake()
    {
        base.Awake();
        this.infoData = UIInfos.MessageBoxUI;
        deleOK = null;
        deleCancel = null;
        deleCountDownOver = null;
        deleDynamic = null;
        delWaitTimeOut = null;
    }

    private void OnDestroy()
    {
    }

    #region set text msg
    public void SetTitleMsg(string msg)
    {
        TitleText.text = msg;
    }

    public void SetContentMsg(string msg)
    {
        ContextText.text = msg;
    }

    public void SetCancelMsg(string msg)
    {
        CancelBtnText.text = msg;
    }

    public void SetOKMsg(string msg)
    {
        OkBtnText.text = msg;
    }

    public void SetDynamicMsg(string msg)
    {
        DynamicBtnText.text = msg;
    }
    #endregion
    
    public void SetCancelBtnCallBackEvent(DelOnCancelClick del)
    {
        deleCancel = del;
    }


    private void OnMessageBoxCloaseClick(BaseEventData baseData)
    {
        UIManager.CloseUI(this.infoData);
    }

    protected override void SetUIMaskEvent()
    {
        base.SetUIMaskEvent();
        if (null != Mask)
        {
            EventTriggerListener.Get(this._mask.gameObject).AddEvent(EventTriggerType.PointerClick, OnMessageBoxCloaseClick);
        }
    }

    protected override void RemoveUIMaskEvent()
    {
        base.RemoveUIMaskEvent();
        if (null != this._mask)
        {
            EventTriggerListener.Get(this._mask.gameObject).RemoveEvent(EventTriggerType.PointerClick, OnMessageBoxCloaseClick);
        }
    }
}