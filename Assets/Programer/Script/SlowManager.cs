using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowManager : MonoBehaviour
{
    [Header("設定")]
    [SerializeField, Tooltip("スロー時の再生速度の割合"), Range(0,1)] float _slowSpeedRate;
    /// <summary>通常からスローに切り替わる時に使うAction</summary>
    event Action<float> ChangeSlowSpeed;
    /// <summary>スローから通常に切り替わる時に使うAction</summary>
    event Action ChangeNormalSpeed;
    /// <summary>通常からスローに切り替わる時に使うAction/第一パラメータはスロー時の再生速度の割合</summary>
    public Action<float> OnChangeSlowSpeed { get {return ChangeSlowSpeed;} set { ChangeSlowSpeed = value; } }
    /// <summary>スローから通常に切り替わる時に使うAction</summary>
    public Action OnChangeNormalSpeed { get { return ChangeNormalSpeed; } set { ChangeNormalSpeed = value; } }

    /// <summary>スローの切り替え処理を行う</summary>
    /// <param name="isSlow">スローにするかどうか</param>
    public void OnOffSlow(bool isSlow)
    {
        //何も入ってなかったら
        if (ChangeSlowSpeed == null) return;
        if(isSlow)
        {
            //スロー
            ChangeSlowSpeed.Invoke(_slowSpeedRate);
        }
        else
        {
            //通常
            ChangeNormalSpeed.Invoke();
        }
    }
}
