using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.CoreDefine;

namespace Games.UICore
{
    public class UIManager : MonoBehaviour
    {
        private Dictionary<int, UIBase> _allUIDic;
        private Dictionary<int, UIBase> _showUIDic;
        private Stack<NavigationData> _backSequenceStack;

        private int _curUIID = CoreGlobeVar.INVAILD_UIID;
        private int _preUIiD = CoreGlobeVar.INVAILD_UIID;


        /// <summary>
        /// 场景变更时处理相关UI
        /// </summary>
        public void OnSenceChange()
        {

        }
    }

}