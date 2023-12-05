using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAvoid
{
    [Header("---@回避の移動設定---")]
    [SerializeField] private PlayerAvoidMove _avoidMove;

    [Header("@回避時間")]
    [SerializeField] private float _avoidTime = 0.5f;

    [Header("回避中のプレイヤーのマテリアル")]
    [SerializeField] private Material _avoidMaterial;

    [Header("Playerの顔のMesh")]
    [SerializeField] private List<SkinnedMeshRenderer> _meshRendererFace = new List<SkinnedMeshRenderer>();

    [Header("通常のプレイヤーのマテリアル_Face")]
    [SerializeField] private Material _defaultMaterialFace;

    [Header("Playerの体のMesh")]
    [SerializeField] private List<SkinnedMeshRenderer> _meshRendererBody = new List<SkinnedMeshRenderer>();

    [Header("通常のプレイヤーのマテリアル_Body")]
    [SerializeField] private Material _defaultMaterialBody;

    [Header("杖のMesh")]
    [SerializeField] private List<MeshRenderer> _tueMesh = new List<MeshRenderer>();

    [Header("杖のマテリアル")]
    [SerializeField] private List<Material> _tueMaterial = new List<Material>();


    [Header("回避終了時のエフェクト_氷")]
    [SerializeField] private List<ParticleSystem> _endParticleIce = new List<ParticleSystem>();

    [Header("回避終了時のエフェクト_草")]
    [SerializeField] private List<ParticleSystem> _endParticleGrass = new List<ParticleSystem>();

    [Header("ダミー_氷")]
    [SerializeField] private GameObject _dummyIce;

    [Header("ダミー_草")]
    [SerializeField] private GameObject _dummyGrass;

    private PlayerAttribute _startAttribute;

    private Vector3 _dir = default;

    private bool _isStartAvoid = false;

    private float _countAvoidTime = 0;

    private bool _isEndAvoid = false;

    private bool _isEndAnimation = false;

    private PlayerControl _playerControl;


    public bool IsStartAvoid => _isStartAvoid;
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
        _startAttribute = _playerControl.PlayerAttribute;

        _isStartAvoid = false;
        _isEndAvoid = false;
        _isEndAnimation = false;
        _countAvoidTime = 0;

        if (_startAttribute == PlayerAttribute.Ice)
        {
            var go = UnityEngine.GameObject.Instantiate(_dummyIce);
            go.transform.position = _playerControl.PlayerT.position;
        }
        else
        {
            var go = UnityEngine.GameObject.Instantiate(_dummyGrass);
            go.transform.position = _playerControl.PlayerT.position;
        }


        _playerControl.PlayerAnimControl.Avoid(true);
        _avoidMove.StartAvoid(_playerControl.PlayerT.position);

    }

    /// <summary>回避の開始アニメーションが終わった事を通知</summary>
    public void StartAvoidAnim()
    {
        foreach (var m in _meshRendererFace)
        {
            m.material = _avoidMaterial;
        }

        foreach (var m in _meshRendererBody)
        {
            m.material = _avoidMaterial;
        }

        foreach(var m in _tueMesh)
        {
            m.material = _avoidMaterial;
        }

        _isStartAvoid = true;
    }

    /// <summary>回避のアニメーションが終わった事を通知</summary>
    public void EndAvoidAnim()
    {
        _isEndAnimation = true;
    }


    /// <summary>回避を完了</summary>
    public void EndMove()
    {
        _avoidMove.MoveEnd();

        foreach (var m in _meshRendererFace)
        {
            m.material = _defaultMaterialFace;
        }

        foreach (var m in _meshRendererBody)
        {
            m.material = _defaultMaterialBody;
        }

        for(int i =0; i<_tueMesh.Count;i++)
        {
            _tueMesh[i].material = _tueMaterial[i];
        }

        _playerControl.PlayerAnimControl.Avoid(false);
        _isEndAvoid = true;


        if (_startAttribute == PlayerAttribute.Ice)
        {
            foreach (var a in _endParticleIce)
            {
                a.Play();
            }
        }
        else
        {
            foreach (var a in _endParticleGrass)
            {
                a.Play();
            }
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