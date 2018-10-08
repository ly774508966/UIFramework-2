using DG.Tweening;
using Games.UICore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MessageBoxUILogic : UIBase, IUIAnimation
{
    public Text TitleText;
    public Text ContextText;
    public Text CancelBtnText;
    public Text OkBtnText;
    public Text DynamicBtnText;

    // UI Content
    public GameObject Content;
    public CanvasGroup ContentCanvas;

    // btn
    public GameObject OKBtn;
    public GameObject CancelBtn;
    public GameObject DynamicBtn;
    // public HorizontalLayoutGroup BtnLayoutGroup;

    // delegate
    private DelOnOKClick _delOK;
    private DelOnCancelClick _delCancel;
    private DelOnCountDownOver _delCountDownOver;
    private DelOnDynamicClick _delDynamic;
    private DelOnWaitTimeOut _delWaitTimeOut;

    // time
    private float _durationTime;
    private float _countdownTime;
    private float _oneSecondTime;
    private float _delayTime;

    // control
    private bool _isEnable;

    // messageBox Info
    private MessageBoxInfo _messageBoxInfo;

    // tween
    private Sequence _seqTweener;

    protected override void Awake()
    {
        base.Awake();
        this.infoData = UIInfos.MessageBoxUI;
        CleanInfo();

        _seqTweener = DOTween.Sequence();
        _seqTweener.Append(DOTween.To(() => ContentCanvas.alpha, target => ContentCanvas.alpha = target, 1, 0.3f));

        EventTriggerListener.Get(OKBtn).AddEvent(EventTriggerType.PointerClick, OnOKBtnClick);
        EventTriggerListener.Get(CancelBtn).AddEvent(EventTriggerType.PointerClick, OnCancelBtnClick);
        EventTriggerListener.Get(DynamicBtn).AddEvent(EventTriggerType.PointerClick, OnDynamicBtnClick);
    }

    private void Update()
    {
        Tick_WaitBox();
        Tick_CountDownBox();
    }
    private void OnDestroy()
    {
        EventTriggerListener.Get(OKBtn).RemoveEvent(EventTriggerType.PointerClick, OnOKBtnClick);
        EventTriggerListener.Get(CancelBtn).RemoveEvent(EventTriggerType.PointerClick, OnCancelBtnClick);
        EventTriggerListener.Get(DynamicBtn).RemoveEvent(EventTriggerType.PointerClick, OnDynamicBtnClick);
    }

    public override void ShowUI(DelOnCompleteShowUI onComplete = null, object param = null)
    {
        base.ShowUI(onComplete, param);
        ShowAnimation();
        _messageBoxInfo = param as MessageBoxInfo;
        if (null == _messageBoxInfo)
        {
            return;
        }
        switch (_messageBoxInfo.MessageType)
        {
            case MESSAGEBOX_TYPE.TYPE_OK:
                OnOpenOkBox();
                break;
            case MESSAGEBOX_TYPE.TYPE_OKCANCEL:
                OnOpenOKCancelBox();
                break;
            case MESSAGEBOX_TYPE.TYPE_WAIT:
                OnOpenWaitBox();
                break;
            case MESSAGEBOX_TYPE.TYPE_OKCANCELCOUNTDOWN:
                OnOpenOKCancelCountDownBox();
                break;
            case MESSAGEBOX_TYPE.TYPE_OKCANCELCOUNTDOWNENABLE:
                OnOpenOKCancelCountDownEnableBox();
                break;
            case MESSAGEBOX_TYPE.TYPE_OKCANCELDYNAMIC:
                OnOpenOKCancelDynamicBox();
                break;
            default:
                break;
        }
    }

    private void CleanInfo()
    {
        _delOK = null;
        _delCancel = null;
        _delCountDownOver = null;
        _delDynamic = null;
        _delWaitTimeOut = null;
        _messageBoxInfo = null;
        _durationTime = _delayTime = -1.0f;
        _oneSecondTime = 0.0f;
        _isEnable = true;
        TitleText.text = "";
        ContextText.text = "";
        CancelBtnText.text = "";
        OkBtnText.text = "";
        DynamicBtnText.text = "";
    }

    #region diff message box logic
    private void OnOpenOkBox()
    {
        OKBtn.SetActive(true);
        CancelBtn.SetActive(false);
        DynamicBtn.SetActive(false);
        ContextText.text = _messageBoxInfo.TextMsg;
        TitleText.text = _messageBoxInfo.TitleMsg;
        OkBtnText.text = _messageBoxInfo.OKBtnMsg;
        _delOK = _messageBoxInfo.OnOKClickEvent;
    }

    private void OnOpenOKCancelBox()
    {
        OKBtn.SetActive(true);
        CancelBtn.SetActive(true);
        DynamicBtn.SetActive(false);
        ContextText.text = _messageBoxInfo.TextMsg;
        TitleText.text = _messageBoxInfo.TitleMsg;
        OkBtnText.text = _messageBoxInfo.OKBtnMsg;
        CancelBtnText.text = _messageBoxInfo.CancelBtnMsg;
        _delOK = _messageBoxInfo.OnOKClickEvent;
        _delCancel = _messageBoxInfo.OnCancelClickEvent;
    }

    private void OnOpenWaitBox()
    {
        OKBtn.SetActive(false);
        CancelBtn.SetActive(false);
        DynamicBtn.SetActive(false);
        ContextText.text = _messageBoxInfo.TextMsg;
        TitleText.text = _messageBoxInfo.TitleMsg;
        _delWaitTimeOut = _messageBoxInfo.OnWaitTimeOutEvent;
        _durationTime = _messageBoxInfo.DuratonTime;
        _delayTime = _messageBoxInfo.DelayTime;
        if (_delayTime > 0)
        {
            if (null != _mask)
            {
                _mask.gameObject.SetActive(false);
            }
            Content.SetActive(false);
        }
    }

    private void OnOpenOKCancelCountDownBox()
    {
        OKBtn.SetActive(true);
        CancelBtn.SetActive(true);
        DynamicBtn.SetActive(false);
        ContextText.text = _messageBoxInfo.TextMsg;
        TitleText.text = _messageBoxInfo.TitleMsg;
        OkBtnText.text = _messageBoxInfo.OKBtnMsg;
        CancelBtnText.text = _messageBoxInfo.CancelBtnMsg;
        _delOK = _messageBoxInfo.OnOKClickEvent;
        _delCancel = _messageBoxInfo.OnCancelClickEvent;
        _delCountDownOver = _messageBoxInfo.OnCountDownOverEvent;
        _countdownTime = _messageBoxInfo.DuratonTime;
        if (_countdownTime > 0)
        {
            OkBtnText.text = _messageBoxInfo.OKBtnMsg + "(" + ((int)_countdownTime).ToString() + ")s";
        }
    }

    private void OnOpenOKCancelCountDownEnableBox()
    {
        OKBtn.SetActive(true);
        CancelBtn.SetActive(true);
        DynamicBtn.SetActive(false);
        ContextText.text = _messageBoxInfo.TextMsg;
        TitleText.text = _messageBoxInfo.TitleMsg;
        OkBtnText.text = _messageBoxInfo.OKBtnMsg;
        CancelBtnText.text = _messageBoxInfo.CancelBtnMsg;
        _delOK = _messageBoxInfo.OnOKClickEvent;
        _delCancel = _messageBoxInfo.OnCancelClickEvent;
        _delCountDownOver = _messageBoxInfo.OnCountDownOverEvent;
        _countdownTime = _messageBoxInfo.DuratonTime;
        if (_countdownTime > 0)
        {
            _isEnable = false;
            OkBtnText.text = _messageBoxInfo.OKBtnMsg + "(" + ((int)_countdownTime).ToString() + ")s";
        }
    }

    private void OnOpenOKCancelDynamicBox()
    {
        OKBtn.SetActive(true);
        CancelBtn.SetActive(true);
        DynamicBtn.SetActive(true);
        ContextText.text = _messageBoxInfo.TextMsg;
        TitleText.text = _messageBoxInfo.TitleMsg;
        OkBtnText.text = _messageBoxInfo.OKBtnMsg;
        CancelBtnText.text = _messageBoxInfo.CancelBtnMsg;
        DynamicBtnText.text = _messageBoxInfo.DynamicBtnMsg;
        _delOK = _messageBoxInfo.OnOKClickEvent;
        _delCancel = _messageBoxInfo.OnCancelClickEvent;
        _delDynamic = _messageBoxInfo.OnDynamicClickEvent;
    }
    #endregion

    private void Tick_WaitBox()
    {
        if (_delayTime > 0)
        {
            _delayTime -= Time.deltaTime;
            if (_delayTime <= 0)
            {
                if (null != _mask)
                {
                    _mask.gameObject.SetActive(false);
                }
                Content.SetActive(true);
            }
            return;
        }
        if (_durationTime > 0)
        {
            _durationTime -= Time.deltaTime;
            if (_durationTime <= 0)
            {
                UIManager.CloseUI(UIInfos.MessageBoxUI);
                if (null != _delWaitTimeOut)
                {
                    _delWaitTimeOut();
                }
            }
        }
    }

    private void Tick_CountDownBox()
    {
        if (_countdownTime > 0)
        {
            _oneSecondTime += Time.deltaTime;
            if (_oneSecondTime >= 1.0f)
            {
                _countdownTime -= _oneSecondTime;
                if (_countdownTime <= 0)
                {
                    _countdownTime = 0.0f;
                    OkBtnText.text = _messageBoxInfo.OKBtnMsg;
                    if (false == _isEnable)
                    {
                        _isEnable = true;
                    }
                    if (null != _delCountDownOver)
                    {
                        _delCountDownOver();
                    }
                    return;
                }
                OkBtnText.text = _messageBoxInfo.OKBtnMsg + "(" + ((int)_countdownTime).ToString() + ")s";
                _oneSecondTime = 0.0f;
            }
        }
    }

    private void OnOKBtnClick(BaseEventData baseData)
    {
        if (false == _isEnable)
        {
            return;
        }
        if (null != _delOK)
        {
            _delOK();
        }
    }

    private void OnCancelBtnClick(BaseEventData baseData)
    {
        if (null != _delCancel)
        {
            _delOK();
        }
    }

    private void OnDynamicBtnClick(BaseEventData baseData)
    {
        if (null != _delDynamic)
        {
            _delDynamic();
        }
    }

    private void OnMessageBoxCloaseClick(BaseEventData baseData)
    {
        UIManager.CloseUI(this.infoData);
    }

    public override void HideUI(DelOnCompleteHideUI onComplete = null)
    {
        HideAnimation(delegate
            {
                base.HideUI(onComplete);
                Debug.Log("Animation is over!");
            });
    }

    protected override void SetUIMaskEvent()
    {
        base.SetUIMaskEvent();
        if (null != this._mask)
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

    public void ShowAnimation(TweenCallback onComplete = null)
    {
        // ContentCanvas.DOPlayForward();
        _seqTweener.PlayForward();
    }

    public void HideAnimation(TweenCallback onComplete = null)
    {
        Tweener temp = DOTween.To(() => ContentCanvas.alpha, target => ContentCanvas.alpha = target, 0.2f, 0.3f);
        if (null != onComplete)
        {
            temp.onComplete = onComplete;
        }
        temp.PlayForward();
    }
}