using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{    
    [SerializeField] string _nextSceneName = "";
    [SerializeField] private float _waitTimer;
    private static LoadingPanel _loadingPanelInstance;
    private void Start()
    {
        if (_loadingPanelInstance == null)
        {
            _loadingPanelInstance = FindObjectOfType<LoadingPanel>();

            if (_loadingPanelInstance == null)
            {
                return;
            }
        }

        _loadingPanelInstance.gameObject.SetActive(false);
    }
    public void LoadingScene()
    {
        _loadingPanelInstance.gameObject.SetActive(true);
        StartCoroutine(WaitForLoading());
    }

    IEnumerator WaitForLoading()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(_nextSceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress > 0.9f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(_waitTimer);

            async.allowSceneActivation = true;
        }
    }
}
