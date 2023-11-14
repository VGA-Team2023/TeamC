using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SlowManager
{
    [SerializeField,Tooltip("スロー速度の割合"),Range(0,1)] float _slowSpeedRate = 0.05f;
    bool _isSlow = false;
    List<ISlow> _slows = new List<ISlow>();
 
    /// <summary>スローの切り替え処理を行う</summary>
    /// <param name="isSlow">スローにするかどうか</param>
    public void OnOffSlow(bool isSlow)
    {
        _isSlow = isSlow;
        foreach(ISlow slow in _slows)
        {
            if(_isSlow)
            {
                slow.OnSlow(_slowSpeedRate);
            }
            else
            {
                slow.OffSlow();
            }
        }
    }

    /// <summary>登録</summary>
    ///<param name="slow">自分</param>
    public void Add(ISlow slow)
    {
        _slows.Add(slow);
        if (_isSlow)
        {
            slow.OnSlow(_slowSpeedRate);
        }
    }

    /// <summary>解除</summary>
    /// <param name="slow">自分</param>
    public void Remove(ISlow slow)
    {
        _slows.Remove(slow);
    }
}
