using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSumon : MonoBehaviour
{
    [Header("Boss‚ÌƒvƒŒƒnƒu")]
    [SerializeField] private GameObject _boss;

    [Header("“oê‚³‚¹‚é‚Ü‚Å‚ÌŠÔ")]
    [SerializeField] private float _summonTime = 1f;


    [Header("SpowmPoint")]
    [SerializeField] private List<Transform> _spownPoint = new List<Transform>();

    private bool _isSummon = false;
    private float _countSummonTime = 0;

    private Transform _player;

    void Start()
    {
   
    }

    void Update()
    {
        if (_isSummon) return;

        _countSummonTime += Time.deltaTime;

        if (_summonTime < _countSummonTime)
        {
            _isSummon = true;
            _boss.SetActive(true);
        }

    }
}
