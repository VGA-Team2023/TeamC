using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TutorialMissionBase 
{
    [Header("チュートリアル")]
    [SerializeField] private TutorialNum _tutorialNum;

    [Header("説明データ")]
    [SerializeField] protected TutorialTalkData _talkData;
    public TutorialTalkData TalkData =>_talkData;

    public TutorialNum TutorialNum => _tutorialNum;

    [NonSerialized]
    protected TutorialManager _tutorialManager;

    public TutorialMissionBase Init(TutorialManager tutorialManager)
    {
        _tutorialManager = tutorialManager;
        return this;
    }


    public abstract void Enter();

    public abstract bool Updata();

    public abstract void Exit();


}
