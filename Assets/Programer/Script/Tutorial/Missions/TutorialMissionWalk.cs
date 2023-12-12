using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class TutorialMissionWalk : TutorialMissionBase
{
    [Header("目的地のコライダー")]
    [SerializeField] private GameObject _targetCollider;

    [Header("目的地のマーカー")]
    [SerializeField] private GameObject _marker;

    [Header("プレイヤー")]
    [SerializeField] private GameObject _player;

    [Header("プレイヤーの変更位置")]
    [SerializeField] private Transform _playerPos;

    [Header("プレイヤーの回転")]
    [SerializeField] private Vector3 _playerRotation = new Vector3(0, 0, 0);

    [Header("カメラの回転")]
    [SerializeField] private Vector3 _cameraRotation = new Vector3(5, 0, 0);

    [Header("デフォルトカメラ")]
    [SerializeField] private CinemachineVirtualCamera _camera;

    [Header("フェードアウトのパネル")]
    [SerializeField] private GameObject _fadeOutpanel;

    private CinemachinePOV _cinemachinePOV;

    private Rigidbody _rb;

    private bool _isEnd = false;

    private bool _isMove = false;

    public override void Enter()
    {
        _cinemachinePOV = _camera.GetCinemachineComponent<CinemachinePOV>();
        _rb = _player.GetComponent<Rigidbody>();

        _targetCollider.SetActive(true);
        _marker.SetActive(true);
        GameObject.FindObjectOfType<WalkTutorialEnterBox>().Set(this);
    }

    public override bool Updata()
    {
        if (_isMove)
        {
            Vector3 dir = _playerPos.position - _player.gameObject.transform.position;
            _rb.velocity = dir.normalized * 20f;

            if(Vector3.Distance(_player.transform.position,_playerPos.position)<1f)
            {
                _isMove = false;
                _player.transform.position = _playerPos.position;
                _player.transform.eulerAngles = _playerRotation;

                _cinemachinePOV.m_HorizontalAxis.Value = _cameraRotation.x;
                _cinemachinePOV.m_VerticalAxis.Value = _cameraRotation.y;
                _rb.velocity = Vector3.zero;
            }
        }

        if (_isEnd)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Exit()
    {
        _targetCollider.SetActive(false);
        _marker.SetActive(false);
    }

    public void End()
    {
        //入力を不可にする
        _tutorialManager.SetCanInput(false);
        _fadeOutpanel.SetActive(true);
    }

    public void SetPos()
    {
        _isMove = true;
    }

    public void EndAnim()
    {
        _isEnd = true;

        _fadeOutpanel.SetActive(false);
    }

}
