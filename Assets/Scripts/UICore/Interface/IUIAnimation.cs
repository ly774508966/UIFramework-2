/*****************************************************************************
 * filename :  IUIAnimation.cs
 * author   :  Zhang Yunxing
 * date     :  2018/10/08 20:28
 * desc     :  UI动画接口，具体动画使用dotween实现
 * changelog:  
*****************************************************************************/
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.UICore
{
    /// <summary>
    /// 窗口动画
    /// </summary>
    interface IUIAnimation
    {

        /// <summary>
        /// 显示动画
        /// </summary>
        /// <param name="onComplete"></param>
        void ShowAnimation(TweenCallback onComplete = null);

        /// <summary>
        /// 隐藏动画
        /// </summary>
        /// <param name="onComplete"></param>
        void HideAnimation(TweenCallback onComplete = null);
    }
}