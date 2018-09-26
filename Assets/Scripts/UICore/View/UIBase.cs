/*****************************************************************************
 * filename :  UIBase.cs
 * author   :  Zhang Yunxing
 * date     :  2018/09/20 11:34
 * desc     :  UI基础类
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.CoreDefine;

namespace Games.UICore
{
    public class UIBase : MonoBehaviour
    {
        public UIInfoData infoData = UIInfos.defaultUI;

        protected RectTransform _mask;
        public RectTransform Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }
        protected RectTransform _mTrans;
        public RectTransform Trans
        {
            get { return _mTrans; }
        }

        // 前置UIID
        protected int _preuiid = CoreGlobeVar.INVAILD_UIID;

        // 当前UIID
        private int _uiid = CoreGlobeVar.INVAILD_UIID;

        /// <summary>
        ///  是否加入反向导航序列
        /// </summary>
        public bool IsAddedToBackSequence
        {
            get { return this.infoData.CoreData.NavigationMode == UINavigationMode.NormalNavigation; }
        }

        /// <summary>
        /// 是否刷新导航序列
        /// </summary>
        public bool IsRefreshBackSequenceData
        {
            get { return this.infoData.CoreData.NavigationMode == UINavigationMode.IngoreNavigation; }
        }

        protected virtual void Awake()
        {
            this.gameObject.SetActive(true);
            _mTrans = this.gameObject.GetComponent<RectTransform>();
        }

        public virtual void ShowUI()
        {
            gameObject.SetActive(true);
        }

        public virtual void HideUI()
        {

        }

        public virtual void DestoryUI()
        {

        }

        /// <summary>
        /// 添加碰撞体到UI的背景上
        /// UI背景的碰撞体事件
        /// </summary>
        /// <param name="go"></param>
        public virtual void OnAddColliderBg(GameObject go)
        {

        }
    }
}
