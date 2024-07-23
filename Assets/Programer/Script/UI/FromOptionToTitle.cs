using UnityEngine;
using UnityEngine.UI;

public class FromOptionToTitle : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _panel;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>ReturnTitle()) ;
    }
    private void ReturnTitle()
    {
        _panel?.SetActive(false);
        GameManager.Instance.ScoreManager.ScoreReset();
    }
}
