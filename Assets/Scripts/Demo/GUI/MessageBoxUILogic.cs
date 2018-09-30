using Games.UICore;
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

    protected override void Awake()
    {
        base.Awake();
        this.infoData = UIInfos.MessageBoxUI;
    }

    private void OnDestroy()
    {
    }

    private void OnMessageBoxCloaseClick(BaseEventData baseData)
    {
        UIManager.Instance.CloseUI(this.infoData);
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