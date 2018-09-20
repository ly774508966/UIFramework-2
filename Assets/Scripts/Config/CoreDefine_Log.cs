/*****************************************************************************
 * filename :  CoreDefine_Log.cs
 * author   :  Zhang Yunxing
 * date     :  2018/08/29 15:19
 * desc     :  Log 相关常量定义
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.CoreDefine
{
    public partial class CoreGlobeVar
    {
        /// <summary>
        /// 日志状态（部署模式）
        /// </summary>
        public enum LogModel
        {
            Default,        // 默认模式（不写入到文件）
            Develop,        // 开发模式（输出所有日志）
            Deploy,         // 部署模式（只输出最核心日志信息，警告错误信息等）
        }

        // 不同种类log存储文件名称
        public const string LOG_NORMAL_FILE = "log.txt";
        public const string LOG_WARNING_FILE = "logwarning.txt";
        public const string LOG_ERROR_FILE = "logerror.txt";
    }
}
