using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class RotateFrame : MonoBehaviour
{
    [SerializeField] private Image _rotateImage;
    private void OnEnable()
    {
        _rotateImage.transform.DORotate(new Vector3(0,0,1)*90f,1f);
    }
}
