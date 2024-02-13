using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 逃げる側
/// 子オブジェクトとなるMoveクラスをもつオブジェクトを、上から順に行動させる親
/// 各子オブジェクトのboolを真にし、すべての子オブジェクトにその処理が終わったら、NPCの行動ターンが終了したことを知らせる
/// </summary>
public class RunawayController : MonoBehaviour
{
    #region 宣言

    [Header("子オブジェクトのコンポーネントを取得するか")] [SerializeField, Tooltip("子オブジェクトのコンポーネントを取得するか")]
    bool _isGetComponents = false;

    [Header("逃げる側のターンか")] [SerializeField, Tooltip("逃げる側のターンか")]
    bool _isRunawayTurn = false;

    [Tooltip("待機時間")] WaitForSeconds _wfs = default;

    [Header("準備から移動までの待機時間")] [SerializeField, Tooltip("準備から移動までの待機時間")]
    float _waitTime = 1.5f;

    [Header("すべての子オブジェクト")] [SerializeField]
    List<GameObject> _moveComponents = default;

    [Tooltip("ゲームマネジャーのインスタンス")] GameManager _gameManager = default;

    #endregion

    #region プロパティ

    /// <summary> グリッドを成すゲームオブジェクト </summary>
    public List<GameObject> MoveComponents
    {
        get => _moveComponents; /*set => _moveComponents = value;*/
    }

    #endregion

    void Start()
    {
        _wfs = new WaitForSeconds(_waitTime);
        _moveComponents = new List<GameObject>();
        _gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (_gameManager.NowProcessState == GameManager.ProcessState.ListUpdate)
        {
            _isGetComponents = true;
        }

        // リストの更新
        if (_isGetComponents)
        {
            _moveComponents.Clear();
            _moveComponents = GameObject.FindGameObjectsWithTag("Runaway").ToList();
            Debug.Log("clear");
            _isGetComponents = false;
            if (_gameManager.NowPhaseState == GameManager.PhaseState.FirstPhase)
            {
                _gameManager.ChangeNowProcessState(GameManager.ProcessState.Record);
            }
            else
            {
                _gameManager.ChangeNowProcessState(GameManager.ProcessState.SetStartPosition);
                Debug.Log("ToSetStartPosition");
            }
        }

        // 切り替わった瞬間
        // 第１フェーズ：nowP == moveR && oldP == record
        // 第２フェーズ：nowP == moveR && oldP == pick
        if (_gameManager.NowProcessState == GameManager.ProcessState.MoveRunaway
            && (_gameManager.OldProcessState == GameManager.ProcessState.Record
                || _gameManager.OldProcessState == GameManager.ProcessState.PickUp))
        {
            _isRunawayTurn = true;
            Debug.Log("_isRunawayTurn");
        }

        if (_isRunawayTurn)
        {
            StartCoroutine(PrepareAndMove());
            _isRunawayTurn = false;
        }
    }

    /// <summary>
    /// 子オブジェクトひとつずつ順に行動していることを見せるため
    /// </summary>
    /// <returns></returns>
    IEnumerator PrepareAndMove()
    {
        for (var i = 0; i < _moveComponents.Count; i++)
        {
            var move = _moveComponents[i].GetComponent<Move>();
            move.CurrentState = Move.State.Prepare;

            Debug.Log($"i : {i}    長さ : {_moveComponents.Count}");
            yield return _wfs;
        }

        // 子オブジェクト全てが行動し終えたら
        _gameManager.ChangeNowProcessState(GameManager.ProcessState.Check);
        Debug.Log($"check");
    }

    /// <summary>
    /// ボタンで真偽を切り替える
    /// ターンが経過するたびに呼ぶ
    /// 第二フェーズへ移るときも呼ぶ
    /// </summary>
    /// <param name="flag"></param>
    public void OnClick(bool flag)
    {
        _isGetComponents = flag;
    }
}