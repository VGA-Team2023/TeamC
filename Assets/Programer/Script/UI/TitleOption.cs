using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleOption : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private GameObject _returnTarget;
    [SerializeField] private EventSystem _eventSystem;
    private void OnEnable()
    {
        foreach (var button in _buttons)
        {
            button.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.gameObject.SetActive(true);
        }
        _eventSystem.SetSelectedGameObject(_returnTarget);
        _returnTarget.GetComponent<DisplayTargetPointer>()?.TargetImage.gameObject.SetActive(true);
    }
}
