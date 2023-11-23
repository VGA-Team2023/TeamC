using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlle : MonoBehaviour
{
    [SerializeField, Tooltip("次のシーン名")] string _nextSceneName;
    /// <summary>シーン遷移メソッド</summary>
    /// <param name="nextSecene">遷移先のシーン名</param>
    public void SceneChange()
    {
        SceneManager.LoadScene(_nextSceneName);
    }
}
