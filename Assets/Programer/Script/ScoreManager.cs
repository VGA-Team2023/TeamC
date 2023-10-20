using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>主にスコアの計算をするClass</summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>クリア時間をもとにスコア値を求めるメソッド</summary>
    /// <param name="time">ゲームクリア時間</param>
    /// /// <param name="count">撃破数</param>
    /// <returns></returns>
    public int ScoreCaster(float time, int count)
    {
        int resultScore = count - (count *  (int)(time / 1000)); 
        return resultScore;
    }
}
