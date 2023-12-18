using UnityEngine;
using UnityEngine.UI;

public class FromOptionToTitle : MonoBehaviour
{
    [SerializeField] private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => GameManager.Instance.ScoreManager.ScoreReset());
    }
}
