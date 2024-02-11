using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
/// <summary>
/// 逃げる側
/// 子オブジェクトとなるMoveクラスをもつオブジェクトを、上から順に行動させる親
/// 各子オブジェクトのboolを真にし、すべての子オブジェクトにその処理が終わったら、NPCの行動ターンが終了したことを知らせる
/// </summary>
public class RunawayController : MonoBehaviour
{
    [Header("子オブジェクトのコンポーネントを取得するか")]
    [SerializeField, Tooltip("子オブジェクトのコンポーネントを取得するか")] bool _isGetComponents = false;
    [Header("逃げる側のターンか")]
    [SerializeField, Tooltip("逃げる側のターンか")] bool _isRunawayTurn = false;
    [Tooltip("待機時間")] WaitForSeconds _wfs = default;
    [Header("準備から移動までの待機時間")]
    [SerializeField, Tooltip("準備から移動までの待機時間")] float _waitTime = 1.5f;
    [Header("すべての子オブジェクト")]
    [SerializeField] List<GameObject> _moveComponents = default;

    #region プロパティ
    /// <summary> グリッドを成すゲームオブジェクト </summary>
    public List<GameObject> MoveComponents { get => _moveComponents; /*set => _moveComponents = value;*/ }
    #endregion


    void Start()
    {
        _wfs = new WaitForSeconds(_waitTime);
        _moveComponents = new List<GameObject>();
    }

    void Update()
    {
        if (_isGetComponents)
        {
            _moveComponents.Clear();

            _moveComponents = GameObject.FindGameObjectsWithTag("Runaway").ToList();
            Debug.Log("clear");
            _isGetComponents = false;
            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.Record);
        }

        // 切り替わった瞬間
        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.MoveRunaway
            && GameManager.Instance.OldProcessState == GameManager.ProcessState.Record)
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

    IEnumerator PrepareAndMove()
    {
        for (var i = 0; i < _moveComponents.Count; i++)
        {
            var move = _moveComponents[i].GetComponent<Move>();
            move.CurrentState = Move.State.Prepare;

            Debug.Log($"i : {i}    長さ : {_moveComponents.Count}");
            yield return _wfs;
        }
        GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.Check);
        Debug.Log($"check");
    }
}
