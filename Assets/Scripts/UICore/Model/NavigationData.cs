/*****************************************************************************
 * filename :  NavigationData.cs
 * author   :  Zhang Yunxing
 * date     :  2018/09/20 20:39
 * desc     :  UI导航数据
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.UICore
{
    public class NavigationData : MonoBehaviour
    {
        public UIBase HideTargetUI;
        public List<int> backShowTargets;
    }

}