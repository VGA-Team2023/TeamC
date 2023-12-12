using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInputControl : MonoBehaviour
{
    private InputManager _inputManager;
    private TutorialManager _tutorialManager;

    public TutorialManager TutorialManager => _tutorialManager;
    private void Awake()
    {
        _inputManager = GameObject.FindObjectOfType<InputManager>();
        _tutorialManager = GameObject.FindObjectOfType<TutorialManager>();

        _inputManager.SetTutorial(this);
    }
}
