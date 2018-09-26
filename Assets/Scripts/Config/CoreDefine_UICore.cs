/*****************************************************************************
 * filename :  CoreDefine_UICore.cs
 * author   :  Zhang Yunxing
 * date     :  2018/08/30 15:57
 * desc     :  UICore的一些枚举常量定义
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.CoreDefine
{
    public partial class CoreGlobeVar
    {
        public const int INVAILD_UIID = -1;

        public const string MASK_NAME = "UIMask";


        // 遮罩颜色常量管理
        public const float UIMASK_LUCENCY_COLOR_RGB = 255f / 255f;
        public const float UIMASK_LUCENCY_COLOR_RGB_A = 0f / 255f;

        public const float UIMASK_TRANSLUCENT_COLOR_RGB = 220f / 255f;
        public const float UIMASK_TRANSLUCENT_COLOR_RGB_A = 50f / 255f;

        public const float UIMASK_IMPENETRABLE_COLOR_RGB = 50f / 255f;
        public const float UIMASK_IMPENETRABLE_COLOR_RGB_A = 200f / 255f;
    }
}