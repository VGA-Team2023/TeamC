using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerChangeAttribute
{
    [Header("属性変更のクールタイム")]
    [SerializeField] private float _coolTime = 1;

    [Header("草属性のアイコン")]
    [SerializeField] private GameObject _playerGrassIcon;
    [Header("氷属性のアイコン")]
    [SerializeField] private GameObject _playerIceIcon;

    [Header("氷属性エフェクト")]
    [SerializeField] private List<ParticleSystem> _ice = new List<ParticleSystem>();

    [Header("草属性エフェクト")]
    [SerializeField] private List<ParticleSystem> _grass = new List<ParticleSystem>();


    [Header("氷属性の杖")]
    [SerializeField] private GameObject _iceWand;
    [Header("草属性の杖")]
    [SerializeField] private GameObject _grassWand;

    private PlayerAttribute _playerAttribute = PlayerAttribute.Ice;



    private float _count = 0;

    private bool _isCoolTime = true;
    public bool IsCoolTimeEnd => _isCoolTime;

    public PlayerAttribute PlayerAttribute => _playerAttribute;


    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void CoolTime()
    {
        if (_isCoolTime) return;
        _count += Time.deltaTime;

        if (_count > _coolTime)
        {
            _count = 0;
            _isCoolTime = true;
        }

    }

    //中断
    public void StopChange()
    {
        _count = 0;
        _isCoolTime = false;

        _grass.ForEach(i => i.Stop());
        _ice.ForEach(i => i.Stop());
    }

    /// <summary>属性変更処理</summary>
    public void ChangeAttribute()
    {
        _isCoolTime = false;

        _playerControl.PlayerAnimControl.ChangeAttribute();

        //コントローラーの振動
        _playerControl.ControllerVibrationManager.OneVibration(0.2f, 0.6f, 0.6f);

        if (_playerAttribute == PlayerAttribute.Ice)
        {
            AudioController.Instance.Voice.Play(VoiceState.PlayerAttributeChangeGrass);

            _grass.ForEach(i => i.Play());

            _playerAttribute = PlayerAttribute.Grass;
            _grassWand.SetActive(true);
            _iceWand.SetActive(false);
            _playerIceIcon.SetActive(false);
            _playerGrassIcon.SetActive(true);
        }
        else
        {
            AudioController.Instance.Voice.Play(VoiceState.PlayerAttributeChangeIce);

            _ice.ForEach(i => i.Play());

            _playerAttribute = PlayerAttribute.Ice;
            _iceWand.SetActive(true);
            _grassWand.SetActive(false);
            _playerGrassIcon.SetActive(false);
            _playerIceIcon.SetActive(true);
        }
    }

}
