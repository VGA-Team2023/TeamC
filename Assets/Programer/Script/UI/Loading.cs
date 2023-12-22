﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{    
    [SerializeField] string _nextSceneName = "";
    [SerializeField] private float _waitTimer;
    private LoadingPanel _loadingPanelInstance;
    private void Start()
    {
        if (_loadingPanelInstance == null)
        {
            _loadingPanelInstance = LoadingPanel.Instance;

            if (_loadingPanelInstance == null)
            {
                return;
            }
        }

        _loadingPanelInstance.gameObject.SetActive(false);
    }
    public void LoadingScene()
    {
        GameManager.Instance?.BGMStop();
        GameManager.Instance?.SEStopAll();
        _loadingPanelInstance.gameObject.SetActive(true);
        StartCoroutine(WaitForLoading());
    }

    IEnumerator WaitForLoading()
    {
        yield return new WaitForSeconds(_waitTimer);
        //AsyncOperation async = SceneManager.LoadSceneAsync(_nextSceneName);
        //async.allowSceneActivation = false;

        //while (async.progress < 0.9f && !async.isDone)
        //{
        //    yield return null;
        //}

        //yield return new WaitForSeconds(_waitTimer);

        //async.allowSceneActivation = true;
    }
}
