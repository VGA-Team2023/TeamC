using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButtonController : MonoBehaviour, ISelectHandler
{
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _returnTarget = null;
    [SerializeField] GameObject _targetObj;
    EventSystem _eventSystem;
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
            _eventSystem.SetSelectedGameObject(_targetObj);
            if (_targetObj.TryGetComponent<DisplayTargetPointer>(out var displayTargetPointer))
                displayTargetPointer.TargetImage.gameObject.SetActive(true);
        }
    }
}
