using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>普段の一時停止を管理するクラス</summary>
public class PauseManager
{
    bool _isPause = false;
    List<IPause> _ipauselist = new List<IPause>();
    /// <summary>一時停止かどうか読み取り専用</summary>
    public bool IsPause => _isPause;
    /// <summary>一時停止と通常の切り替え処理を行う</summary>
    /// <param name="pause">停止するかどうか</param>
    public void PauseResume(bool pause)
    {
        _isPause = pause;
        foreach(IPause ipause in _ipauselist)
        {
            if (_isPause)
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
    /// <param name="ipause">自分</param>
    public void Add(IPause ipause)
    {
        _ipauselist.Add(ipause);
        if (_isPause)
        {
            ipause.Pause();
        }
    }
    /// <summary>解除</summary>
    /// <param name="ipause">自分</param>
    public void Remove(IPause ipause)
    {
        _ipauselist.Remove(ipause);
    }
}
