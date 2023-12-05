using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>ボタン用音再生スクリプト</summary>
public class ButtonOnClickAudio : MonoBehaviour
{
    Button _button;
    [SerializeField, Tooltip("True時は決定音・False時はキャンセル音が再生されます")] bool _applySESound;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => AudioManager.Instance.ButtonSEPlay(_applySESound));
    }

}
