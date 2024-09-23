using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InGameMovie : MonoBehaviour, IPause
{
    [Header("ムービーを再生するかどうか")]
    public bool IsMoviePlay = false;

    [Header("ムービー")]
    [SerializeField] private PlayableDirector _movie;

    [Header("ムービー描画")]
    [SerializeField] private LayerMask _layer;

    [Header("ムービー描画All")]
    [SerializeField] private LayerMask _layerAll;

    [SerializeField] private PlayerControl _playerControl;

    [SerializeField] private GameObject _startUI;

    [SerializeField] private List<Animator> _anims = new List<Animator>();

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
        _playerControl.IsBossMovie = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
    }

    private void Start()
    {
        if (!IsMoviePlay)
        {
            //Playerを動けるように
            _playerControl.IsBossMovie = false;

            //ゲーム開始　
            GameManager.Instance.InGameStart();

            _startUI.SetActive(true);
        }
        else
        {
            //ムービーを再生
            _movie.Play();
            //カメラの描画設定
            Camera.main.cullingMask = _layer;
            //アニメーション再生
            _playerControl.PlayerAnimControl.GameStartMovie();
        }
    }


    public void MovieEnd()
    {
        //カメラの描画を元に
        Camera.main.cullingMask = _layerAll;
        _playerControl.IsBossMovie = false;
        _startUI.SetActive(true);

        //ゲーム開始　
        GameManager.Instance.InGameStart();
    }

    public void Pause()
    {
        _movie.Pause();
        _anims.ForEach(i => i.speed = 0);
    }

    public void Resume()
    {
        _movie.Resume();
        _anims.ForEach(i => i.speed = 1);
    }


}
