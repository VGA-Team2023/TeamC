using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class FinishingAttackUI
{
    [Header("とどめ可能なことを示すUIのプレハブ")]
    [SerializeField] private GameObject _canFinishUIPrefab;

    [Header("とどめをさしている間のUIのプレハブ")]
    [SerializeField] private GameObject _finishUIPrefab;

    [Header("敵の最大数")]
    [SerializeField] private int _enemyMaxNum = 10;


    [Header("トドメを促すUI")]
    [SerializeField] private GameObject _finishingUI;

    [Header("トドメのUI")]
    [SerializeField] private GameObject _dofinishingUI;

    [Header("トドメのパーセンテージを表示")]
    [SerializeField] private Image _finishingSliderUI;

    // オブジェクト位置のオフセット
    [SerializeField] private Vector3 _worldOffset;

    [SerializeField] private Canvas _canvas;

    private float _finishTime = 3;

    private RectTransform _parentUI;

    private List<GameObject> _canFinishUI = new List<GameObject>();

    private List<GameObject> _finishUI = new List<GameObject>();

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl,float finishTime)
    {
        _playerControl = playerControl;
        _parentUI = _canvas.GetComponent<RectTransform>();
        _finishTime = finishTime;

        for (int i = 0; i < _enemyMaxNum; i++)
        {
            _canFinishUI.Add(UnityEngine.GameObject.Instantiate(_canFinishUIPrefab));
            _finishUI.Add(UnityEngine.GameObject.Instantiate(_finishUIPrefab));

            _canFinishUI[i].transform.SetParent(_parentUI);
            _finishUI[i].transform.SetParent(_parentUI);
        }


    }

    public void ShowCanFinishingUI(bool isON)
    {
        _finishingUI.SetActive(isON);
    }

    public void SetFinishUI(float max,int _enemyNum)
    {
        for (int i = 0; i < _enemyNum; i++)
        {
            _finishUI[i].SetActive(true);
        }

        foreach(var a in _canFinishUI)
        {
            a.SetActive(false);
  
        }

        _dofinishingUI.SetActive(true);

        _finishingSliderUI.fillAmount = 0;
    }

    public void UnSetFinishUI()
    {
        for (int i = 0; i < _enemyMaxNum; i++)
        {
            _finishUI[i].SetActive(false);
        }
        _dofinishingUI.SetActive(false);
    }


    public void ChangeValue(float time)
    {
        _finishingSliderUI.fillAmount += time/_finishTime;
    }



    public void ShowUI(Transform[] pos)
    {
        for (int i = 0; i < _enemyMaxNum; i++)
        {
            _canFinishUI[i].SetActive(false);
            _finishUI[i].SetActive(false);
        }

        for (int i = 0; i < pos.Length; i++)
        {
            UpdateCanFinishingUIPosition(pos[i], i);
        }
    }

    // UIの位置を更新する
    public void UpdateFinishingUIPosition(Transform t, int num)
    {
        var cameraTransform = Camera.main.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;

        // オブジェクトの位置
        var targetWorldPos = t.position ;

        // カメラからターゲットへのベクトル
        var targetDir = targetWorldPos - cameraTransform.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラ前方ならUI表示、後方なら非表示
        _finishUI[num].gameObject.SetActive(isFront);
        if (!isFront) return;

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransformのローカル座標を更新
        _finishUI[num].transform.localPosition = uiLocalPos;
    }

    // UIの位置を更新する
    private void UpdateCanFinishingUIPosition(Transform t, int num)
    {
        _canFinishUI[num].SetActive(true);

        var cameraTransform = Camera.main.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;

        // オブジェクトの位置
        var targetWorldPos = t.position + _worldOffset;

        // カメラからターゲットへのベクトル
        var targetDir = targetWorldPos - cameraTransform.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラ前方ならUI表示、後方なら非表示
        _canFinishUI[num].gameObject.SetActive(isFront);
        if (!isFront) return;

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransformのローカル座標を更新
        _canFinishUI[num].transform.localPosition = uiLocalPos;
    }

}
