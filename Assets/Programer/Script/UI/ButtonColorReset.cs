using UnityEngine;
using UnityEngine.UI;

public class ButtonColorReset : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] Color _startColor;
    private void OnEnable()
    {
        _text = GetComponentInChildren<Text>();
        _text.color = _startColor;
    }
}
