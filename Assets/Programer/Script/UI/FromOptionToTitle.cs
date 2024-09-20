using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FromOptionToTitle : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _panel;
    private string _nextSceneName = "Title 1";
    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>ReturnTitle()) ;
    }
    private void ReturnTitle()
    {
        GameManager.Instance?.BGMStop();
        GameManager.Instance?.SEStopAll();
        GameManager.Instance?.VoiceStopAll();
        _panel?.SetActive(false);
        GameManager.Instance.ScoreManager.ScoreReset();
        LoadingPanel.Instance?.gameObject?.SetActive(true);
        SceneManager.LoadScene(_nextSceneName);
    }
}
