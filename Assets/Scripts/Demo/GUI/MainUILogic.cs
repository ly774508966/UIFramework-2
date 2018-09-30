using Games.UICore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainUILogic : UIBase
{
    public GameObject LoginBtn;
    protected override void Awake()
    {
        base.Awake();
        this.infoData = UIInfos.MainUI;

        EventTriggerListener.Get(LoginBtn).AddEvent(EventTriggerType.PointerClick, OnLoginBtnClick);
    }

    private void OnDestroy()
    {
        EventTriggerListener.Get(LoginBtn).RemoveEvent(EventTriggerType.PointerClick, OnLoginBtnClick);
    }

    private void OnLoginBtnClick(BaseEventData baseData)
    {
        UIManager.ShowUI(UIInfos.CharacterUI);
    }
}
