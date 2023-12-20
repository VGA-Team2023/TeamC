using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanelResetter : MonoBehaviour
{
    [SerializeField]
    GameObject _mainUi;
    [SerializeField]
    GameObject _settingPanel;
    [SerializeField]
    GameObject _helpUI;
    [SerializeField]
    GameObject[] _pointers;

    public void Reset()
    {
        //_mainUi.SetActive(false);
        _settingPanel.SetActive(true);
        _helpUI.SetActive(false);
        foreach(var point in _pointers)
        {
            point.SetActive(false);
        }
    }
}
