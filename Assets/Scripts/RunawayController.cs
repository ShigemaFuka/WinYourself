using System.Collections;
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
    //[SerializeField, Tooltip("子オブジェクトの数")] int _childrenAmount = default;
    [Header("逃げる側のターンか")]
    [SerializeField, Tooltip("逃げる側のターンか")] bool _isRunawayTurn = false;
    [Tooltip("待機時間")] WaitForSeconds _wfs = default;
    [Header("準備から移動までの待機時間")]
    [SerializeField, Tooltip("準備から移動までの待機時間")] float _waitTime = 1.5f;
    [Header("すべての子オブジェクト")]
    [SerializeField, Tooltip("すべての子オブジェクト")] Move[] _moveComponents = default;

    void Start()
    {
        _wfs = new WaitForSeconds(_waitTime);
    }

    void Update()
    {
        if (_isGetComponents)
        {
            // 子オブジェクトを全部取得
            //_childrenAmount = transform.childCount;
            _moveComponents = GetComponentsInChildren<Move>();
            _isGetComponents = false;
        }
        if (_isRunawayTurn)
        {
            StartCoroutine(PrepareAndMove());

            //for (var i = 0; i < _moveComponents.Length; i++)
            //{
            //    //    //StartCoroutine(PrepareAndMove(i));

            //    //    //_moveComponents[i].IsPrepare = true;
            //    //    //_moveComponents[i].IsMove = true;

            //    _moveComponents[i].CurrentState = Move.State.Prepare;

            //}
            _isRunawayTurn = false;
        }
    }

    IEnumerator PrepareAndMove()
    {
        for (var i = 0; i < _moveComponents.Length; i++)
        {
            _moveComponents[i].CurrentState = Move.State.Prepare;
            yield return _wfs;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state">変えたいステート</param>
    void Runaway(Move.State state)
    {
        for (var i = 0; i < _moveComponents.Length; i++)
        {
            _moveComponents[i].CurrentState = state;
        }
        Debug.Log(state);
    }
}
