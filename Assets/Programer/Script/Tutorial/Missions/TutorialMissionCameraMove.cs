using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class TutorialMissionCameraMove : TutorialMissionBase
{
    [Header("ƒJƒƒ‰ˆÚ“®‚ğ‚·ŠÔ")]
    [SerializeField] private float _checkCameraMoveTime = 5;

    private float _count = 0;

    private bool _isMoveCamera = false;

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override bool Updata()
    {
        if (!_isMoveCamera)
        {
            var h = Input.GetAxisRaw("CameraX");
            var v = Input.GetAxisRaw("CameraY");

            if (h > 0.2f || h < -0.2)
            {            //“ü—Í‚ğ•s‰Â‚É‚·‚é
                _tutorialManager.SetCanInput(false);
                _isMoveCamera = true;
            }
            else if (v > 0.2f || v < -0.2)
            {            //“ü—Í‚ğ•s‰Â‚É‚·‚é
                _tutorialManager.SetCanInput(false);
                _isMoveCamera = true;
            }
        }

        if (_isMoveCamera)
        {
            _count += Time.deltaTime;

            if (_count > _checkCameraMoveTime)
            {
                return true;
            }
        }

        return false;
    }
}
