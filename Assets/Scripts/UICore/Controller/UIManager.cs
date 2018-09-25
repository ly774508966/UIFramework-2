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

        // Each UIType root
        private Transform _baseUIRoot;
        private Transform _popupUIRoot;
        private Transform _fixedUIRoot;
        private Transform _floatingUIRoot;



        private void Awake()
        {
            if (null == _allUIDic)
            {
                _allUIDic = new Dictionary<int, UIBase>();
            }
            if (null == _showUIDic)
            {
                _showUIDic = new Dictionary<int, UIBase>();
            }
            if (null == _backSequenceStack)
            {
                _backSequenceStack = new Stack<NavigationData>();
            }
            InitUIManager();
        }

        private void InitUIManager()
        {
            if (null != _allUIDic)
            {
                _allUIDic.Clear();
            }
            if (null != _showUIDic)
            {
                _showUIDic.Clear();
            }
            if (null != _backSequenceStack)
            {
                _backSequenceStack.Clear();
            }

            /*
            zyx_TODO_list
                1. root初始化&获取
            */

        }

        public void ShowUI(int uiID)
        {

        }

        public void HideUI(int uiID)
        {

        }
    }

}