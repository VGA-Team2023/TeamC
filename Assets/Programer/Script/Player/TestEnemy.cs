using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, IEnemyDamageble, IFinishingDamgeble
{
    [SerializeField] private Transform _player;

    [SerializeField] private Rigidbody _rb;

    [Header("動きのタイプ")]
    [SerializeField] private bool _isGun = false;

    [Header("エフェクト弱")]
    [SerializeField] private GameObject pLow;

    [Header("エフェクト強")]
    [SerializeField] private GameObject pHigh;

    [Header("体力")]
    [SerializeField] private int _maxHp = 3;

    [Header("通常のレイヤー")]
    [SerializeField] private int _defaltLayerNum = 8;

    [Header("弱点時ののレイヤー")]
    [SerializeField] private int _lowLayerNum = 9;

    [Header("撃破時のエフェクト")]
    [SerializeField] private GameObject _downEffect;

    [Header("コア")]
    [SerializeField] private GameObject _core;

    [Header("弾")]
    [SerializeField] private GameObject _bullet;

    [Header("マズル")]
    [SerializeField] private Transform _muzzle;
    [Header("回転速度")]
    [SerializeField] private float _rotationSpeed = 200;

    private int _nowHp = 3;

    private float _destroyTime = 2f;

    private float _coundDestroyTime = 0;

    private bool _isDestroy = false;

    private bool _isOnGround = false;

    private float _countTime = 0;


    private void Awake()
    {
        _nowHp = _maxHp;
    }


    private void Update()
    {
        if (_isDestroy)
        {
            _coundDestroyTime += Time.deltaTime;

            if (_coundDestroyTime > _destroyTime)
            {
                var go = Instantiate(_downEffect);
                go.transform.position = transform.position;

                CameraControl camera = FindObjectOfType<CameraControl>();
                camera?.ShakeCamra(CameraType.All, CameraShakeType.AttackNomal);


                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_rb.velocity.y < -0.2f || _nowHp <= 0 || _isDestroy)
        {
            return;
        }
        Vector3 dirP = _player.position - transform.position;
        dirP.y = 0;
        Quaternion _targetRotation = Quaternion.LookRotation(dirP, Vector3.up);

        Quaternion setR = Quaternion.RotateTowards(transform.rotation, _targetRotation, Time.deltaTime * _rotationSpeed);
        setR.x = 0;
        setR.z = 0;
        transform.rotation = setR;


        if (_isGun)
        {
            _countTime += Time.deltaTime;

            if (_countTime > 5)
            {
                var go = Instantiate(_bullet);
                go.transform.position = _muzzle.position;
                Vector3 dir = _player.position - _muzzle.position;

                go.GetComponent<Rigidbody>().velocity = dir.normalized * 4f;

                _countTime = 0;
            }
        }
        else
        {
            Vector3 dir = _player.position - transform.position;

            _rb.velocity = dir.normalized * 3f;

            if (Vector3.Distance(_player.position, transform.position) < 4)
            {
                _rb.velocity = Vector3.zero;
            }
        }
    }


    public void Damage(AttackType attackType, MagickType attackHitTyp,float damage)
    {
        _rb.velocity = Vector3.zero;

        if (attackType == AttackType.ShortChantingMagick)
        {
            if (attackHitTyp == MagickType.Ice)
            {
                pLow.gameObject.SetActive(true);
                // pLow.Play();

                Vector3 dir = transform.position - _player.position;
                _rb.AddForce(((dir.normalized / 2) + (Vector3.up * 0.5f)) * 5, ForceMode.Impulse);
            }
            else if (attackHitTyp == MagickType.Grass)
            {
                pHigh.gameObject.SetActive(true);
                // pHigh.Play();

                Vector3 dir = transform.position - _player.position;
                _rb.AddForce((dir.normalized / 2 + Vector3.up) * 5, ForceMode.Impulse);
            }

            _nowHp--;

            if (_nowHp <= 0)
            {
                gameObject.layer = _lowLayerNum;
                _core.SetActive(true);
            }
        }
        else
        {
            _nowHp--;

            pLow.gameObject.SetActive(true);

            if (_nowHp <= 0)
            {
                gameObject.layer = _lowLayerNum;
                _core.SetActive(true);
            }
        }
    }

    public void EndFinishing()
    {
        _rb.velocity = Vector3.zero;
        pHigh.gameObject.SetActive(true);
        Vector3 dir = transform.position - _player.position;
        _rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);

        gameObject.layer = _defaltLayerNum;
        _core.SetActive(false);
        _nowHp = _maxHp;

        _isDestroy = true;

        gameObject.layer = 0;
    }

    public void StartFinishing()
    {

    }

    public void StopFinishing()
    {
        gameObject.layer = _defaltLayerNum;
        _core.SetActive(false);
        _nowHp = _maxHp;
    }
}
