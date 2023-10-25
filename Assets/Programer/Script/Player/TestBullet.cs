using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 7;


    void Start()
    {
        Destroy(gameObject, _lifeTime);        
    }


}
