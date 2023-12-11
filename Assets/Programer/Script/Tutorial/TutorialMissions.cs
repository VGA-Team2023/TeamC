using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialMissions
{
    [SerializeField] private TutorialMissionWalk _walkMission;


    
    /// <summary>全てのチュートリアル</summary>
    private List<TutorialMissionBase> _tutorials = new List<TutorialMissionBase>();

    private TutorialMissionBase _currentTutorial;

    public List<TutorialMissionBase> Tutorials => _tutorials;
    public TutorialMissionBase CurrentTutorial { get => _currentTutorial; set => _currentTutorial = value; }

    public TutorialMissionWalk WalkMission => _walkMission;

    public void Init(TutorialManager tutorialManager,InputManager inputManager)
    {
        _tutorials.Add(_walkMission.Init(tutorialManager,inputManager));
    }





}
