using UnityEngine;

public class OptionPanelAdder : MonoBehaviour
{
    [SerializeField] private GameObject _inGameOptionPanel;
    [SerializeField] OptionPanelResetter _resetter;
    GameManager _gameManager;
    private void OnEnable()
    {
        var tmp = _inGameOptionPanel.GetComponent<IPause>();
        _gameManager = GameManager.Instance;
        _gameManager.PauseManager.Add(tmp);
        _inGameOptionPanel.SetActive(false);
    }
    private void OnDisable()
    {
        _gameManager.PauseManager.Remove(_inGameOptionPanel.GetComponent<IPause>());
    }
}
