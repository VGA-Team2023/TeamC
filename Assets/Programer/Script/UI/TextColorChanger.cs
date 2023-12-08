using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextColorChanger : MonoBehaviour
{
    private EventSystem _eventSystem;
    private Color _selectedColor = Color.white;
    private Color _defaultColor = Color.white;
    private GameObject _target = null;
    void Start()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        if (ColorUtility.TryParseHtmlString("#FF4500", out Color selectedcolor))
        {
            _selectedColor = selectedcolor;
        }
        if (ColorUtility.TryParseHtmlString("#220000", out Color defaltcolor))
        {
            _defaultColor = defaltcolor;
        }
        _target = _eventSystem.firstSelectedGameObject;
        _target.GetComponentInChildren<Text>().color = _selectedColor;
    }
    private void Update()
    {        
        GameObject selectedObj = _eventSystem.currentSelectedGameObject?.gameObject;
 
        if (_target != selectedObj)
        {
            SetTextColor(_target, _defaultColor);
            _target = selectedObj;
            SetTextColor(_target, _selectedColor);
        }
    }
    private void OnPointerEnter(BaseEventData eventData)
    {
        SetTextColor(_target, _selectedColor);
    }

    private void OnPointerExit(BaseEventData eventData)
    {
        SetTextColor(_target, _defaultColor);
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
