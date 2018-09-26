using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.CoreDefine;
using UnityEngine.UI;

namespace Games.UICore
{
    public class UIManager : MonoBehaviour
    {
        private Dictionary<int, UIBase> _allUIDic;
        private Dictionary<int, UIBase> _showUIDic;
        private Stack<NavigationData> _backSequenceStack;

        private int _curUIID = CoreGlobeVar.INVAILD_UIID;
        private int _preUIiD = CoreGlobeVar.INVAILD_UIID;

        [SerializeField]
        private RectTransform _uiCanvas;

        // Each UIType root
        private RectTransform _fixedUIRoot;
        private RectTransform _baseUIRoot;
        private RectTransform _popupUIRoot;
        private RectTransform _sencondPopUpUIRoot;

        public static UIManager Instance;


        #region mono func
        private void Awake()
        {
            Instance = this;
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
            DontDestroyOnLoad(_uiCanvas);
        }

        private void OnDestroy()
        {
            Instance = null;
        }
        #endregion


        #region core public func
        public void ShowUI(UIInfoData uiInfo)
        {
            UIBase showUI = ReadyShowUI(uiInfo);
            if (null != showUI)
            {
                // todo, 这个逻辑应该写在baseUI的showUI里面
                showUI.Mask.SetAsLastSibling();
                showUI.Trans.SetAsLastSibling();
            }
        }

        public void HideUI(int uiID)
        {

        }
        #endregion


        #region custom private func
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
            if (null == _fixedUIRoot)
            {
                _fixedUIRoot = AddUINullChild(_uiCanvas, "fixedUIRoot");
            }
            if (null == _baseUIRoot)
            {
                _baseUIRoot = AddUINullChild(_uiCanvas, "baseUIRoot");
            }
            if (null == _popupUIRoot)
            {
                _popupUIRoot = AddUINullChild(_uiCanvas, "popupUIRoot");
            }
            if (null == _sencondPopUpUIRoot)
            {
                _sencondPopUpUIRoot = AddUINullChild(_uiCanvas, "sencondPopUpUIRoot");
            }
        }

        /// <summary>
        /// 获取当前uiid对应的类型root节点
        /// </summary>
        /// <param name="uiInfo"></param>
        /// <returns></returns>
        private RectTransform GetUITypeRootTrans(UIInfoData uiInfo)
        {
            if (null != uiInfo)
            {
                switch (uiInfo.CoreData.RootType)
                {
                    case UIRootType.Base:
                        return _baseUIRoot;
                    case UIRootType.PopUp:
                        return _popupUIRoot;
                    case UIRootType.Fixed:
                        return _fixedUIRoot;
                    case UIRootType.SecondPopUp:
                        return _sencondPopUpUIRoot;
                    default:
                        return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 在已加载的UI中查询目标id的UI管理脚本
        /// </summary>
        /// <param name="uiID"></param>
        /// <returns></returns>
        private UIBase GetUIBase(int uiID)
        {
            if (_allUIDic.ContainsKey(uiID))
            {
                return _allUIDic[uiID];
            }
            return null;
        }

        /// <summary>
        /// 向对应UI物体添加空白子物体，默认拉伸平铺布局
        /// </summary>
        /// <param name="parientTrns"></param>
        /// <param name="rootName"></param>
        /// <returns></returns>
        private RectTransform AddUINullChild(RectTransform parientTrns, string rootName)
        {
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
        /// UI显示准备过程，UI加载，遮罩设置，导航栈设置
        /// </summary>
        /// <param name="uiID"></param>
        /// <returns></returns>
        private UIBase ReadyShowUI(UIInfoData uiInfo)
        {
            if (_showUIDic.ContainsKey(uiInfo.UIID))
            {
                return null;
            }

            RectTransform rootTrans = GetUITypeRootTrans(uiInfo);
            UIBase baseUI = GetUIBase(uiInfo.UIID);
            if (null == baseUI)
            {
                string resourcePath = uiInfo.ResPathStr + "/" + uiInfo.ResNameStr;
                GameObject cacheUI = Resources.Load<GameObject>(resourcePath);
                if (null != cacheUI)
                {
                    GameObject willShowUI = GameObject.Instantiate(cacheUI);
                    willShowUI.SetActive(true);
                    baseUI = willShowUI.GetComponent<UIBase>();
                    Utils.AddChildToParent(rootTrans, willShowUI.transform);
                    _allUIDic[uiInfo.UIID] = baseUI;
                    willShowUI = null;
                }
            }
            AddUIMask(baseUI, uiInfo, rootTrans);
            // todo导航栈
            return baseUI;
        }

        /// <summary>
        /// 设置UI遮罩
        /// </summary>
        /// <param name="showUI"></param>
        /// <param name="showUIInfo"></param>
        /// <param name="rootTrans"></param>
        private void AddUIMask(UIBase showUI, UIInfoData showUIInfo, RectTransform rootTrans)
        {
            if (UIColliderType.Penetrate == showUIInfo.CoreData.ColliderType)
            {
                return;
            }
            Image maskImage = null;
            if (null == showUI.Mask)
            {
                RectTransform mask = AddUINullChild(rootTrans, "uiMask");
                maskImage = Utils.TryAddComponent<Image>(mask.gameObject);
                showUI.Mask = mask;
            }
            if (null != showUIInfo)
            {
                switch (showUIInfo.CoreData.ColliderType)
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
        #endregion
    }
}