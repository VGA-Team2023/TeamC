using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialMissionOption : TutorialMissionBase
{
    [SerializeField] private GameObject _pausePanel;

    private int _count;

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override bool Updata()
    {
        if (_inputManager.IsPause)
        {
            _count++;
        }

        if (_count == 2)
        {
            //“ü—Í‚ð•s‰Â‚É‚·‚é
            _tutorialManager.SetCanInput(false);
            return true;
        }

        return false;
    }
}
