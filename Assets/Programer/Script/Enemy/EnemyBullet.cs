using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    float _bulletSpeed;
    Rigidbody _rb;
    Vector3 _shootForward;
    public Vector3 ShootForward { get => _shootForward; set => _shootForward = value; }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(_shootForward * _bulletSpeed, ForceMode.Impulse);
    }
}
