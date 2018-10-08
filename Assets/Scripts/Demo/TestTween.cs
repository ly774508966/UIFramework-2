using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Games.UICore;
using Games.Event;
using Games.GameDefine;

public class TestTween : MonoBehaviour
{
    private Tweener tweener;

    private void Awake()
    {
    }

    // Event Test
    private void OnEnable()
    {
        RegisterEvent();
    }

    private void OnDisable()
    {
        UnRegisterEvent();
    }

    public void RegisterEvent()
    {
        EventDispatcher.MainEventManager.AddEventListener<int, int>(GameGlobeVar.EventId.TestEvent, TestMessageBox);
    }


    public void UnRegisterEvent()
    {
        EventDispatcher.MainEventManager.RemoveEventListener<int, int>(GameGlobeVar.EventId.TestEvent, TestMessageBox);
    }

    private void TestMessageBox(int mobile, int unicom)
    {
        Debug.Log("[China Mobile phone number:" + mobile + "][China Unicom phone number: " + unicom + "]");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            float num = 3;
            DOTween.To(() => num, x => num = x, 5, 1);
            tweener = transform.DOMove(new Vector3(3, 0, 0), 5);
        }
        if (Input.GetKey(KeyCode.B))
        {
            EventTriggerListener.Get(this.gameObject).AddEvent(EventTriggerType.PointerClick, OnButtonClick);
            Debug.Log("Event Binding!");
        }
        if (Input.GetKey(KeyCode.R))
        {
            EventTriggerListener.Get(this.gameObject).RemoveEvent(EventTriggerType.PointerClick, OnButtonClick);
            Debug.Log("Event Removed!");
        }
        if (Input.GetKey(KeyCode.D))
        {
            tweener.Flip();
            Debug.Log("Tween Reset!");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventDispatcher.MainEventManager.TriggerEvent<int, int>(GameGlobeVar.EventId.TestEvent, 10086, 10010);
        }
    }

    private void OnButtonClick(BaseEventData baseData)
    {
        Debug.Log("Button Clicked!");
    }
}