using UnityEngine;

public class TestActionEnemy : MonoBehaviour
{
    [SerializeField]
    MeleeAttackEnemy _meleeEnemy;

    [SerializeField]
    LongAttackEnemy _longEnemy;
    [SerializeField]
    bool _testOn = false;

    private void Start()
    {
        TryGetComponent(out _meleeEnemy);
        TryGetComponent(out _longEnemy);
    }

    void Update()
    {
        if (_testOn)
        {
            if (_meleeEnemy)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _meleeEnemy.Damage(AttackType.ShortChantingMagick, MagickType.Ice, 3f);
                }
                if (gameObject.layer == 10 && Input.GetMouseButtonDown(1))
                {
                    _meleeEnemy.EndFinishing(MagickType.Ice);
                }
            }
            if (_longEnemy)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _longEnemy.Damage(AttackType.ShortChantingMagick, MagickType.Ice, 3f);
                }
                if (gameObject.layer == 10 && Input.GetMouseButtonDown(1))
                {
                    _longEnemy.EndFinishing(MagickType.Ice);
                }
            }
        }
    }
}
