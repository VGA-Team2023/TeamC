﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerControl _control;

    private bool _isKeybord = false;

    /// <summary>構え押す</summary>
    private bool _isSetUpDown = false;

    /// <summary>構え、押し続ける</summary>
    private bool _isSetUp = false;

    /// <summary> 構え離す </summary>
    private bool _isSetUpUp = false;

    private bool _isFinishAttack = false;

    private bool _isFinishAttackDown = false;

    private bool _isAvoid = false;

    /// <summary>ジャンプ</summary>
    private bool _isJump;

    private bool _isAttack = false;
    private bool _isAttacks = false;

    private bool _isAttackUp = false;

    private float _horizontalInput;

    private float _verticalInput;

    private bool _isLockOn = false;

    private float _isChangeLockOnEnemy = 0;

    private bool _isChangeLockOnEnemyRight = false;

    private bool _isChangeLockOnEnemyLeft = false;

    private float _wheel;

    /// <summary>属性変更ボタンを押したかどうか </summary>
    private bool _isChangeAttribute = false;

    public bool IsChangeLockOnEnemyRight => _isChangeLockOnEnemyRight;
    public bool IsChangeLockOnEnemyLeft => _isChangeLockOnEnemyLeft;



    public bool IsChangeAttribute => _isChangeAttribute;
    public float IsChangeLockOnEney => _isChangeLockOnEnemy;
    public bool IsLockOn => _isLockOn;
    public float HorizontalInput => _horizontalInput;
    public float VerticalInput => _verticalInput;
    public bool IsAttack => _isAttack;
    public bool IsAttacks => _isAttacks;
    public bool IsAttackUp => _isAttackUp;
    public bool IsFinishAttack => _isFinishAttack;
    public bool IsFinishAttackDown => _isFinishAttackDown;
    public bool IsAvoid => _isAvoid;

    private float _saveTrigger;

    private bool _isTutorial = false;

    private bool _isPause = false;

    public bool IsPause => _isPause;

    public bool IsTutorial { get => _isTutorial; set => _isTutorial = value; }

    private TutorialInputControl _tutorialInputControl;
    public void SetTutorial(TutorialInputControl tutorialInputControl)
    {
        _tutorialInputControl = tutorialInputControl;
        _isTutorial = true;
    }

    private void Start()
    {
        if (_control.IsMousePlay)
        {
            _isKeybord = true;

        }
        else
        {
            _isKeybord = false;
        }
        if (!_isTutorial)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    private void Update()
    {
        _isPause = Input.GetButtonDown("Pause");

        if (!_isTutorial)
        {
            DefultInput();
        }
        else
        {
            Tutorial();
        }

    }

    public void DefultInput()
    {
        if (_control.IsNewAttack && !_isKeybord)
        {
            float v = Input.GetAxis("Trigger");
            if (v > 0)
            {
                _isAttacks = true;
            }
            else
            {
                _isAttacks = false;
            }


            if (_saveTrigger <= 0 && v > 0)
            {
                _isAttack = true;
            }
            else
            {
                _isAttack = false;
            }

            if (_saveTrigger > 0 && v <= 0)
            {
                _isAttackUp = true;
            }
            else
            {
                _isAttackUp = false;
            }

            _saveTrigger = v;
        }
        else
        {
            _isAttack = Input.GetButtonDown("Attack");
            _isAttackUp = Input.GetButtonUp("Attack");
            _isAttacks = Input.GetButton("Attack");
        }


        if (_control.IsMousePlay)
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");

            _isChangeLockOnEnemyRight = false;
            _isChangeLockOnEnemyLeft = false;
            if (_wheel == 0)
            {
                if (wheel > 0)
                {
                    _isChangeLockOnEnemyRight = true;
                }
                else if (wheel < 0)
                {
                    _isChangeLockOnEnemyLeft = true;
                }
            }
            _wheel = Input.GetAxis("Mouse ScrollWheel");
        }
        else
        {
            float wheel = Input.GetAxisRaw("ChengeLockOnEnemy");

            _isChangeLockOnEnemyRight = false;
            _isChangeLockOnEnemyLeft = false;
            if (_wheel == 0)
            {
                if (wheel > 0)
                {
                    _isChangeLockOnEnemyRight = true;
                }
                else if (wheel < 0)
                {
                    _isChangeLockOnEnemyLeft = true;
                }
            }
            _wheel = Input.GetAxisRaw("ChengeLockOnEnemy");
        }

        //属性変更
        _isChangeAttribute = Input.GetButtonDown("ChangeType");

        //ロックオン
        _isLockOn = Input.GetButtonDown("LockOn");

        //トドメ
        _isFinishAttack = Input.GetButton("FinishAttack");

        _isFinishAttackDown = Input.GetButtonDown("FinishAttack");

        //回避
        _isAvoid = Input.GetButtonDown("Avoid");

        //横入力
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        //縦入力
        _verticalInput = Input.GetAxisRaw("Vertical");
    }



    public void Tutorial()
    {
        _horizontalInput = 0;
        _verticalInput = 0;
        _isAvoid = false;
        _isChangeLockOnEnemy = 0;

        if (!_tutorialInputControl.TutorialManager.IsCanInput)
        {
            return;
        }

        //移動入力
        if (_tutorialInputControl.TutorialManager.TutorialMissions.CurrentTutorial.TutorialNum == TutorialNum.Walk)
        {
            //横入力
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            //縦入力
            _verticalInput = Input.GetAxisRaw("Vertical");
        }

        //回避
        if (_tutorialInputControl.TutorialManager.TutorialMissions.CurrentTutorial.TutorialNum == TutorialNum.Avoid)
        {
            _isAvoid = Input.GetButtonDown("Avoid");
        }

        //属性変更
        if (_tutorialInputControl.TutorialManager.TutorialMissions.CurrentTutorial.TutorialNum == TutorialNum.ChangeAttribute)
        {
            _isChangeAttribute = Input.GetButtonDown("ChangeType");
        }

        //とどめ
        if (_tutorialInputControl.TutorialManager.TutorialMissions.CurrentTutorial.TutorialNum == TutorialNum.FinishAttack)
        {
            _isFinishAttack = Input.GetButton("FinishAttack");
            _isFinishAttackDown = Input.GetButtonDown("FinishAttack");
        }

        //ロックオン
        if (_tutorialInputControl.TutorialManager.TutorialMissions.CurrentTutorial.TutorialNum == TutorialNum.LockOn)
        {
            _isLockOn = Input.GetButtonDown("LockOn");
        }

        //ロックオン敵変更
        if (_tutorialInputControl.TutorialManager.TutorialMissions.CurrentTutorial.TutorialNum == TutorialNum.LockOnChangeEnemy)
        {
            if (_control.IsMousePlay)
            {
                float wheel = Input.GetAxis("Mouse ScrollWheel");

                _isChangeLockOnEnemyRight = false;
                _isChangeLockOnEnemyLeft = false;
                if (_wheel == 0)
                {
                    if (wheel > 0)
                    {
                        _isChangeLockOnEnemyRight = true;
                    }
                    else if (wheel < 0)
                    {
                        _isChangeLockOnEnemyLeft = true;
                    }
                }
                _wheel = Input.GetAxis("Mouse ScrollWheel");
            }
            else
            {
                float wheel = Input.GetAxisRaw("ChengeLockOnEnemy");

                _isChangeLockOnEnemyRight = false;
                _isChangeLockOnEnemyLeft = false;
                if (_wheel == 0)
                {
                    if (wheel > 0)
                    {
                        _isChangeLockOnEnemyRight = true;
                    }
                    else if (wheel < 0)
                    {
                        _isChangeLockOnEnemyLeft = true;
                    }
                }
                _wheel = Input.GetAxisRaw("ChengeLockOnEnemy");
            }
        }


        //攻撃
        if (_tutorialInputControl.TutorialManager.TutorialMissions.CurrentTutorial.TutorialNum == TutorialNum.Attack
            || _tutorialInputControl.TutorialManager.TutorialMissions.CurrentTutorial.TutorialNum == TutorialNum.ChangeAttribute)
        {
            if (_control.IsNewAttack && !_isKeybord)
            {
                float v = Input.GetAxis("Trigger");

                if (v != 0) Debug.Log("GGGG");

                if (v > 0)
                {
                    _isAttacks = true;
                }
                else
                {
                    _isAttacks = false;
                }


                if (_saveTrigger <= 0 && v > 0)
                {
                    _isAttack = true;
                }
                else
                {
                    _isAttack = false;
                }

                if (_saveTrigger > 0 && v <= 0)
                {
                    _isAttackUp = true;
                }
                else
                {
                    _isAttackUp = false;
                }

                _saveTrigger = v;
            }
            else
            {
                _isAttack = Input.GetButtonDown("Attack");
                _isAttackUp = Input.GetButtonUp("Attack");
                _isAttacks = Input.GetButton("Attack");
            }

        }

    }

    public void EndAttack()
    {
        _isAttack = false;
        _isAttacks = false;
        _isAttackUp = true;
    }

    public void EndLockOn()
    {
        _isLockOn = true;
    }


}