using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SlowManager
{
    [SerializeField] float _slowSpeedRate = 0.05f;
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

    public void Add(ISlow slow)
    {
        _slows.Add(slow);
        if (_isSlow)
        {
            slow.OnSlow(_slowSpeedRate);
        }
    }
    public void Remove(ISlow slow)
    {
        _slows.Remove(slow);
    }
}
