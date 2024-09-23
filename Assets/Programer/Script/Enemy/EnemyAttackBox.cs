using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    [Header("çUåÇóÕ")]
    [SerializeField] private float _attackPower = 5;

    private PlayerControl _playerControl;

    private void OnEnable()
    {
        _playerControl = GameObject.FindAnyObjectByType<PlayerControl>();
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENter");
        var player = other.TryGetComponent<PlayerControl>(out PlayerControl playerControl);

        if (playerControl != null)
        {
            Debug.Log("HHIIT");
            playerControl.Damage(_attackPower);
        }


    }
}
