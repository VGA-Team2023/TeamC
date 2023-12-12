using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialMissionWalk : TutorialMissionBase
{
    [Header("目的地のコライダー")]
    [SerializeField] private GameObject _targetCollider;

    [Header("目的地のマーカー")]
    [SerializeField] private GameObject _marker;

    private bool _isEnd = false;



    public override void Enter()
    {
        _targetCollider.SetActive(true);
        _marker.SetActive(true);
        GameObject.FindObjectOfType<WalkTutorialEnterBox>().Set(this);
    }

    public override bool Updata()
    {
        if (_isEnd)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Exit()
    {
        _targetCollider.SetActive(false);
        _marker.SetActive(false);
    }

    public void End()
    {          
        //入力を不可にする
        _tutorialManager.SetCanInput(false);
        _isEnd = true;
    }

}
