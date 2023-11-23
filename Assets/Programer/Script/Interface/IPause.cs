using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>普段の一時停止処理を行うインターフェイス</summary>
public interface IPause
{
    /// <summary>一時停止時に実行</summary>
    public void Pause();
    /// <summary>一時停止から通常に切り替わる時に実行</summary>
    public void Resume();
}
