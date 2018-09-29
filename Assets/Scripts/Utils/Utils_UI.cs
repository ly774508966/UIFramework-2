/*****************************************************************************
 * filename :  Utils_UI.cs
 * author   :  Zhang Yunxing
 * date     :  2018/09/28 21:06
 * desc     :  UI Utils
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Games.CoreDefine;
using Games.UICore;

public partial class Utils
{
    /// <summary>
    /// 向对应UI物体添加空白子物体，默认拉伸平铺布局
    /// </summary>
    /// <param name="parientTrns"></param>
    /// <param name="rootName"></param>
    /// <returns></returns>
    public static RectTransform AddUINullChild(RectTransform parientTrns, string rootName)
    {
        if(null == parientTrns)
        {
            return null;
        }
        GameObject tempRoot = new GameObject(rootName);
        RectTransform rectTrans = tempRoot.AddComponent<RectTransform>();
        rectTrans.SetParent(parientTrns);
        rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
        rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
        rectTrans.anchorMin = Vector2.zero;
        rectTrans.anchorMax = Vector2.one;
        rectTrans.localPosition = Vector3.zero;
        rectTrans.localScale = Vector3.one;
        rectTrans.localEulerAngles = Vector3.zero;
        Utils.SetLayer(parientTrns.gameObject.layer, rectTrans);
        return rectTrans;
    }


    /// <summary>
    /// 设置UI遮罩
    /// </summary>
    /// <param name="showUI"></param>
    /// <param name="rootTrans"></param>
    public static void AddUIMask(UIBase showUI, RectTransform rootTrans)
    {
        if (null == showUI.infoData)
        {
            return;
        }
        if (UIColliderType.Penetrate == showUI.infoData.CoreData.ColliderType)
        {
            return;
        }
        Image maskImage = null;
        if (null == showUI.Mask)
        {
            RectTransform mask = AddUINullChild(rootTrans, "uiMask");
            maskImage = Utils.TryAddComponent<Image>(mask.gameObject);
            if(null == maskImage)
            {
                return;
            }
            showUI.Mask = mask;
        }
        switch (showUI.infoData.CoreData.ColliderType)
        {
            case UIColliderType.Lucency:
                showUI.Mask.gameObject.GetComponent<Image>().color = new Color(CoreGlobeVar.UIMASK_LUCENCY_COLOR_RGB, CoreGlobeVar.UIMASK_LUCENCY_COLOR_RGB,
                    CoreGlobeVar.UIMASK_LUCENCY_COLOR_RGB, CoreGlobeVar.UIMASK_LUCENCY_COLOR_RGB_A);
                break;
            case UIColliderType.Translucent:
                showUI.Mask.gameObject.GetComponent<Image>().color = new Color(CoreGlobeVar.UIMASK_TRANSLUCENT_COLOR_RGB,
                    CoreGlobeVar.UIMASK_TRANSLUCENT_COLOR_RGB, CoreGlobeVar.UIMASK_TRANSLUCENT_COLOR_RGB, CoreGlobeVar.UIMASK_TRANSLUCENT_COLOR_RGB_A);
                break;
            case UIColliderType.ImPenetrable:
                showUI.Mask.gameObject.GetComponent<Image>().color = new Color(CoreGlobeVar.UIMASK_IMPENETRABLE_COLOR_RGB, CoreGlobeVar.UIMASK_IMPENETRABLE_COLOR_RGB,
                    CoreGlobeVar.UIMASK_IMPENETRABLE_COLOR_RGB, CoreGlobeVar.UIMASK_IMPENETRABLE_COLOR_RGB_A);
                break;
            default:
                break;
        }
    }
}
