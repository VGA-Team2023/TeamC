using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonDisplayImage : MonoBehaviour,ISelectHandler,IDeselectHandler
{
    [SerializeField] private Image _displayImage;
    public void OnSelect(BaseEventData eventData)
    {
        _displayImage.gameObject.SetActive(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        _displayImage.gameObject.SetActive(false);
    }
}
