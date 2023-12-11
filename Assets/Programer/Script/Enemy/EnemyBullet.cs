using UnityEditor.ShaderGraph;
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

    public void Init(Vector3 forward, int damage)
    {
        _shootForward = (forward - transform.position).normalized;
        _damage = damage;
    }

    void Start()
    {
        transform.forward = _shootForward;
        GetComponent<Rigidbody>().AddForce(_shootForward * _bulletSpeed, ForceMode.Impulse);
        AudioController.Instance.SE.Play3D(SEState.EnemyLongAttackTrail, transform.position);
        Destroy(gameObject, 10f);
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
        AudioController.Instance.SE.Stop(SEState.EnemyLongAttackTrail);
    }
}
