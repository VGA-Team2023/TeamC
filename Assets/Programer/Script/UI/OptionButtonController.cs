using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionButtonController : MonoBehaviour, ISelectHandler
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _returnTarget = null;
    [SerializeField] private GameObject _targetObj;
    private EventSystem _eventSystem = null;
    private void OnEnable()
    {
        if (_eventSystem == null)
        {
            _eventSystem = FindObjectOfType<EventSystem>();
        }
    }
    public void OnSelect(BaseEventData eventData)
    {
        if (_panel != null) _panel.SetActive(true);
    }
    public void ClosePickMenu()
    {
        _eventSystem.SetSelectedGameObject(_returnTarget);
        _panel.SetActive(false);
    }
    public void TargetChange()
    {
        if (_targetObj != null)
        {
            //_eventSystem.SetSelectedGameObject(_targetObj);
            _targetObj.GetComponent<Button>()?.Select();
            if (_targetObj.TryGetComponent<DisplayTargetPointer>(out var displayTargetPointer))
                displayTargetPointer.TargetImage.gameObject.SetActive(true);
            if (_targetObj.TryGetComponent<ButtonTextColorChanger>(out var buttonTextColorChanger))
                buttonTextColorChanger.Target.gameObject.SetActive(true);
        }
    }
}
