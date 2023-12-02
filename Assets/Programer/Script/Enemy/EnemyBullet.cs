using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField,Tooltip("弾の速度")]
    float _bulletSpeed;
    [SerializeField, Tooltip("弾が当たった時のエフェクト")]
    GameObject _hitEffect;

    Vector3 _shootForward;
    int _damage;

    public void Init(Vector2 forward, int damage)
    {
        _shootForward = forward;
        _damage = damage;
    }

    void Start()
    {
        transform.forward = _shootForward;
        //GetComponent<Rigidbody>().AddForce(new Vector3(_shootForward.x, 0, _shootForward.z) * _bulletSpeed, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(_shootForward * _bulletSpeed, ForceMode.Impulse);
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
}
