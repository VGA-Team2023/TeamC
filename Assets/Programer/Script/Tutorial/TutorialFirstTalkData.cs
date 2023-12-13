using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialFirstTalkData : ScriptableObject
{
    [Header("チュートリアル確認前までの文章")]
    [SerializeField] private List<string> _beforTalk = new List<string>();

    [Header("チュートリアル確認後:受ける場合の文章")]
    [SerializeField] private List<string> _okTalk = new List<string>();

    [Header("チュートリアル確認後:受けない場合の文章")]
    [SerializeField] private List<string> _noTalk = new List<string>();

    [Header("チュートリアル完了後の文章")]
    [SerializeField] private List<string> _completTalk = new List<string>();


    public List<string> BeforTalk => _beforTalk;
    public List<string> OkTalk => _okTalk;
    public List<string> NoTalk => _noTalk;
    public List<string> CompleteTalk => _completTalk;



}
