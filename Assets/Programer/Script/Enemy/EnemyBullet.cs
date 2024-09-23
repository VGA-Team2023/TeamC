using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : MonoBehaviour, IPause, ISlow, ISpecialMovingPause
{
    [SerializeField,Tooltip("弾の速度")]
    float _bulletSpeed;
    [SerializeField, Tooltip("弾が当たった時のエフェクト")]
    GameObject _hitEffect;

    Vector3 _shootForward;
    int _damage;

    private Vector3 _savePauseVelocity;

    private Rigidbody _rb;

    public void Init(Vector3 forward, int damage)
    {
        _shootForward = (forward - transform.position).normalized;
        _damage = damage;
    }

    void Start()
    {
        transform.forward = _shootForward;
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(_shootForward * _bulletSpeed, ForceMode.Impulse);
        AudioController.Instance.SE.Play3D(SEState.EnemyLongAttackTrail, transform.position);
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        AudioController.Instance.SE.Update3DPos(SEState.EnemyLongAttackTrail, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerControl player))
        {
            player.Damage(_damage);
            Instantiate(_hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    { 
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SlowManager.Remove(this);
        GameManager.Instance.SpecialMovingPauseManager.Resume(this);
        AudioController.Instance.SE.Stop(SEState.EnemyLongAttackTrail);
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
        GameManager.Instance.SpecialMovingPauseManager.Add(this);
    }


    public void Pause()
    {
        _savePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    public void Resume()
    {
        _rb.isKinematic = false;
        _rb.velocity = _savePauseVelocity;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _rb.velocity = _shootForward * _bulletSpeed * slowSpeedRate;
    }

    public void OffSlow()
    {
        _rb.velocity = _shootForward * _bulletSpeed;
    }

    void ISpecialMovingPause.Pause()
    {
        _savePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    void ISpecialMovingPause.Resume()
    {
        _rb.isKinematic = false;
        _rb.velocity = _savePauseVelocity;
    }

}
