using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] LoadingPanel _loadingPanel;
    [SerializeField] string _nextSceneName = "";
    [SerializeField] private float _waitTimer;
    private void Start()
    {
        _loadingPanel = FindObjectOfType<LoadingPanel>();
        _loadingPanel.gameObject.SetActive(false);
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
            //_loadingPanel.SetActive(false);
        }
    }
}
