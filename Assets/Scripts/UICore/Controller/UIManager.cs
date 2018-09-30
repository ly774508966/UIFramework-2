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
        // 当UI显示隐藏所有UI的时候，缓存额外隐藏的UIID以便复原
        private List<int> _hiddenAllUICache;

        private UIBase _curNavUIBase = null;

        // Each UIType root
        private RectTransform _fixedUIRoot;
        private RectTransform _baseUIRoot;
        private RectTransform _popupUIRoot;
        private RectTransform _sencondPopUpUIRoot;

        public RectTransform UiCanvas;
        public static RectTransform UICanvasTrans
        {
            get { return _instance.UiCanvas; }
        }

        private static UIManager _instance;

        // 按照BaseUI所在Hierarchy目录的节点顺序升序排序规则
        private CompareUIBase compareWithSibling = new CompareUIBase();
        private class CompareUIBase : IComparer<UIBase>
        {
            public int Compare(UIBase left, UIBase right)
            {
                return left.Trans.GetSiblingIndex() - right.Trans.GetSiblingIndex();
            }
        }

        #region mono func
        private void Awake()
        {
            _instance = this;
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
            if (null == _hiddenAllUICache)
            {
                _hiddenAllUICache = new List<int>();
            }
            InitUIManager();
            DontDestroyOnLoad(UiCanvas);
        }

        private void OnDestroy()
        {
            _instance = null;
        }
        #endregion


        #region core static public func

        /// <summary>
        /// 显示UI，包括导航栈，模态属性初始化等
        /// </summary>
        /// <param name="uiInfo"></param>
        public static void ShowUI(UIInfoData uiInfo, DelOnCompleteShowUI onComplete = null)
        {
            if (null == uiInfo || CoreGlobeVar.INVAILD_UIID == uiInfo.UIID)
            {
                return;
            }
            UIManager._instance.OnShowUI(uiInfo, onComplete);
        }
        private void OnShowUI(UIInfoData uiInfo, DelOnCompleteShowUI onComplete = null)
        {
            UIBase showUI = LoadShowUI(uiInfo);
            if (null == showUI)
            {
                return;
            }
            // 设置模态窗口
            Utils.AddUIMask(showUI, GetUITypeRootTrans(uiInfo));
            RefreshNavData(showUI);
            // 不同显示模式采取不同差异化策略
            switch (uiInfo.CoreData.ShowModel)
            {
                case UIShowModel.DoNoting:
                    break;
                case UIShowModel.HideOther:
                case UIShowModel.HideEverything:
                    HideAllOtherUI(showUI);
                    break;
                case UIShowModel.TypeMutex:
                    CheckMutexHidden(showUI);
                    break;
                case UIShowModel.DestoryOther:
                    break;
                default:
                    break;
            }
            CheckNavData(showUI);
            showUI.ShowUI(onComplete);
            _showUIDic[uiInfo.UIID] = showUI;
            if (showUI.IsAddedToBackSequence)
            {
                showUI.PreUIInfo = null == _curNavUIBase ? null : _curNavUIBase.infoData;
                _curNavUIBase = showUI;
            }
        }


        /// <summary>
        /// 直接关闭隐藏UI，不经过导航栈
        /// </summary>
        /// <param name="uiInfo"></param>
        public static void HideUI(UIInfoData uiInfo, DelOnCompleteHideUI onComplete = null)
        {
            UIManager._instance.OnHideUI(uiInfo, onComplete);
        }
        private void OnHideUI(UIInfoData uiInfo, DelOnCompleteHideUI onComplete = null)
        {
            if (!_showUIDic.ContainsKey(uiInfo.UIID))
            {
                return;
            }
            int uiid = uiInfo.UIID;
            // 委托不为空等待逻辑执行完成后再隐藏UI
            if (null != onComplete)
            {
                onComplete += delegate
                {
                    _showUIDic.Remove(uiInfo.UIID);
                    if (uiInfo.CoreData.IsDestoryOnClosed && _allUIDic.ContainsKey(uiid))
                    {
                        _allUIDic.Remove(uiInfo.UIID);
                    }
                };
                _showUIDic[uiid].HideUI(onComplete);
            }
            else
            {
                _showUIDic[uiid].HideUI();
                _showUIDic.Remove(uiInfo.UIID);
                if (uiInfo.CoreData.IsDestoryOnClosed && _allUIDic.ContainsKey(uiid))
                {
                    _allUIDic.Remove(uiInfo.UIID);
                }
            }
        }


        /// <summary>
        /// 经过反向导航栈然后关闭隐藏UI
        /// </summary>
        /// <param name="uiInfo"></param>
        public static void CloseReturnUI(UIInfoData uiInfo, DelOnCompleteHideUI onComplete = null)
        {
            UIManager._instance.OnCloseReturnUI(uiInfo, onComplete);
        }
        private void OnCloseReturnUI(UIInfoData uiInfo, DelOnCompleteHideUI onComplete = null)
        {
            if (!_showUIDic.ContainsKey(uiInfo.UIID))
            {
                return;
            }
            // 没有导航数据使用前置UI的Infodata导回去
            if (0 == _backSequenceStack.Count)
            {
                if (null == _curNavUIBase)
                {
                    return;
                }
                UIInfoData preUIInfo = _curNavUIBase.PreUIInfo;
                if (null != preUIInfo)
                {
                    if (null == onComplete)
                    {
                        onComplete = delegate { OnShowUI(preUIInfo); };
                    }
                    else
                    {
                        onComplete += delegate { OnShowUI(preUIInfo); };
                    }
                    OnHideUI(uiInfo, onComplete);
                }
                return;
            }
            // 有导航数据
            NavigationData uiReturnInfo = _backSequenceStack.Peek();
            if (null != uiReturnInfo)
            {
                int willShowUIID = uiReturnInfo.HideTargetUI.infoData.UIID;
                if (uiInfo.UIID != willShowUIID)
                {
                    return;
                }
                if (!_showUIDic.ContainsKey(willShowUIID))
                {
                    return;
                }
                DelOnCompleteHideUI tmpDel = delegate
                {
                    int siblingIndex = 0;
                    foreach (int backId in uiReturnInfo.BackShowTargetsList)
                    {
                        if (_showUIDic.ContainsKey(backId))
                        {
                            continue;
                        }
                        if (!_allUIDic.ContainsKey(backId))
                        {
                            if (null == NavBackUIReload(backId, siblingIndex + 1))
                            {
                                continue;
                            }
                        }
                        _allUIDic[backId].ReShowUI();
                        _showUIDic[backId] = _allUIDic[backId];
                        siblingIndex = _showUIDic[backId].Trans.GetSiblingIndex();
                    }
                    _allUIDic.TryGetValue(uiReturnInfo.BackShowTargetsList[uiReturnInfo.BackShowTargetsList.Count - 1], out _curNavUIBase);
                    _backSequenceStack.Pop();
                };
                if(null == onComplete)
                {
                    onComplete = tmpDel;
                }
                else
                {
                    onComplete += tmpDel;
                }
                OnHideUI(uiInfo, onComplete);
            }
            ReShowHiddenAllCache();
        }


        /// <summary>
        /// 关闭UI，根据导航策略采取不同关闭行为
        /// </summary>
        /// <param name="uiInfo"></param>
        /// <param name="onComplete"></param>
        public static void CloseUI(UIInfoData uiInfo, DelOnCompleteHideUI onComplete = null)
        {
            if (null == uiInfo || CoreGlobeVar.INVAILD_UIID == uiInfo.UIID)
            {
                return;
            }
            switch (uiInfo.CoreData.NavigationMode)
            {
                case UINavigationMode.NormalNavigation:
                    UIManager._instance.OnCloseReturnUI(uiInfo);
                    break;
                case UINavigationMode.IngoreNavigation:
                    UIManager._instance.OnHideUI(uiInfo, onComplete);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 在切换场景时删除所有切场景需要销毁的UI
        /// </summary>
        public static void OnSceneChangedDestrtory()
        {
            UIManager._instance.OnSceneChangedUIDestrtory();
        }
        private void OnSceneChangedUIDestrtory()
        {
            List<int> removeKey = null;
            foreach (KeyValuePair<int, UIBase> pair in _allUIDic)
            {
                if (pair.Value.infoData.CoreData.IsCloseOnSceneChange)
                {
                    if (null == removeKey)
                    {
                        removeKey = new List<int>();
                    }
                    removeKey.Add(pair.Key);
                }
            }
            if (null != removeKey)
            {
                for (int i = 0; i < removeKey.Count; ++i)
                {
                    _allUIDic[removeKey[i]].DestoryUI();
                    if (_showUIDic.ContainsKey(removeKey[i]))
                    {
                        _showUIDic.Remove(removeKey[i]);
                    }
                }
            }
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
            if (null != _hiddenAllUICache)
            {
                _hiddenAllUICache.Clear();
            }
            if (null == _fixedUIRoot)
            {
                _fixedUIRoot = Utils.AddUINullChild(UiCanvas, "fixedUIRoot");
            }
            if (null == _baseUIRoot)
            {
                _baseUIRoot = Utils.AddUINullChild(UiCanvas, "baseUIRoot");
            }
            if (null == _popupUIRoot)
            {
                _popupUIRoot = Utils.AddUINullChild(UiCanvas, "popupUIRoot");
            }
            if (null == _sencondPopUpUIRoot)
            {
                _sencondPopUpUIRoot = Utils.AddUINullChild(UiCanvas, "sencondPopUpUIRoot");
            }
        }


        /// <summary>
        /// 反向导航遇到关闭就销毁的UI，重新load一次
        /// Hint : 设置在 Hierarchy 中的节点顺序， 确保UI层级正确显示
        /// </summary>
        /// <param name="uiid"></param>
        /// <param name="siblingIndex"></param>
        /// <returns></returns>
        private UIBase NavBackUIReload(int uiid, int siblingIndex)
        {
            if (CoreGlobeVar.INVAILD_UIID == uiid)
            {
                return null;
            }
            UIBase reloadUI = LoadShowUI(UIInfoData.UIInfoDic[uiid]);
            Utils.AddUIMask(reloadUI, GetUITypeRootTrans(reloadUI.infoData));
            if (null != reloadUI.Mask)
            {
                reloadUI.Mask.SetSiblingIndex(siblingIndex);
                reloadUI.Trans.SetSiblingIndex(siblingIndex + 1);
            }
            else
            {
                reloadUI.Trans.SetSiblingIndex(siblingIndex);
            }
            return reloadUI;
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
        /// 通过UIinfo加载UI，并缓存到allUIDic
        /// </summary>
        /// <param name="uiInfo"></param>
        /// <returns></returns>
        private UIBase LoadShowUI(UIInfoData uiInfo)
        {
            if (_showUIDic.ContainsKey(uiInfo.UIID))
            {
                return null;
            }
            UIBase showUI = GetUIBase(uiInfo.UIID);
            if (null == showUI)
            {
                string resourcePath = uiInfo.ResPathStr + "/" + uiInfo.ResNameStr;
                GameObject cacheUI = Resources.Load<GameObject>(resourcePath);
                if (null != cacheUI)
                {
                    GameObject willShowUI = GameObject.Instantiate(cacheUI);
                    willShowUI.SetActive(true);
                    showUI = willShowUI.GetComponent<UIBase>();
                    Utils.AddChildToParent(GetUITypeRootTrans(uiInfo), willShowUI.transform);
                    _allUIDic[uiInfo.UIID] = showUI;
                    willShowUI = null;
                }
            }
            return showUI;
        }

        /// <summary>
        /// 清空导航栈
        /// </summary>
        private void ClearBackSequence()
        {
            if (_backSequenceStack != null)
                _backSequenceStack.Clear();
        }

        /// <summary>
        /// 更新导航栈，此流程UI隐藏只是隐藏，不检测是否销毁
        /// </summary>
        /// <param name="showUI"></param>
        private void RefreshNavData(UIBase showUI)
        {
            if (!showUI.IsAddedToBackSequence)
            {
                return;
            }
            List<int> removeKey = null;
            List<UIBase> sortedHiddenList = new List<UIBase>();
            NavigationData navData = new NavigationData();
            foreach (KeyValuePair<int, UIBase> pair in _showUIDic)
            {
                // DoNothing显示模式不会隐藏UI，无需遍历装载removeKey
                if (UIShowModel.DoNoting != showUI.infoData.CoreData.ShowModel)
                {
                    if (UIRootType.Fixed == pair.Value.infoData.CoreData.RootType)
                    {
                        continue;
                    }
                    if (null == removeKey)
                    {
                        removeKey = new List<int>();
                    }
                    removeKey.Add(pair.Key);
                    pair.Value.HideDirectly();
                }
                if (pair.Value.IsAddedToBackSequence)
                {
                    sortedHiddenList.Add(pair.Value);
                }
            }
            sortedHiddenList.Sort(compareWithSibling);
            for (int i = 0; i < sortedHiddenList.Count; ++i)
            {
                if (null == navData.BackShowTargetsList)
                {
                    navData.BackShowTargetsList = new List<int>();
                }
                navData.BackShowTargetsList.Add(sortedHiddenList[i].infoData.UIID);
            }
            navData.HideTargetUI = showUI;
            _backSequenceStack.Push(navData);

            if (null != removeKey)
            {
                foreach(int removeid in removeKey)
                {
                    if(CoreGlobeVar.INVAILD_UIID == removeid)
                    {
                        continue;
                    }
                    if(!_showUIDic[removeid].IsAddedToBackSequence || _showUIDic[removeid].IsDestoryWhenClosed)
                    {
                        _showUIDic[removeid].DestoryUI();
                        _allUIDic.Remove(removeid);
                    }
                    _showUIDic.Remove(removeid);
                }
            }
        }

        /// <summary>
        /// 处理非正常导航之外的隐藏其他窗口的逻辑
        /// </summary>
        /// <param name="showUI"></param>
        private void HideAllOtherUI(UIBase showUI)
        {
            if (!showUI.IsHiddenAll)
            {
                return;
            }
            _hiddenAllUICache.Clear();
            bool isHiddenFixed = UIShowModel.HideEverything == showUI.infoData.CoreData.ShowModel ? true : false;
            foreach (KeyValuePair<int, UIBase> pair in _showUIDic)
            {
                if (UIRootType.Fixed == pair.Value.infoData.CoreData.RootType && !isHiddenFixed)
                    continue;
                _hiddenAllUICache.Add(pair.Value.infoData.UIID);
                pair.Value.HideDirectly();
            }
            if (_hiddenAllUICache.Count > 0)
            {
                foreach (int removeid in _hiddenAllUICache)
                {
                    if (CoreGlobeVar.INVAILD_UIID == removeid)
                    {
                        continue;
                    }
                    if (!_showUIDic[removeid].IsAddedToBackSequence || _showUIDic[removeid].IsDestoryWhenClosed)
                    {
                        _showUIDic[removeid].DestoryUI();
                        _allUIDic.Remove(removeid);
                    }
                    _showUIDic.Remove(removeid);
                }
            }
        }

        /// <summary>
        /// 处理类型互斥的隐藏逻辑
        /// </summary>
        /// <param name="showUI"></param>
        private void CheckMutexHidden(UIBase showUI)
        {
            if (UIShowModel.TypeMutex != showUI.infoData.CoreData.ShowModel)
            {
                return;
            }
            foreach (RectTransform child in GetUITypeRootTrans(showUI.infoData))
            {
                UIBase uiBase = child.GetComponent<UIBase>();
                if (null != uiBase && uiBase.infoData.UIID != showUI.infoData.UIID)
                {
                    if (_showUIDic.ContainsKey(uiBase.infoData.UIID))
                    {
                        _showUIDic.Remove(uiBase.infoData.UIID);
                    }
                    uiBase.HideDirectly();
                    if (uiBase.IsDestoryWhenClosed)
                    {
                        uiBase.DestoryUI();
                        _allUIDic.Remove(uiBase.infoData.UIID);
                    }
                }
            }
        }

        /// <summary>
        /// 检查导航信息的有效性
        /// </summary>
        /// <param name="showUI"></param>
        private void CheckNavData(UIBase showUI)
        {
            if (showUI.infoData.CoreData.IsClearNavStack)
            {
                ClearBackSequence();
                return;
            }
            if(!showUI.IsAddedToBackSequence)
            {
                return;
            }
            if (_backSequenceStack.Count > 0)
            {
                NavigationData backData = _backSequenceStack.Peek();
                if (null != backData.HideTargetUI)
                {
                    // 导航序列已经被打乱了
                    if (showUI.infoData.UIID != backData.HideTargetUI.infoData.UIID)
                    {
                        ClearBackSequence();
                    }
                }
                else
                {
                    // 导航目标错误
                }
            }
        }

        /// <summary>
        /// 重新显示额外隐藏的UI
        /// </summary>
        private void ReShowHiddenAllCache()
        {
            int siblingIndex = 0;
            foreach (int reshowID in _hiddenAllUICache)
            {
                if (_showUIDic.ContainsKey(reshowID))
                {
                    continue;
                }
                if (!_allUIDic.ContainsKey(reshowID))
                {
                    if (null == NavBackUIReload(reshowID, siblingIndex + 1))
                    {
                        continue;
                    }
                }
                _allUIDic[reshowID].ReShowUI();
                _showUIDic[reshowID] = _allUIDic[reshowID];
                siblingIndex = _allUIDic[reshowID].Trans.GetSiblingIndex();
            }
            _hiddenAllUICache.Clear();
        }
        #endregion
    }
}