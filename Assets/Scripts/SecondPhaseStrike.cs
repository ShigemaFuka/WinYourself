using UnityEngine;

/// <summary>
/// 叩く側
/// 第一フェーズで記録していた、プレイヤーの叩いた場所に依存
///（叩く順番も、プレイヤーが叩いたそのままの順番）
/// </summary>
public class SecondPhaseStrike : MonoBehaviour
{
    [Tooltip("ゲームマネジャーのインスタンス")] GameManager _gameManager = default;

    [Header("叩く側の処理クラス")] [SerializeField, Tooltip("叩く側の処理クラス")]
    FirstPhaseStrike _firstPhaseStrike = default;

    [Header("叩く箇所：縦")] [SerializeField, Tooltip("叩く箇所：縦")]
    int _depth = default;

    [Header("叩く箇所：横")] [SerializeField, Tooltip("叩く箇所：横")]
    int _width = default;

    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    void Update()
    {
        // 第1フェーズでは使わない
        if (_gameManager.NowPhaseState == GameManager.PhaseState.FirstPhase)
        {
            return;
        }

        // 登録されたリストの要素を順に見る
        if (_gameManager.NowProcessState == GameManager.ProcessState.PickUp)
        {
            // 第一フェーズの機能を使えるように、縦横の情報を入れる
            _depth = _gameManager.StrikePointList[_gameManager.TurnCount]._depth;
            _width = _gameManager.StrikePointList[_gameManager.TurnCount]._width;
            _firstPhaseStrike.Depth = _depth;
            _firstPhaseStrike.Width = _width;

            // 逃げる側が行動する
            _gameManager.ChangeNowProcessState(GameManager.ProcessState.MoveRunaway);
        }
    }
}