/*****************************************************************************
 * filename :  SceneLoadHelper.cs
 * author   :  Zhang Yunxing
 * date     :  2018/09/29 20:57
 * desc     :  场景加载助手
 * changelog:  
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Games.GameDefine;
using Games.UICore;

public class SceneLoadHelper : MonoBehaviour
{
    public static SceneLoadHelper Instance;
    public delegate void DelOnCompleteLoadScene();

    private DelOnCompleteLoadScene _onComplete;

    private Slider _loadingSlider;
    private Transform _loadingSliderTrans;
    private AsyncOperation _asyn;
    private int _barProcess;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 调用携程异步加载目标场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="onComplete"></param>
    public void LoadTargetScene(string sceneName, DelOnCompleteLoadScene onComplete = null)
    {
        // 切换到过渡场景，异步加载目标场景，完成后切换
        SceneManager.LoadScene(GameGlobeVar.LOADING_SCENE_NAME);
        _onComplete = onComplete;
        _barProcess = 0;
        if (null == _loadingSlider)
        {
            GameObject cacheLoadingUI = Resources.Load<GameObject>(GameGlobeVar.LOADING_UI_RESPATH);
            if(null != cacheLoadingUI)
            {
                _loadingSliderTrans = GameObject.Instantiate(cacheLoadingUI).transform;
                _loadingSlider = Utils.GetComponentFromTransRecursion<Slider>(_loadingSliderTrans);
                Utils.AddChildToParent(UIManager.Instance.UiCanvas, _loadingSliderTrans);
            }
        }
        _loadingSliderTrans.SetAsLastSibling();
        _loadingSlider.gameObject.SetActive(true);
        UIManager.Instance.OnSceneChangedDestrtory();
        StartCoroutine(LoadScene(sceneName));
    }

    private void Update()
    {
        if (_asyn == null)
        {
            return;
        }
        int theProcess = 0;
        if (_asyn.progress < 0.9f)
        {
            theProcess = (int)(_asyn.progress) * 100;
        }
        else
        {
            theProcess = 100;
        }
        if (_barProcess < theProcess)
        {
            _barProcess++;
        }
        _loadingSlider.value = _barProcess * 1.0f / 100.0f;
        if (100 == _barProcess)
        {
            _asyn.allowSceneActivation = true;
        }
        if (_asyn.isDone)
        {
            if (null != _onComplete)
            {
                _onComplete();
            }
            _asyn = null;
            _loadingSlider.value = 0.0f;
            // this.gameObject.SetActive(false);
            _loadingSliderTrans.gameObject.SetActive(false);
            Utils.ClearMemory();
        }
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadScene(string sceneName)
    {
        _asyn = SceneManager.LoadSceneAsync(sceneName);
        _asyn.allowSceneActivation = false;
        yield return _asyn;
    }
}