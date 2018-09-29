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
        public UIInfoData infoData = UIInfos.DefaultUI;

        protected RectTransform _mask;
        public RectTransform Mask
        {
            get
            {
                return _mask;
            }
            set
            {
                _mask = value;
            }
        }
        protected RectTransform _mTrans;
        public RectTransform Trans
        {
            get
            {
                return _mTrans;
            }
        }

        // 前置UIInfoData
        protected UIInfoData _preUIInfo = null;
        public UIInfoData PreUIInfo
        {
            get
            {
                return _preUIInfo;
            }
            set
            {
                _preUIInfo = value;
            }
        }

        /// <summary>
        ///  是否加入反向导航序列
        /// </summary>
        public bool IsAddedToBackSequence
        {
            get
            {
                return this.infoData.CoreData.NavigationMode == UINavigationMode.NormalNavigation;
            }
        }

        /// <summary>
        /// 是否隐藏其他UI
        /// </summary>
        public bool IsHiddenAll
        {
            get
            {
                return UIShowModel.HideOther == this.infoData.CoreData.ShowModel
                    || UIShowModel.HideEverything == this.infoData.CoreData.ShowModel;
            }
        }

        /// <summary>
        /// 在UI关闭时是否销毁
        /// </summary>

        public bool IsDestoryWhenClosed
        {
            get
            {
                return this.infoData.CoreData.IsDestoryOnClosed;
            }
        }

        protected virtual void Awake()
        {
            this.gameObject.SetActive(true);
            _mTrans = this.gameObject.GetComponent<RectTransform>();
        }

        public virtual void ShowUI(DelOnCompleteShowUI onComplete = null)
        {
            if (null != _mask)
            {
                _mask.SetAsLastSibling();
                // _mask.gameObject.SetActive(true);
            }
            _mTrans.SetAsLastSibling();
            this.gameObject.SetActive(true);
            if (null != onComplete)
            {
                onComplete();
            }
        }

        public virtual void ReShowUI()
        {
            if (null != _mask)
            {
                _mask.gameObject.SetActive(true);
            }
            this.gameObject.SetActive(true);
        }

        public virtual void HideUI(DelOnCompleteHideUI onComplete = null)
        {
            if (infoData.CoreData.IsDestoryOnClosed)
            {
                if (null != onComplete)
                {
                    onComplete();
                }
                DestoryUI();
            }
            else
            {
                HideDirectly();
                if (null != onComplete)
                {
                    onComplete();
                }
            }
        }


        /// <summary>
        /// 管理器管理导航栈时隐藏调用，直接隐藏对应UI窗体
        /// </summary>
        public virtual void HideDirectly()
        {
            if (null != _mask)
            {
                _mask.gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 销毁当前UI
        /// </summary>
        public virtual void DestoryUI()
        {
            if (null != _mask)
            {
                GameObject.DestroyImmediate(_mask, gameObject);
                _mask = null;
            }
            GameObject.DestroyImmediate(this.gameObject);
        }
    }
}
