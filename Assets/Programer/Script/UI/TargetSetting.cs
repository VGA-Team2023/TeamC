using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSetting : MonoBehaviour
{
    [SerializeField] GameObject _target;
    [SerializeField] EventSystem _eventSystem;
    private void OnEnable()
    {
        if (_eventSystem == null)
        {
            _eventSystem = FindObjectOfType<EventSystem>();
        }
        _eventSystem?.SetSelectedGameObject(_target);
        _target.GetComponent<ButtonTextColorChanger>()?.Target?.gameObject.SetActive(true);
    }
}
