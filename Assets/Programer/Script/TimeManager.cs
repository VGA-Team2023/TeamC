using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>ゲームの経過時間を操作するClass</summary>
public class TimeManager : MonoBehaviour
{
    /// <summary>ゲーム中の経過時間</summary>
    float _elapsedTime = 0;
    public float ElapsedTime => _elapsedTime;

    void Update()
    {
        _elapsedTime += Time.deltaTime;
    }
}
