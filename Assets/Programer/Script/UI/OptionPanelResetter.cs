using UnityEngine;
using UnityEngine.UI;

public class OptionPanelResetter : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainUi = null;
    [SerializeField]
    private GameObject _settingPanel = null;
    [SerializeField]
    private GameObject _helpUI = null;
    [SerializeField]
    private GameObject[] _pointers;
    [SerializeField]
    private Text[] _resetTexts;
    [SerializeField]
    private Color _defaltcolor;
    [SerializeField]
    private Color _selectColor;
    [SerializeField]
    private Text _firstSelectedText;
    public void Reset()
    {
        _settingPanel?.SetActive(true);
        _helpUI?.SetActive(false);
        foreach (var point in _pointers)
        {
            point?.SetActive(false);
        }
        foreach (var text in _resetTexts)
        {
            if (text == _firstSelectedText)
            {
                text.color = _selectColor;
            }
            else
            {
                text.color = _defaltcolor;
            }
        }
    }
}
