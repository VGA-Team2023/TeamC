using UnityEngine;
using CriWare;
using UnityEngine.UI;

public class ButtonCancelSound : MonoBehaviour
{
    private Button _button;
    private AudioController _audioController;
    private void Start()
    {
        _audioController = AudioController.Instance;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => AudioController.Instance.SE.Play(SEState.SystemCancel));
    }
}
