using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimEvents : MonoBehaviour,IPause
{
    [Header("アニメーター")]
    [SerializeField] private List<Animator> _anims = new List<Animator>();
    public void Pause()
    {
        _anims.ForEach(i => i.speed = 0);
    }

    public void Resume()
    {
        _anims.ForEach(i => i.speed = 1);
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
    }

}
