using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("チュートリアルの順番設定")]
    [SerializeField] private List<TutorialNum> _tutorialOrder = new List<TutorialNum>();

    [Header("チュートリアルの詳細設定")]
    [SerializeField] private TutorialMissions _tutorialMissions;

    [Header("チュートリアルの説明UI")]
    [SerializeField] private TutorialUI _tutorialUI;

    [Header("チュートリアルの文章")]
    [SerializeField] private TutorialFirstTalkData _tutorialFirstTalkData;

    [SerializeField] private Loading _loading;

    protected InputManager _inputManager;

    private int _tutorialCount = 0;

    /// <summary>チュートリアルを受けえるかどうか</summary>
    private bool _isTutorilReceve = false;

    private bool _isEndTutorial = false;

    private bool _isFirstRead = false;

    private bool _isCanInput = false;

    private bool _isFirstVoice = false;

    public bool IsCanInput => _isCanInput;

    private bool _isReadEndMissinFirst = false;

    private bool _isEndFirstLead = false;
    private bool _isCheckPanel = false;

    private float _countWait = 0;

    private TutorialSituation _tutorialSituation = TutorialSituation.GameStartTalk;

    public TutorialMissions TutorialMissions => _tutorialMissions;

    public enum TutorialSituation
    {
        /// <summary>ゲーム開始時の会話</summary>
        GameStartTalk,

        /// <summary>チュートリアルを受けるかどうかの判断をした後の会話</summary>
        TutorialReceve,

        /// <summary>ミッション内容を説明中 </summary>
        FirstTalk,

        /// <summary>ミッション内容を実行</summary>
        TryMove,

        /// <summary>ミッション完了の説明 </summary>
        CompleteTalk,

        /// <summary>チュートリアルを終了</summary>
        TutorialEnd,

    }

    private void Awake()
    {
        _inputManager = GameObject.FindObjectOfType<InputManager>();
        _tutorialMissions.Init(this, _inputManager);

        AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialStart);

        //チュートリアル開始前の会話を設定
        _tutorialUI.SetTalk(_tutorialFirstTalkData.BeforTalk);
    }

    void Update()
    {
        // Debug.Log("F" + _tutorialMissions.CurrentTutorial.TutorialNum);

        if (_tutorialSituation == TutorialSituation.GameStartTalk)
        {
            if (_isEndFirstLead && !_isFirstVoice)
            {
                _tutorialUI.ActiveBttun();
                _isFirstVoice = true;
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialCheck);
            }

            if (_isEndFirstLead && !_isCheckPanel)
            {
                _countWait += Time.deltaTime;

                if (_countWait > 0.01f)
                {
                    _tutorialUI.ShowTutorilCheck(true);
                    _isCheckPanel = true;
                }
            }

            //チュートリアルを受けるかどうかの確認パネルを表示
            if (_isFirstRead && !_isEndFirstLead)
            {
                _isEndFirstLead = true;
            }
            //文章を読み終えたかどうか
            _isFirstRead = _tutorialUI.Read();
        }
        else if (_tutorialSituation == TutorialSituation.TutorialReceve)
        {
            //文章を読み終えたかど うか
            bool isReadEnd = _tutorialUI.Read();

            if (isReadEnd)
            {

                _isReadEndMissinFirst = true;
                if (_isTutorilReceve)
                {
                    SetFirstTutorial();
                }   //チュートリアルを受ける場合はチュートリアルをセット
                else
                {
                    //会話のパネルを非表示
                    _tutorialUI.TalkPanelSetActive(false);
                    _loading.LoadingScene();
                }   //チュートリアルを受けない場合はSceneを推移
            }
        }
        else if (_tutorialSituation == TutorialSituation.FirstTalk)
        {
            //文章を読み終えたかどうか
            bool isReadEnd = _tutorialUI.Read();

            if (_isReadEndMissinFirst)
            {
                _isReadEndMissinFirst = false;
                SetTryMove();
            }   //読み終えたら、実行状況に以降

            if (isReadEnd)
            {
                _isReadEndMissinFirst = true;
            }
        }
        else if (_tutorialSituation == TutorialSituation.TryMove)
        {
            if (_tutorialMissions.CurrentTutorial.Updata())
            {
                _tutorialMissions.CurrentTutorial.Exit();
                SetEndTalk();
            }
        }
        else if (_tutorialSituation == TutorialSituation.CompleteTalk)
        {
            //文章を読み終えたかどうか
            bool isReadEnd = _tutorialUI.Read();

            if (isReadEnd)
            {
                if (_isEndTutorial)
                {
                    //チュートリアル開始前の会話を設定
                    _tutorialUI.SetTalk(_tutorialFirstTalkData.CompleteTalk);
                    _tutorialSituation = TutorialSituation.TutorialEnd;
                }
                else
                {
                    SetNextTutorial();
                }

            }   //読み終えたら、実行状況に以降
        }
        else
        {
            //文章を読み終えたかどうか
            bool isReadEnd = _tutorialUI.Read();

            if (isReadEnd)
            {
                //会話のパネルを非表示
                _tutorialUI.TalkPanelSetActive(false);
                _loading.LoadingScene();
            }   //読み終えたら、実行状況に以降
        }

    }

    public void SetCanInput(bool canInput)
    {
        _isCanInput = canInput;
    }

    /// <summary>チュートリアルを受ける、ボタンを押したときに呼ぶ </summary>
    public void PlayTutorial()
    {
        _tutorialSituation = TutorialSituation.TutorialReceve;

        AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialOK);

        //チュートリアルを受ける場合の会話を設定
        _tutorialUI.SetTalk(_tutorialFirstTalkData.OkTalk);
        _isTutorilReceve = true;

        //チュートリアルを受けるかどうかの確認パネルを非表示
        _tutorialUI.ShowTutorilCheck(false);
    }

    /// <summary>チュートリアルを受けない、ボタンを押した時に呼ぶ </summary>
    public void UnPlayTutorial()
    {
        _tutorialSituation = TutorialSituation.TutorialReceve;

        //チュートリアルを受ける場合の会話を設定
        _tutorialUI.SetTalk(_tutorialFirstTalkData.NoTalk);
        _isTutorilReceve = false;

        //チュートリアルを受けるかどうかの確認パネルを非表示
        _tutorialUI.ShowTutorilCheck(false);
    }


    void SetFirstTutorial()
    {
        foreach (var t in _tutorialMissions.Tutorials)
        {
            if (t.TutorialNum == _tutorialOrder[_tutorialCount])
            {
                _tutorialCount++;
                _tutorialMissions.CurrentTutorial = t;
                SetFirstTalk();
                if (_tutorialCount == _tutorialOrder.Count)
                {
                    _isEndTutorial = true;
                }
                Voice(true);
                return;
            }
        }
        Debug.LogError("チュートリアルがありません");
    }

    /// <summary>ミッションを設定する</summary>
    void SetNextTutorial()
    {
        foreach (var t in _tutorialMissions.Tutorials)
        {
            if (t.TutorialNum == _tutorialOrder[_tutorialCount])
            {
                _tutorialCount++;
                _tutorialMissions.CurrentTutorial = t;
                SetFirstTalk();

                if (_tutorialCount == _tutorialOrder.Count)
                {
                    _isEndTutorial = true;
                }
                Voice(true);
                return;
            }
        }
        Debug.LogError("チュートリアルがありません");
    }

    /// <summary>チュートリアルの説明を設定 </summary>
    public void SetFirstTalk()
    {
        //最初の説明を受ける、状態
        _tutorialSituation = TutorialSituation.FirstTalk;

        _tutorialMissions.CurrentTutorial.InfoUIActive(true);

        //説明の文章を設定
        _tutorialUI.SetTalk(_tutorialMissions.CurrentTutorial.TalkData.FirstTalks);
    }

    public void SetTryMove()
    {
        //入力を不可にする
        SetCanInput(true);

        //実行する、状態
        _tutorialSituation = TutorialSituation.TryMove;

        //ミッションの初期設定をする
        _tutorialMissions.CurrentTutorial.Enter();

        //会話のパネルを非表示
        _tutorialUI.TalkPanelSetActive(false);
    }

    public void SetEndTalk()
    {
        Voice(false);

        //完了後の説明の、状態
        _tutorialSituation = TutorialSituation.CompleteTalk;

        //説明の文章を設定
        _tutorialUI.SetTalk(_tutorialMissions.CurrentTutorial.TalkData.CompletedTalks);

        _tutorialMissions.CurrentTutorial.InfoUIActive(false);
    }


    public void Voice(bool isFirstTalk)
    {
        TutorialNum num = _tutorialOrder[_tutorialCount - 1];

        if (isFirstTalk)
        {
            if (num == TutorialNum.Walk)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialMove);
            }
            else if (num == TutorialNum.Look)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialCamera);
            }
            else if (num == TutorialNum.Attack)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialAttack);
            }
            else if (num == TutorialNum.ChangeAttribute)
            {
                //   AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.FinishAttack)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialFinish);
            }
            else if (num == TutorialNum.LockOn)
            {
                //  AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.LockOnChangeEnemy)
            {
                //   AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.Avoid)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialDodge);
            }
            else if (num == TutorialNum.Opption)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialOption);
            }
        }
        else
        {
            if (num == TutorialNum.Walk)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialMoveOK);
            }
            else if (num == TutorialNum.Look)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialCameraOK);
            }
            else if (num == TutorialNum.Attack)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialAttackOK);
            }
            else if (num == TutorialNum.ChangeAttribute)
            {
                //  AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.FinishAttack)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialFinishOK);
            }
            else if (num == TutorialNum.LockOn)
            {
                // AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.LockOnChangeEnemy)
            {
                // AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.Avoid)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialDodgeOK);
            }
            else if (num == TutorialNum.Opption)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialOptionOK);
            }
        }
    }


}

public enum TutorialNum
{
    /// <summary>移動のチュートリアル </summary>
    Walk,

    /// <summary>カメラの操作 </summary>
    Look,

    /// <summary>攻撃</summary>
    Attack,

    /// <summary>トドメ </summary>
    FinishAttack,

    /// <summary>回避 </summary>
    Avoid,

    /// <summary>属性変更 </summary>
    ChangeAttribute,

    /// <summary>ロックオン </summary>
    LockOn,

    /// <summary>ロックオンをして、ロックオンの敵を変える</summary>
    LockOnChangeEnemy,

    /// <summary>オプションを開く</summary>
    Opption,
}
