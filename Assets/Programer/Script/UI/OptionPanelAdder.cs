using UnityEngine;

public class OptionPanelAdder : MonoBehaviour
{
    [SerializeField] private GameObject _inGameOptionPanel;
    private void OnEnable()
    {
        var tmp = _inGameOptionPanel.GetComponent<IPause>();
        GameManager.Instance.PauseManager.Add(tmp);
    }
    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(_inGameOptionPanel.GetComponent<IPause>());
    }
}
