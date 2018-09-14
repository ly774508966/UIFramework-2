using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.UICore
{
    // 一个UI窗体的完整数据
    public class UIInfoData
    {
        public UICoreData CoreData;     // 核心数据
        public string ResPathStr;       // 资源路径
        public string ResNameStr;       // 资源名称
        public UIInfoData(UICoreData coreData, string resPath, string resName)
        {
            CoreData = coreData;
            ResPathStr = resPath;
            ResNameStr = resName;
        }
    }
}