using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>必殺時の一時停止を管理するクラス</summary>
public class SpecialMovingPauseManager
{
    List<ISpecialMovingPause> _specialMovingPauseList = new List<ISpecialMovingPause>();
    bool _isPause = false;
    /// <summary>現在一時停止しているかどうか(読み取り専用)</summary>
    public bool IsPaused => _isPause;

    /// <summary>必殺時の停止と通常の切り替え関数</summary>
    /// <param name="isPause">一時停止するかどうか</param>
    public void PauseResume(bool isPause)
    {
        _isPause = isPause;
        foreach (var ipause in _specialMovingPauseList)
        {
            if(_isPause)
            {
                ipause.Pause();
            } 
            else
            {
                ipause.Resume();
            }
        }
    }
    /// <summary>登録</summary>
    /// <param name="ipause">自身</param>
    public void Add(ISpecialMovingPause ipause)
    {
        _specialMovingPauseList.Add(ipause);
        if(_isPause)
        {
            ipause.Pause();
        }
    }

    /// <summary>解除</summary>
    /// <param name="ipause">自身</param>
    public void Resume(ISpecialMovingPause ipause)
    {
        _specialMovingPauseList.Remove(ipause);
    }
}
