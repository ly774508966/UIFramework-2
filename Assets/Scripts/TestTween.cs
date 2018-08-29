using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TestTween : MonoBehaviour
{
    private Tweener tweener;
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
            tweener.Flip();
        }
        //Debug.logger.logEnabled = false;
    }

}