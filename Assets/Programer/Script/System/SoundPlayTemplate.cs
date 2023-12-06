using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayTemplate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        ////Enemyに関して
        //if (Input.GetMouseButtonDown(0))
        //    //Player通常攻撃があたった時
        //    AudioManager.Instance.EnemyHitSEPlay(this.gameObject,EnemyHitSEState.Hit);
        //if (Input.GetMouseButtonDown(1))
        //    //コア露出時のPlayerの必殺攻撃があたった時
        //    AudioManager.Instance.EnemyHitSEPlay(this.gameObject, EnemyHitSEState.SpecialHit);

        ////Playerに関して
        ///*１つのメソッドで属性の氷と草どちらにも対応しています*/
        //if (Input.GetMouseButtonDown(0))
        //    //攻撃時の弾発射音
        //    AudioManager.Instance.PlayerSEPlay(PlayerAttackSEState.Shoot);
        //if (Input.GetMouseButtonDown(2))
        //    //トレイル音
        //    AudioManager.Instance.PlayerSEPlay(PlayerAttackSEState.Trail);
        //if (Input.GetMouseButtonDown(1))
        //    //必殺攻撃時のチャージ音
        //    AudioManager.Instance.PlayerSEPlay(PlayerAttackSEState.Charge);
        //else if (Input.GetMouseButtonUp(1))
        //    //音停止
        //    AudioManager.Instance.PlayerSEStop();

    }
}
