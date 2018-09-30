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

        /// <summary>
        /// UI配套的背景遮罩
        /// </summary>
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
                SetUIMaskEvent();
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

        /// <summary>
        /// 当前UI前置UIInfoData，用于反向导航
        /// </summary>
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
                return infoData.CoreData.NavigationMode == UINavigationMode.NormalNavigation;
            }
        }

        /// <summary>
        /// 是否隐藏其他UI
        /// </summary>
        public bool IsHiddenAll
        {
            get
            {
                return UIShowModel.HideOther == infoData.CoreData.ShowModel
                    || UIShowModel.HideEverything == infoData.CoreData.ShowModel;
            }
        }

        /// <summary>
        /// 在UI关闭时是否销毁
        /// </summary>

        public bool IsDestoryWhenClosed
        {
            get
            {
                return infoData.CoreData.IsDestoryOnClosed;
            }
        }

        protected virtual void Awake()
        {
            gameObject.SetActive(true);
            _mTrans = gameObject.GetComponent<RectTransform>();
        }
        #region custom func

        /// <summary>
        /// 展示UI
        /// </summary>
        /// <param name="onComplete"></param>
        public virtual void ShowUI(DelOnCompleteShowUI onComplete = null)
        {
            if (null != _mask)
            {
                _mask.SetAsLastSibling();
                // _mask.gameObject.SetActive(true);
            }
            _mTrans.SetAsLastSibling();
            gameObject.SetActive(true);
            if (null != onComplete)
            {
                onComplete();
            }
        }

        /// <summary>
        /// 重新显示UI
        /// </summary>
        public virtual void ReShowUI()
        {
            if (null != _mask)
            {
                _mask.gameObject.SetActive(true);
            }
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 按照UI的核心策略，隐藏或者销毁UI
        /// </summary>
        /// <param name="onComplete"></param>

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
        /// 隐藏对应UI窗体
        /// </summary>
        public virtual void HideDirectly()
        {
            if (null != _mask)
            {
                _mask.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 销毁当前UI
        /// </summary>
        public virtual void DestoryUI()
        {
            if (null != _mask)
            {
                RemoveUIMaskEvent();
                GameObject.DestroyImmediate(_mask.gameObject);
                _mask = null;
            }
            GameObject.DestroyImmediate(gameObject);
        }
        #endregion

        #region mask event setting

        /// <summary>
        /// 设置当前UI背景遮罩的相关UI事件
        /// </summary>
        protected virtual void SetUIMaskEvent()
        {

        }

        /// <summary>
        /// 移除当前UI背景遮罩的相关UI事件
        /// </summary>
        protected virtual void RemoveUIMaskEvent()
        {

        }
        #endregion
    }
}
