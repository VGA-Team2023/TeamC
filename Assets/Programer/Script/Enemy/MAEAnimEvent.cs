using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAEAnimEvent : MonoBehaviour
{
    [Header("攻撃判定用のBox")]
    [SerializeField] private GameObject _attackBox;

    [Header("死亡エフェクト")]
    [SerializeField] private GameObject _deathEffect;

    [Header("敵モデル")]
    [SerializeField] private List<GameObject> _model = new List<GameObject>();

    [SerializeField] private MeleeAttackEnemy _enemy;



    public void AttackOn()
    {
        _attackBox.SetActive(true);
    }

    public void AttackOff()
    {
        _attackBox.SetActive(false);
    }


    public void AttackEffect()
    {
        _enemy.AttackEffectPlay();
    }

    public void DeathEffect()
    {
        var go = Instantiate(_deathEffect);
        go.transform.position = transform.position;
    }

    public void ModelOff()
    {
        _model.ForEach(i => i.SetActive(false));
    }

}
