using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>必殺技時の一時停止処理を行うインターフェイス</summary>
public interface ISpecialMovingPause 
{
    /// <summary>停止</summary>
    public void Pause();
    /// <summary>再生</summary>
    public void Resume();
}
