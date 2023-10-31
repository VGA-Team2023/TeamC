using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlle : MonoBehaviour
{
    [SerializeField,Tooltip("次のシーンの状態")] GameState _nextGameState;
    /// <summary>シーン遷移メソッド</summary>
    /// <param name="nextSecene">遷移先のシーン名</param>
    public void SceneChange(string nextSecene)
    {
        SceneManager.LoadScene(nextSecene);
        GameManager.Instance.ChangeGameState(_nextGameState);
    }
}
