using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager
{
    bool _isPause = false;
    List<IPause> _ipauselist = new List<IPause>();

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

    public void Add(IPause ipause)
    {
        _ipauselist.Add(ipause);
        if (_isPause)
        {
            ipause.Pause();
        }
    }
    public void Remove(IPause ipause)
    {
        _ipauselist.Remove(ipause);
    }
}
