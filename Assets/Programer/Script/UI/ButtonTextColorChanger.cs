using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTextColorChanger : MonoBehaviour, ISelectHandler, IDeselectHandler
{   
    private Color _selectedColor = Color.white;
    private Color _defaultColor = Color.white;
    private Text _text;
    [SerializeField] private Button _button;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Image _target = default;
    void Start()
    {
        if (_button != null)
        {
            Selectable buttonSelectable = _button.GetComponent<Selectable>();

            if (buttonSelectable != null)
            {
                buttonSelectable.OnSelect(null);
            }
        }
        if (ColorUtility.TryParseHtmlString("#FF4500", out Color selectedcolor))
        {
            _selectedColor = selectedcolor;
        }
        if (ColorUtility.TryParseHtmlString("#220000", out Color defaltcolor))
        {
            _defaultColor = defaltcolor;
        }
        _text = GetComponentInChildren<Text>();
    }
    public void OnSelect(BaseEventData eventData)
    {
        SetTextColor(_button.gameObject, _selectedColor);
        _target.gameObject.SetActive(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        SetTextColor(_button.gameObject, _defaultColor);
        _target.gameObject.SetActive(false);
    }
    private void SetTextColor(GameObject target, Color color)
    {
        if (target != null)
        {
            Text textComponent = target.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.color = color;
            }
        }
    }
}