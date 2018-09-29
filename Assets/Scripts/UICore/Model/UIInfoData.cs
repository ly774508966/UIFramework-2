/*****************************************************************************
 * filename :  UIInfoData.cs
 * author   :  Zhang Yunxing
 * date     :  2018/09/20 22:00
 * desc     :  一个UI窗体的完整基本信息定义
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.UICore
{
    // 一个UI窗体的完整数据
    public class UIInfoData
    {
        private static int _idNum = -1; // 自增ID静态变量
        public int UIID = -1;           // UIID编号(便于存贮和导航查询)
        public UICoreData CoreData;     // 核心数据
        public string ResPathStr;       // 资源路径
        public string ResNameStr;       // 资源名称
        public UIInfoData(UICoreData coreData, string resPath, string resName)
        {
            UIID = ++_idNum;
            CoreData = coreData;
            ResPathStr = resPath;
            ResNameStr = resName;
        }
    }
}