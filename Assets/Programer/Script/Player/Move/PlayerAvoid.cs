using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAvoid
{
    [Header("回避の移動設定")]
    [SerializeField] private PlayerAvoidMove _avoidMove;

    [Header("回避時間")]
    [SerializeField] private float _avoidTime = 0.5f;

    [Header("通常のプレイヤーのマテリアル")]
    [SerializeField] private Material _defaultMaterial;

    [Header("回避中のプレイヤーのマテリアル")]
    [SerializeField] private Material _avoidMaterial;

    [Header("回避終了時のエフェクト")]
    [SerializeField] private List<ParticleSystem> _endParticle = new List<ParticleSystem>();

    [SerializeField] private GameObject _dummy;


    private Vector3 _dir = default;

    private float _countAvoidTime = 0;

    private bool _isEndAvoid = false;

    private bool _isEndAnimation = false;

    private PlayerControl _playerControl;


    public bool IsEndAnim => _isEndAnimation;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _avoidMove.Init(playerControl);
    }

    public void SetAvoidDir()
    {
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        if (h == 0 && v == 0)
        {
            v = -1;
        }

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        _dir = horizontalRotation * new Vector3(h, 0, v).normalized;

        _avoidMove.SetAvoidDir(_dir);
    }


    /// <summary>回避を開始</summary>
    public void StartAvoid()
    {
        _isEndAvoid = false;
        _isEndAnimation = false;
        _countAvoidTime = 0;

        var go = UnityEngine.GameObject.Instantiate(_dummy);
        go.transform.position = _playerControl.PlayerT.position;

        _playerControl.PlayerAnimControl.Avoid(true);
        _avoidMove.StartAvoid(_playerControl.PlayerT.position);

    }

    /// <summary>回避の開始アニメーションが終わった事を通知</summary>
    public void StartAvoidAnim()
    {
        _playerControl.MeshRenderer.material = _avoidMaterial;
    }

    /// <summary>回避のアニメーションが終わった事を通知</summary>
    public void EndAvoidAnim()
    {
        _isEndAnimation = true;
    }


    /// <summary>回避を完了</summary>
    public void EndMove()
    {
        _playerControl.MeshRenderer.material = _defaultMaterial;
        _playerControl.PlayerAnimControl.Avoid(false);
        _isEndAvoid = true;


        foreach (var a in _endParticle)
        {
            a.Play();
        }

    }

    /// <summary>回避中の実行</summary>
    public void DoAvoid()
    {
        if (_isEndAvoid) return;

        if (_avoidMove.Move())
        {
            EndMove();
        }
    }


    /// <summary>回避の実行時間を計測する/summary>
    public void CountAvoidTime()
    {
        if (_isEndAvoid) return;

        _countAvoidTime += Time.deltaTime;

        if (_countAvoidTime > _avoidTime)
        {
            EndMove();
        }
    }



}