using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGM
{
    [SerializeField,Tooltip("どのBGMか")] BGMState bgmState;
    [SerializeField] string soundCueName;

    public string SoundCueName => soundCueName;
}
[System.Serializable]
public class PlayerSE
{
    [SerializeField,Tooltip("PlayerのSEの状態")] PlayerAttackSEState playerSE;
    [SerializeField] AttributeSE attributeSE;
    public AttributeSE AttributeSE => attributeSE;
}

/// <summary>属性別の音に対応したもの</summary>
[System.Serializable]
public struct AttributeSE
{
    [SerializeField, Tooltip("氷属性時の音")] string iceSoundCueName;
    [SerializeField, Tooltip("草属性時の音")] string grassSoundCueName;
    public string IceSoundCueName => iceSoundCueName;
    public string GrassSoundCueName => grassSoundCueName;
}


[System.Serializable]
public class EnemyHitSE
{
    [SerializeField] EnemyHitSEState enemyHitSE;
    [SerializeField] AttributeSE attributeSE;
    public AttributeSE AttributeSE => attributeSE;
}
[System.Serializable]
public class EnemyActionSE
{
    [SerializeField] EnemyActionSEState enemyActionSE;
    [SerializeField] string soundCueName;
    public string SoundCueName => soundCueName;
}

[System.Serializable]
public class ButtonPushSEProperty
{
    [SerializeField] ButtonPushSE buttonPushSE;
    [SerializeField] string soundCueName;

    public string SoundCueName => soundCueName;
}



public enum BGMState
{
    /// <summary>タイトル</summary>
    Title,
    /// <summary>チュートリアル</summary>
    Tutorial,
    /// <summary>インゲーム</summary>
    InGame,
    /// <summary>リザルト</summary>
    Result,
}

public enum PlayerAttackSEState
{
    /// <summary>通常攻撃の弾発射時の音</summary>
    Shoot,
    /// <summary>コア露出時のチャージ中の音</summary>
    Charge,
    /// <summary>トレイル</summary>
    Trail
}

public enum ButtonPushSE
{
    /// <summary>決定時の音</summary>
    Apply,
    /// <summary>キャンセル時の音</summary>
    Cancel,
}

public enum EnemyHitSEState
{
    /// <summary>Player通常攻撃時に当たった時のHit音</summary>
    Hit,
    /// <summary>Player必殺攻撃時に当たった時のHit音</summary>
    SpecialHit,
}

public enum EnemyActionSEState
{

}
