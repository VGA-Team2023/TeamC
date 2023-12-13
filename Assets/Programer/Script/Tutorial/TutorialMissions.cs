using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialMissions
{
    [Header("歩きのチュートリアル")]
    [SerializeField] private TutorialMissionWalk _walkMission;

    [Header("カメラ視点変更のチュートリアル")]
    [SerializeField] private TutorialMissionCameraMove _cameraMission;

    [Header("攻撃のチュートリアル")]
    [SerializeField] private TutorialMissionAttack _attackMission;

    [Header("属性変更のチュートリアル")]
    [SerializeField] private TutorialMissionChangeAttribute _changeAttributeMission;

    [Header("トドメのチュートリアル")]
    [SerializeField] private TutorialMissionFinishAttack _finishAttackMission;

    [Header("ロックオンのチュートリアル")]
    [SerializeField] private TutorialMissionLockOn _lockOnMission;

    [Header("ロックオン敵変更のチュートリアル")]
    [SerializeField] private TutorialMissionLockOnEnemyChange _lockOnEnemyChangeMission;

    [Header("回避のチュートリアル")]
    [SerializeField] private TutorialMissionAvoid _avoidMission;

    [Header("オプションのチュートリアル")]
    [SerializeField] private TutorialMissionOption _optionMission;

    /// <summary>全てのチュートリアル</summary>
    private List<TutorialMissionBase> _tutorials = new List<TutorialMissionBase>();

    /// <summary>現在のミッション</summary>
    private TutorialMissionBase _currentTutorial;


    public List<TutorialMissionBase> Tutorials => _tutorials;
    public TutorialMissionBase CurrentTutorial { get => _currentTutorial; set => _currentTutorial = value; }
    public TutorialMissionWalk WalkMission => _walkMission;
    TutorialMissionCameraMove CameraMission => _cameraMission;
    TutorialMissionAttack AttackMission => _attackMission;
    public TutorialMissionChangeAttribute ChangeAttributeMission => _changeAttributeMission;
    public TutorialMissionFinishAttack FinishAttackMission => _finishAttackMission;
    public TutorialMissionLockOn lockOnMission => _lockOnMission;
    public TutorialMissionLockOnEnemyChange LockOnEnemyChangeMission => _lockOnEnemyChangeMission;
    public TutorialMissionAvoid AvoidMission => _avoidMission;
    public TutorialMissionOption OptionMission => _optionMission;


    public void Init(TutorialManager tutorialManager, InputManager inputManager)
    {
        _tutorials.Add(_walkMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_cameraMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_attackMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_changeAttributeMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_finishAttackMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_lockOnMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_lockOnEnemyChangeMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_avoidMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_optionMission.Init(tutorialManager, inputManager));
    }





}
