using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{    
    [SerializeField] string _nextSceneName = "";
    [SerializeField] private float _waitTimer;
    private LoadingPanel _loadingPanel = null;
    private void Start()
    {
        if (_loadingPanel != null)
        {
            return;
        }
        else
        {
            Loading[] lis = FindObjectsOfType<Loading>();
            LoadingPanel LP = FindObjectOfType<LoadingPanel>();
            foreach (var target in lis)
            {
                target._loadingPanel = LP;
            }
            _loadingPanel.gameObject.SetActive(false);
        }
    }
    public void LoadingScene()
    {
        _loadingPanel.gameObject.SetActive(true);
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
