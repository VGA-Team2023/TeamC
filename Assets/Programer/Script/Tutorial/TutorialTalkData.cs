using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialTalkData : ScriptableObject
{
    [Header("最初の説明")]
    [SerializeField] private List<string> _firstTalk = new List<string>();

    [Header("完了時の説明")]
    [SerializeField] private List<string> _completedTalk = new List<string>();

    public List<string> FirstTalks => _firstTalk;
    public List<string> CompletedTalks => _completedTalk;
}
