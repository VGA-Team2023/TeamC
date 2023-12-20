using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialMissionLockOnEnemyChange : TutorialMissionBase
{
    [Header("チュートリアルの達成後の待機時間")]
    [SerializeField] private float _waitTime = 1;

    [Header("LockOn用のダミー")]
    [SerializeField] private List<GameObject> _dummy = new List<GameObject>();

    [Header("LockOn用のダミーの退場エフェクト")]
    [SerializeField] private List<GameObject> _dummyDeadEffect = new List<GameObject>();

    private float _countWaitTIme = 0;

    private bool _isLockOn = false;
    public override void Enter()
    {
        //入力を不可にする
        _tutorialManager.SetCanInput(true);
    }

    public override void Exit()
    {
        _dummy.ForEach(x => x.SetActive(false));
    }

    public override bool Updata()
    {
        if ((_inputManager.IsChangeLockOnEnemyLeft || _inputManager.IsChangeLockOnEnemyRight) && !_isLockOn)
        {
            _dummyDeadEffect.ForEach(x => x.SetActive(true));

            //入力を不可にする
            _tutorialManager.SetCanInput(false);
            _isLockOn = true;
        }


        if (_isLockOn)
        {
            _countWaitTIme += Time.deltaTime;

            if (_countWaitTIme >= _waitTime)
            {
                return true;
            }
            return false;
        }

        return false;
    }
}
