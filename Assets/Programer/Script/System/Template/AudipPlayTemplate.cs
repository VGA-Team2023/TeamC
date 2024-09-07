using CriWare;
using UnityEngine;

//音操作
public class AudipPlayTemplate : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //再生(3D対応)
            //AudioController.Instance.SE.Play3D(SEState.PlayerChargeIce,transform.position);
            //再生
            //AudioController.Instance.SE.Play3D(SEState.PlayerChargeIce, transform.position);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //一時停止
            //AudioController.Instance.SE.Pause(SEState.PlayerChargeIce);
            //停止
            //AudioController.Instance.SE.Stop(SEState.PlayerChargeIce);
        }
        else if(Input.GetMouseButtonDown(2))
        {
            //一時停止したものを再生
            //AudioController.Instance.SE.Resume(SEState.PlayerChargeIce);
        }
    }
}
