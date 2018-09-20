using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Games.UICore;

public class TestTween : MonoBehaviour
{
    private Tweener tweener;

    private void Awake()
    {
        EventTriggerListener.Get(this.gameObject).AddEvent(EventTriggerType.PointerClick, OnButtonClick);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            float num = 3;
            DOTween.To(() => num, x => num = x, 5, 1);
            tweener = transform.DOMove(new Vector3(3, 0, 0), 5);
        }
        if (Input.GetKey(KeyCode.F))
        {
            EventTriggerListener.Get(this.gameObject).RemoveEvent(EventTriggerType.PointerClick, OnButtonClick);
            Debug.Log("Event Removed!");
            tweener.Flip();
        }
        //Debug.logger.logEnabled = false;
    }

    private void OnButtonClick(BaseEventData baseData)
    {
        Debug.Log("Button Clicked!");
    }
}