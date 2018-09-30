using Games.UICore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Launcher : MonoBehaviour
{
    private GameObject _gameManager = null;

    [SerializeField, SetProperty("IsDebugLogEnable")]
    private bool _isDebugLogEnable = true;

    // 调试用，手动控制自定义Log的开启关闭
    public bool IsDebugLogEnable
    {
        set
        {
            _isDebugLogEnable = value;
            LogModule.IsDebugEnable = LogModule.IsLogFileEnable = value;
            if (false == value)
            {
                Debug.LogWarning("Log2File has been set to disable!");
            }
            else
            {
                Debug.LogWarning("Log2File has been set to enable!");
            }
        }
    }

    private void Awake()
    {
        // 初始化一堆管理器
        if (null == _gameManager)
        {
            _gameManager = new GameObject("_gameManager");
        }
        GameObject.DontDestroyOnLoad(_gameManager);
        Utils.TryAddComponent<LogModule>(_gameManager);
        LogModule.IsDebugEnable = LogModule.IsLogFileEnable = _isDebugLogEnable;
        Utils.TryAddComponent<SceneLoadHelper>(_gameManager);

        /*
         * LogMudel Test
        LogModule.Log("log out");
        LogModule.LogError("logError!");
        LogModule.LogWarning("LogWarning");
        LogModule.Log(GameObject.Find("testssss").GetComponent<Transform>());
        */
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        UIManager.ShowUI(UIInfos.MainUI);
    }
}