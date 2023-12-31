﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ControllerVibrationManager : MonoBehaviour
{

    [Header("最大パワー")]
    [SerializeField] private float _maxPower = 0.3f;

    [Header("最小パワー")]
    [SerializeField] private float _minPower = 0.1f;

    private Gamepad gamepad;

    private float _nowPower = 0;

    private void Start()
    {
        gamepad = Gamepad.current;
    }


    public void StartVibration()
    {
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.2f, 0.2f);
        }
    }

    public void DoVibration()
    {
        if (gamepad == null) return;

        if (_nowPower <= _maxPower)
        {
            _nowPower += Time.deltaTime;
        }   //Maxまでいって無かったら追加
        else
        {
            return;
        }

        gamepad.SetMotorSpeeds(_nowPower, _nowPower);
    }


    public void StopVibration()
    {
        if (gamepad == null) return;

        if (gamepad != null)
        {
            _nowPower = _minPower;
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    public void OneVibration(float time, float powerR, float powerL)
    {
        if (gamepad == null)
        {
            return;
        }
        StartCoroutine(Vibration(time, powerR, powerL));
    }

    public IEnumerator Vibration(float time, float powerR, float powerL)
    {
        gamepad.SetMotorSpeeds(powerL, powerR);

        yield return new WaitForSeconds(time);
        InputSystem.ResetHaptics();
    }

}
