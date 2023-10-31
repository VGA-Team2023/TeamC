using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerHpUI
{
    [Header("Hp‚ÌSlider")]
    [SerializeField] private Slider _slider;

    private PlayerControl _playerControl;
    public void Init(PlayerControl playerControl, float maxHp)
    {
        _playerControl = playerControl;
        _slider.maxValue = maxHp;
        _slider.minValue = 0;
        _slider.value = maxHp;
    }

    /// <summary>Slider‚Ì’l‚ğİ’è</summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        _slider.value = value;
    }


}
