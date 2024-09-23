using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleProlougue : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private List<GameObject> _textData;

    [Header("最初のフェードアウト")]
    [SerializeField] private GameObject _firstFadeOut;

    [Header("プロローグイラスト")]
    [SerializeField] private GameObject _ilast;

    [SerializeField] private GameObject _canvus;

    [SerializeField] private Loading _loading;

    private int _setIndex = 0;

    private bool _isStart = false;

    private bool _isEndFadeOut = false;

    void Start()
    {

    }


    void Update()
    {
        if (_isEndFadeOut)
        {
            TextRead();
        }
    }


    /// <summary>スタートボタンを押す。フェードアウトを始める</summary>
    public void StartButtun()
    {
        if (_isStart) { return; }
        _isStart = true;
        _firstFadeOut.SetActive(true);
    }

    public void EndFadeOut()
    {
        _isEndFadeOut = true;
        // _loading = GameObject.FindAnyObjectByType<Loading>();
        _firstFadeOut.SetActive(false);
    }

    public void ProlougueShow()
    {
        _ilast.SetActive(true);
        _textData[0].SetActive(true);
    }





    public void TextRead()
    {
        if (Input.GetButtonDown("Avoid") || Input.GetKeyDown(KeyCode.Space))
        {
            _textData[_setIndex].gameObject.SetActive(false);
            _setIndex++;

            if (_setIndex == _textData.Count)
            {
                _isEndFadeOut = false;
                _loading.LoadingScene();
                _canvus.SetActive(false);
                return;
            }

            _textData[_setIndex].gameObject.SetActive(true);
        }
    }

}


