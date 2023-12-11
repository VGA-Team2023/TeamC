using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialTalkData : ScriptableObject
{
    [Header("Å‰‚Ìà–¾")]
    [SerializeField] private List<string> _firstTalk = new List<string>();

    [Header("Š®—¹‚Ìà–¾")]
    [SerializeField] private List<string> _completedTalk = new List<string>();

    public List<string> FirstTalks => _firstTalk;
    public List<string> CompletedTalks => _completedTalk;
}
