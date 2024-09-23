using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleOptionActiveFalse : MonoBehaviour
{
    [SerializeField] private GameObject _opitionPanel;

    [SerializeField] private OptionPanelResetter _reset;

    void Update()
    {
        if (_opitionPanel.activeSelf)
        {
            if (Input.GetButtonDown("Pause"))
            {
                _opitionPanel.SetActive(false);
                _reset.Reset();
            }
        }

    }
}
