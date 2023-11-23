using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>スロー処理を行うインターフェイス</summary>
public interface ISlow
{
    /// <summary>通常からスローに切り替わる時に実行する</summary>
    /// <param name="slowSpeedRate">スロー時の速度の割合</param>
    public void OnSlow(float slowSpeedRate);
    /// <summary>スローから通常に切り替わる時に実行する</summary>
    public void OffSlow();
}
