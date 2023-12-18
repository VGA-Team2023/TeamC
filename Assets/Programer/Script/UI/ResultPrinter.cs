using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultPrinter : MonoBehaviour
{
    private AudioController _audioController;
    [SerializeField] private Image _passImage;
    [SerializeField] private Image _failureImage;
    [SerializeField] private Image _judgeImage;
    [SerializeField] private Text _resultText;
    [SerializeField] private Text _clearTimeMinutesResult;
    [SerializeField] private Text _clearTimeSecondResult;
    [SerializeField] private Text _rankResult;
    [SerializeField] private Text _enemyDefeatedCount;
    [SerializeField] private Text _playerDownCount;
    [SerializeField] private Text _judgeText;
    [SerializeField, Tooltip("スコア別テキスト")]
    private string[] _resultTexts;
    [SerializeField, Tooltip("表示にかける秒数")]
    private int _displayTime = 1;
    [SerializeField, Tooltip("クリアタイムの目標値")]
    private int _idealClearTimeSeconds = 150;
    [SerializeField, Tooltip("敵の撃破数の目標値")]
    private int _idealEnemyDefeatedNum = 20;
    [SerializeField, Tooltip("プレイヤーの倒れた回数の目標値")]
    private int _idealPlayerDownNum = 0;
    [SerializeField, Tooltip("Sランクスコアとして扱うクリアタイムの目標値からの範囲")]
    private int _sRankScoreClearTimeGap = 20;
    [SerializeField, Tooltip("Aランクのスコアとして扱うクリアタイムの目標値からの範囲")]
    private int _aRankScoreClearTimeGap = 60;
    [SerializeField, Tooltip("Bランクのスコアとして扱うクリアタイムの目標値からの範囲")]
    private int _bRankScoreClearTimeGap = 120;
    [SerializeField, Tooltip("Cランクのスコアとして扱うクリアタイムの目標値からの範囲")]
    private int _cRankScoreClearTimeGap = 150;
    [SerializeField, Tooltip("Sランクのスコアとして扱う撃破数の目標値からの範囲")]
    private int _sRankScoreEnemyDefeatedGap = 0;
    [SerializeField, Tooltip("Aランクのスコアとして扱う撃破数の目標値からの範囲")]
    private int _aRankScoreEnemyDefeatedGap = 0;
    [SerializeField, Tooltip("Bランクのスコアとして扱う撃破数の目標値からの範囲")]
    private int _bRankScoreEnemyDefeatedGap = 0;
    [SerializeField, Tooltip("Cランクのスコアとして扱う撃破数の目標値からの範囲")]
    private int _cRankScoreEnemyDefeatedGap = 1;
    [SerializeField, Tooltip("Sランクのスコアとして扱うダウン回数の目標値からの範囲")]
    private int _sRankScorePlayerDownGap = 1;
    [SerializeField, Tooltip("Aランクのスコアとして扱うダウン回数の目標値からの範囲")]
    private int _aRankScorePlayerDownGap = 2;
    [SerializeField, Tooltip("Bランクのスコアとして扱うダウン回数の目標値からの範囲")]
    private int _bRankScorePlayerDownGap = 3;
    [SerializeField, Tooltip("Cランクのスコアとして扱うダウン回数の目標値からの範囲")]
    private int _cRankScorePlayerDownGap = 4;
    [SerializeField, Tooltip("Sランク基準値")]
    private int _sRankScore = 11;
    [SerializeField, Tooltip("Aランク基準値")]
    private int _aRankScore = 7;
    [SerializeField, Tooltip("Bランク基準値")]
    private int _bRankScore = 6;
    [SerializeField, Tooltip("Cランク基準値")]
    private int _cRankScore = 4;
    [SerializeField, Tooltip("改行が行われる文字数")]
    private int _lineLength = 25;
    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        _audioController = AudioController.Instance;
        int defeatcount = gameManager.ScoreManager.LongEnemyDefeatedNum +
            gameManager.ScoreManager.ShortEnemyDefeatedNum;
        TweenNum(GameManager.Instance.ScoreManager.ClearTime.Minutes, _clearTimeMinutesResult, () =>
        {
            TweenNum(GameManager.Instance.ScoreManager.ClearTime.Seconds, _clearTimeSecondResult, () =>
            {
                TweenNum(defeatcount, _enemyDefeatedCount, () =>
                {
                    TweenNum(GameManager.Instance.ScoreManager.PlayerDownNum, _playerDownCount, () =>
                    {
                        Judge(GameManager.Instance);
                        GameManager.Instance.ScoreManager.ScoreReset();
                    });
                });
            });
        });
    }
    /// <summary>
    /// 指定秒数でスコアを表示する関数
    /// </summary>
    /// <param name="targetNum">表示する値</param>
    /// <param name="tweenText">表示用Text</param>
    /// <param name="onCompleteCallback">Tweenが終わったかどうかのコールバック</param>
    public void TweenNum(int targetNum, Text tweenText, TweenCallback onCompleteCallback = null)
    {
        //_audioController.SE.Play(SE)
        int startnum = 99;
        DOTween.To(() => startnum, (n) => startnum = n, targetNum, _displayTime)
            .OnUpdate(() => tweenText.text = startnum.ToString("#,0"))
            .OnComplete(onCompleteCallback);

    }
    private void TweenResultText(string displayText)
    {
        int index = 0;

        DOTween.To(() => index, (n) => index = n, displayText.Length, _displayTime)
            .OnUpdate(() =>
            {
                string currentText = displayText.Substring(0, index);
                for (int i = _lineLength; i < currentText.Length; i += _lineLength + 1)
                {
                    currentText = currentText.Insert(i, "\n");
                }

                _resultText.text = currentText;
            });
    }

    /// <summary>
    /// 合否判定用メソッド
    /// </summary>
    /// <param name="GM">GameManagerのInstance</param>
    private void Judge(GameManager GM)
    {
        int evaluationvalue = 0;
        int cleartimesecond = (GM.ScoreManager.ClearTime.Minutes) * 60 +
            GM.ScoreManager.ClearTime.Seconds;
        int enemyDefeatedNum = (GM.ScoreManager.LongEnemyDefeatedNum)+ (GM.ScoreManager.ShortEnemyDefeatedNum);
        int playerDownCount = GM.ScoreManager.PlayerDownNum;

        if (cleartimesecond - _idealClearTimeSeconds <= _sRankScoreClearTimeGap)
        {
            evaluationvalue += 4;
        }
        else if (cleartimesecond - _idealClearTimeSeconds <= _aRankScoreClearTimeGap)
        {
            evaluationvalue += 3;
        }
        else if (cleartimesecond - _idealClearTimeSeconds <= _bRankScoreClearTimeGap)
        {
            evaluationvalue += 2;
        }
        else if (cleartimesecond - _idealClearTimeSeconds <= _cRankScoreClearTimeGap)
        {
            evaluationvalue += 1;
        }

        if (_idealEnemyDefeatedNum - enemyDefeatedNum <= _sRankScoreEnemyDefeatedGap)
        {
            evaluationvalue += 4;
        }
        else if (_idealEnemyDefeatedNum - enemyDefeatedNum <= _aRankScoreEnemyDefeatedGap)
        {
            evaluationvalue += 3;
        }
        else if (_idealEnemyDefeatedNum - enemyDefeatedNum <= _bRankScoreEnemyDefeatedGap)
        {
            evaluationvalue += 2;
        }
        else if (_idealEnemyDefeatedNum - enemyDefeatedNum <= _cRankScoreEnemyDefeatedGap)
        {
            evaluationvalue += 1;
        }

        if (playerDownCount <= _sRankScorePlayerDownGap)
        {
            evaluationvalue += 4;
        }
        else if (playerDownCount <= _aRankScorePlayerDownGap)
        {
            evaluationvalue += 3;
        }
        else if (playerDownCount <= _bRankScorePlayerDownGap)
        {
            evaluationvalue += 2;
        }
        else if (playerDownCount <= _cRankScorePlayerDownGap)
        {
            evaluationvalue += 1;
        }

        _judgeText.gameObject.SetActive(true);

        if (evaluationvalue >= _sRankScore)
        {
            TweenResultText(_resultTexts[0]);
            _judgeText.text = "S";
            _passImage.gameObject.SetActive(true);
        }
        else if (evaluationvalue >= _aRankScore)
        {
            TweenResultText(_resultTexts[1]);
            _judgeText.text = "A";
            _passImage.gameObject.SetActive(true);
        }
        else if (evaluationvalue >= _bRankScore)
        {
            TweenResultText(_resultTexts[2]);
            _judgeText.text = "B";
            _passImage.gameObject.SetActive(true);
        }
        else if (evaluationvalue >= _cRankScore)
        {
            TweenResultText(_resultTexts[2]);
            _judgeText.text = "C";
            _failureImage.gameObject.SetActive(true);
        }
        else
        {
            TweenResultText(_resultTexts[2]);
            _judgeText.text = "D";
            _failureImage.gameObject.SetActive(true);
        }
    }
}
