using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.UICore;
using UnityEngine.EventSystems;
using Games.GameDefine;
public class CharacterUILogic : UIBase
{
    public GameObject PlayBtn;
    public GameObject BackBtn;
    public GameObject ArrowLeftBtn;
    public GameObject ArrowRightBtn;
    protected override void Awake()
    {
        base.Awake();
        this.infoData = UIInfos.CharacterUI;

        EventTriggerListener.Get(BackBtn).AddEvent(EventTriggerType.PointerClick, OnBackBtnClick);
        EventTriggerListener.Get(PlayBtn).AddEvent(EventTriggerType.PointerClick, OnPlayBtnClick);
    }

    private void OnDestroy()
    {
        EventTriggerListener.Get(BackBtn).RemoveEvent(EventTriggerType.PointerClick, OnBackBtnClick);
        EventTriggerListener.Get(PlayBtn).RemoveEvent(EventTriggerType.PointerClick, OnPlayBtnClick);
    }

    private void OnBackBtnClick(BaseEventData baseData)
    {
        UIManager.Instance.CloseUI(this.infoData);
    }
    private void OnPlayBtnClick(BaseEventData baseData)
    {
        SceneLoadHelper.Instance.LoadTargetScene(GameGlobeVar.LOADING_TESTSCENE_NAME);
    }
}