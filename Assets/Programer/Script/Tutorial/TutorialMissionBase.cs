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

    [Header("説明のUI")]
    [SerializeField] private GameObject _infoUI;

    protected InputManager _inputManager;

    public GameObject InfoUI => _infoUI;
    public TutorialTalkData TalkData => _talkData;
    public TutorialNum TutorialNum => _tutorialNum;
    protected InputManager InputManager => _inputManager;
    [NonSerialized]
    protected TutorialManager _tutorialManager;

    public TutorialMissionBase Init(TutorialManager tutorialManager, InputManager inputManager)
    {
        _inputManager = inputManager;
        _tutorialManager = tutorialManager;
        return this;
    }


    public abstract void Enter();

    public abstract bool Updata();

    public abstract void Exit();

    public void InfoUIActive(bool isOn)
    {
        if (isOn)
        {
            _infoUI.SetActive(isOn);
        }
        else
        {
            _infoUI.GetComponent<Animator>().SetTrigger("End");
        }

    }
}
