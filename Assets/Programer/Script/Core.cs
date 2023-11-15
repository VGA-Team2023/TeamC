using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField]
    GameObject _coreEffect;
    [SerializeField]
    float _radius;
    [SerializeField]
    float _thetaSpeed;
    [SerializeField]
    float _phiSpeed;
    void Update()
    {
        float x = transform.position.x + _radius * Mathf.Cos(Time.time * _thetaSpeed);
        float y = transform.position.x + _radius * Mathf.Sin(Time.time * _thetaSpeed) * Mathf.Cos(Time.time * _thetaSpeed);
        float z = 0;
        _coreEffect.transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
    }
}
