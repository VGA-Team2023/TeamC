using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>弾から音が流れているようにしたもの
/// (音がloopであり、かつ音源であるオブジェクトが動くものに使う(例:オブジェクトがヘリコプターでプロペラの音を再生させたい))</summary>
[RequireComponent(typeof(Rigidbody))]
public class BulletAudioTempale : MonoBehaviour
{
    Rigidbody _rb;
    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        AudioController.Instance.SE.Play3D(SEState.PlayerTrailIcePatternA, this.transform.position);
        Destroy(this.gameObject,5);
    }

    public void OnDestroy()
    {
        //オブジェクトが消えても音は再生されているままなので、Stopで停止
        AudioController.Instance.SE.Stop(SEState.PlayerTrailIcePatternA);
    }
    public void Update()
    {
        //音の位置を毎フレーム更新
        AudioController.Instance.SE.Update3DPos(SEState.PlayerTrailIcePatternA, this.transform.position);
    }

    public void FixedUpdate()
    {
        _rb.velocity = new Vector3(0, 0, 5);
    }
}
