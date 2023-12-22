using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DisplayTargetPointer : MonoBehaviour,ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image _targetImage;
    public Image TargetImage => _targetImage;
    public void OnSelect(BaseEventData eventData)
    {
        _targetImage.gameObject.SetActive(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        AudioController.Instance.SE.Play(SEState.SystemSelect);
        _targetImage.gameObject.SetActive(false);
    }
}
