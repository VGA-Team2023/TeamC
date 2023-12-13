using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTutorialAnimEvents : MonoBehaviour
{
    [SerializeField] private TutorialManager _tutorialManager; 
    void Start()
    {

    }

    public void WalkFadeOut()
    {
        _tutorialManager.TutorialMissions.WalkMission.EndAnim();
    }

    public void WalkSetpos()
    {
        _tutorialManager.TutorialMissions.WalkMission.SetPos();
    }

 
}
