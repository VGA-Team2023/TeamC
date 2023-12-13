using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerChangeAttribute
{
    [Header("草属性のアイコン")]
    [SerializeField] private GameObject _playerGrassIcon;
    [Header("氷属性のアイコン")]
    [SerializeField] private GameObject _playerIceIcon;

    [Header("氷属性の杖")]
    [SerializeField] private GameObject _iceWand;

    [Header("草属性の杖")]
    [SerializeField] private GameObject _grassWand;

    private PlayerAttribute _playerAttribute = PlayerAttribute.Ice;
    public PlayerAttribute PlayerAttribute => _playerAttribute;


    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void CheckChangeAttribute()
    {
        if (_playerControl.InputManager.IsChangeAttribute)
        {
            if (_playerAttribute == PlayerAttribute.Ice)
            {
                _playerAttribute = PlayerAttribute.Grass;
                _grassWand.SetActive(true);
                _iceWand.SetActive(false);
                _playerIceIcon.SetActive(false);
                _playerGrassIcon.SetActive(true);
            }
            else
            {
                _playerAttribute = PlayerAttribute.Ice;
                _iceWand.SetActive(true);
                _grassWand.SetActive(false);
                _playerGrassIcon.SetActive(false);
                _playerIceIcon.SetActive(true);
            }
        }
    }

}
