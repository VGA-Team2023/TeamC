using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////
/// ヒットストップと一時停止の実装したもの(テンプレート)
/// 下記にヒットストップや一時停止に切り替えたいときの呼び出し方記載
/// ヒットストップと一時停止の実装したもの(テンプレート)
/////////////////////////////////////////////////////////////////////////
public class HitStopPauseObjectTemplate : MonoBehaviour, ISlow,IPause,ISpecialMovingPause
{
    Rigidbody _rb;
    Animator _anim;
    GameManager _gaManager;
    /// <summary>現在の移動速度</summary>
    [SerializeField,Header("確認用")] float _currentSpeed;
    /// <summary>歩行速度</summary>
    [SerializeField] protected float _walkSpeed;
    private void OnEnable()
    { 
        //実行終了後に出るGameManagerの参照先がなくなるというエラー回避のためGameManager.Instanceを変数に入れておく
        _gaManager = GameManager.Instance;
        _gaManager.SlowManager.Add(this);　//ヒットストップの登録
        _gaManager.PauseManager.Add(this);  //一時停止の登録
        _gaManager.SpecialMovingPauseManager.Add(this); //必殺技時の一時停止の登録
    }
    private void OnDisable()
    {
        _gaManager.SlowManager.Remove(this);　//ヒットストップの解除
        _gaManager.PauseManager.Remove(this);  //一時停止の解除
        _gaManager.SpecialMovingPauseManager.Resume(this); //必殺技時の一時停止の解除
    }
    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _currentSpeed = _walkSpeed;
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector3(0, 0, _currentSpeed);  //前方にまっすぐ移動
    }

    /////////////////////////////////ヒットストップ///////////////////////////////////////
    void ISlow.OnSlow(float slowSpeedRate)
    {
        //ヒットストップ時の処理を書く
        _anim.speed = slowSpeedRate;　//アニメーションの再生速度は0～1までなのでそのまま代入
        _currentSpeed = _walkSpeed * slowSpeedRate;  //割合値をかける
    }

    void ISlow.OffSlow()
    {
        //通常時時の処理を書く
        _anim.speed = 1;
        _currentSpeed = _walkSpeed;
    }

    //////////////////////////////////一時停止///////////////////////////////////////
    void IPause.Pause()
    {
        //一時停止時の処理を書く
        _anim.speed = 0;
        //Rigidbodyの停止のさせ方は各々で決めてもらう？
        _rb.Sleep();
        _rb.isKinematic = true;
    }

    void IPause.Resume()
    {
        //通常時の処理を書く
        _anim.speed = 1;
        _rb.isKinematic = false;
        _rb.WakeUp();
    }
    //////////////////////////////////必殺技時の一時停止///////////////////////////////////////
    void ISpecialMovingPause.Pause()
    {
        //一時停止時の処理を書く
        _anim.speed = 0;
        //Rigidbodyの停止のさせ方は各々で決めてもらう？
        _rb.Sleep();
        _rb.isKinematic = true;
    }

    void ISpecialMovingPause.Resume()
    {
        //通常時の処理を書く
        _anim.speed = 1;
        _rb.isKinematic = false;
        _rb.WakeUp();
    }

    ///////////////////////////////呼び出し方////////////////////////////////////////
    //ヒットストップ
    //GameManager.Instance.SlowManager.OnOffSlow(true); でスローに切り替わる
    //GameManager.Instance.SlowManager.OnOffSlow(false); で通常に戻る
    //一時停止
    //GameManager.Instance.PauseManager.PauseResume(true); で停止に切り替わる
    //GameManager.Instance.PauseManager.PauseResume(false); で通常に戻る
    //必殺技時の一時停止
    //GameManager.Instance.SpecialMovingPauseManager.PauseResume(true); で停止に切り替わる
    //GameManager.Instance.SpecialMovingPauseManager.PauseResume(false); で通常に戻る
}
