using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DisplayTargetPointer : MonoBehaviour,ISelectHandler, IDeselectHandler
{
    [SerializeField] Image _targetImage;
    public void OnSelect(BaseEventData eventData)
    {
        _targetImage.gameObject.SetActive(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        _targetImage.gameObject.SetActive(false);
    }
}
