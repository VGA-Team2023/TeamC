using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class TutorialUI
{
    [Header("会話のパネル")]
    [SerializeField] private GameObject _talkPane;

    [Header("会話の文章を映すText")]
    [SerializeField] private Text _text;

    [Header("チュートリアルを受けるかどうかを確認するパネル")]
    [SerializeField] private GameObject _checkTutorialPanel;

    [SerializeField] GameObject _buttun;

    /// <summary>現在の文章</summary>
    private List<string> _talk = new List<string>();

    private int _count = 0;

    private bool _isReadEnd = false;

    public void SetTalk(List<string> talk)
    {
        Debug.Log("セット:" + talk[0]);
        _talk = talk;
        _count = 0;
        _isReadEnd = false;

        _talkPane.SetActive(true);
        _text.text = _talk[_count];
    }

    public bool Read()
    {
        if (_isReadEnd) return false;

        if (Input.GetButtonDown("ReadText"))
        {
            _count++;

            if (_count >= _talk.Count)
            {
                _text.text = "";
                _isReadEnd = true;
                return true;
            }
            _text.text = _talk[_count];
            return false;
        }
        return false;
    }

    public void TalkPanelSetActive(bool isActive)
    {
        _talkPane.SetActive(isActive);
    }

    public void ShowTutorilCheck(bool isActive)
    {
        EventSystem.current.SetSelectedGameObject(null);
        _checkTutorialPanel.SetActive(isActive);
    }


    public void ActiveBttun()
    {
        //EventSystem.current.SetSelectedGameObject(_buttun);
    }
}
