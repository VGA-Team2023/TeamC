using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager
{
    bool _isPause = false;
    List<IPause> _ipauselist = new List<IPause>();

    /// <summary>ˆê’â~‚Æ’Êí‚ÌØ‚è‘Ö‚¦ˆ—‚ğs‚¤</summary>
    /// <param name="pause">’â~‚·‚é‚©‚Ç‚¤‚©</param>
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
    /// <summary>“o˜^</summary>
    /// <param name="ipause">©•ª</param>
    public void Add(IPause ipause)
    {
        _ipauselist.Add(ipause);
        if (_isPause)
        {
            ipause.Pause();
        }
    }
    /// <summary>‰ğœ</summary>
    /// <param name="ipause">©•ª</param>
    public void Remove(IPause ipause)
    {
        _ipauselist.Remove(ipause);
    }
}
