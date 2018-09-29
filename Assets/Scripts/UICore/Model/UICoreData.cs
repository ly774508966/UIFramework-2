/*****************************************************************************
 * filename :  UICoreData.cs
 * author   :  Zhang Yunxing
 * date     :  2018/09/20 22:02
 * desc     :  UICore 核心数据结构定义
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.UICore
{
    /// <summary>
    /// UI显示前的委托
    /// </summary>
    public delegate void DelOnCompleteShowUI();

    /// <summary>
    /// UI显示后的委托
    /// </summary>
    public delegate void DelOnCompleteHideUI();


    /// <summary>
    /// UI窗体根节点类型
    /// </summary>
    public enum UIRootType
    {
        Base = 0,               // 基础UI (非常驻的一些UI,商城，属性，设置界面....)
        Fixed,                  // 固定窗口 (常驻UI,属性，基础UI或场景常驻)
        PopUp,                  // 一级弹出 (任务，游戏说名，小互动...)
        SecondPopUp,            // 二级弹出 (新手引导，Tips，MessageBox等)
    }

    /// <summary>
    /// UI显示模式
    /// </summary>
    public enum UIShowModel
    {
        DoNoting = 0,           // 直接显示，不采取其他操作
        HideOther,              // 全局互斥 (隐藏其他UI，不包括FixedUI)
        HideEverything,         // 全局互斥 (隐藏其他UI，包括FixedUI)
        TypeMutex,              // 类型互斥 (同类型互斥)
        DestoryOther,           // 销毁其他窗口，保留
    }

    /// <summary>
    /// UI碰撞体类型
    /// </summary>
    public enum UIColliderType
    {
        Penetrate = 0,          // 可穿透
        Lucency,                // 全透明不可穿透
        Translucent,            // 半透明不可穿透
        ImPenetrable            // 低透明度不可穿透
    }

    /// <summary>
    /// UI窗体导航类型
    /// </summary>
    public enum UINavigationMode
    {
        IngoreNavigation = 0,   // 无需导航
        NormalNavigation,       // 
    }
    // UI核心数据, 确定一个UI窗体的表现类型
    public struct UICoreData
    {
        // 是否需要清空反向切换栈
        public bool IsClearNavStack;
        // 场景切换时是否关闭
        public bool IsCloseOnSceneChange;
        // UI关闭时是否直接销毁
        public bool IsDestoryOnClosed;

        public UIRootType RootType;
        public UIShowModel ShowModel;
        public UIColliderType ColliderType;
        public UINavigationMode NavigationMode;

        public UICoreData(UIRootType rootType, UIShowModel showModel, UIColliderType colliderType = UIColliderType.Penetrate,
            UINavigationMode navigationMode = UINavigationMode.IngoreNavigation,
            bool isClearNavStack = false, bool isColseOnSceneChange = false, bool isDestoryOnClosed = false)
        {
            RootType = rootType;
            ShowModel = showModel;
            ColliderType = colliderType;
            NavigationMode = navigationMode;
            IsClearNavStack = isClearNavStack;
            IsCloseOnSceneChange = isColseOnSceneChange;
            IsDestoryOnClosed = isDestoryOnClosed;
        }
    }
}
