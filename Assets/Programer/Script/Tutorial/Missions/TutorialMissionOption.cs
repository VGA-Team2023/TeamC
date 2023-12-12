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
            if(_pausePanel.activeSelf)
            {
                _pausePanel.SetActive(false);
            }
            else
            {
                _pausePanel.SetActive(true);
            }
        }

        if (_count == 0 && _pausePanel.activeSelf)
        {
            _count++;
        }
        else if (_count == 1 && !_pausePanel.activeSelf)
        {
            //“ü—Í‚ð•s‰Â‚É‚·‚é
            _tutorialManager.SetCanInput(false);
            return true;
        }

        return false;
    }
}
