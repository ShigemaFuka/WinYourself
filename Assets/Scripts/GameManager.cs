using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// ゲームの状態管理をする
/// NPCのターンと、ハンマーのターンを管理
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("インスタンスを取得するためのパブリック変数")] public static GameManager Instance = default;

    [Header("現在のゲームステート")]
    [SerializeField, Tooltip("現在のゲームステート")] GameState _gameState = GameState.InGame;
    [Header("現在のシーンステート")]
    [SerializeField, Tooltip("現在のシーンステート")] SceneState _sceneState = SceneState.InGame;
    [Header("現在のプロセスステート")]
    [SerializeField, Tooltip("現在のプロセスステート")] ProcessState _nowProcessState = ProcessState.None;
    [Header("１フレーム前のプロセスステート")]
    [SerializeField, Tooltip("１フレーム前のプロセスステート")] ProcessState _oldProcessState = ProcessState.None;

    [Header("現在のフェーズ（インゲーム中）")]
    [SerializeField, Tooltip("現在のフェーズ（インゲーム中）")] PhaseState _nowPhaseState = PhaseState.FistPhase;

    [Header("何ターン制か")]
    [SerializeField, Tooltip("何ターン制か")] int _totalTurn = default;
    [Header("何ターン経過したか")]
    [SerializeField, Tooltip("何ターン経過したか")] int _turnCount = default;

    [Header("選択した箇所の記録")]
    [SerializeField, Tooltip("選択した箇所の記録")]
    StrikePoint _strikePoint = default;

    [Header("リスト（選択した箇所の記録）")]
    [SerializeField, Tooltip("リスト（選択した箇所の記録）")] List<StrikePoint> _strikePointList = default;

    #region プロパティ
    /// <summary> 現在のプロセスステート </summary>
    public ProcessState NowProcessState { get => _nowProcessState; }

    /// <summary> １フレーム前のプロセスステート </summary>
    public ProcessState OldProcessState { get => _oldProcessState; }

    /// <summary> １フレーム前のプロセスステート </summary>
    public PhaseState NowPhaseState { get => _nowPhaseState; }

    /// <summary> 何ターン制か </summary>
    public int TotalTurn { get => _totalTurn; }

    /// <summary> 何ターン経過したか </summary>
    public int TurnCount { get => _turnCount; }
    #endregion

    /// <summary>
    /// GameManagerのインスタンスを格納
    /// </summary>
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    /// <summary> ゲームの状態を管理する列挙型 </summary>
    public enum GameState
    {
        Start,
        InGame,
        Pause,
        GameOver,
        Result,
    }

    /// <summary> シーンの状態を管理する列挙型 </summary>
    public enum SceneState
    {
        Start,
        InGame,
        //GameOver,
        Result,
    }

    /// <summary> （卵側とハンマー側かの）状態を管理する列挙型 </summary>
    public enum ProcessState
    {
        None,

        /// <summary> 初期位置設定 </summary>
        SetStartPosition,

        /// <summary> 逃げる側が出現 </summary>
        ShowRunaway,

        /// <summary> 更新 </summary>
        Update,
        // 恐らく、ステートの切り替えが早すぎて、思った挙動になっていない
        // その対策として、手動でRunawayControllerの_isGetComponentsを真にしてるから、要らないかも

        /// <summary> 叩く位置記録 </summary>
        Record,

        /// <summary> 逃げる側が行動 </summary>
        MoveRunaway,

        /// <summary> 叩けるか判定 </summary>
        Check,

        /// <summary> 逃げる側が消されるor免れる </summary>
        DeadOrLive
    }

    /// <summary> インゲームのフェーズの状態を管理する列挙型 </summary>
    public enum PhaseState
    {
        [Tooltip("プレイヤーが叩く側")]
        FistPhase,
        [Tooltip("プレイヤーが逃げる側")]
        SecondPhase,
    }

    void Start()
    {

    }

    void Update()
    {
        _oldProcessState = _nowProcessState;

        if (TurnCount >= TotalTurn)
        {
            _nowPhaseState = PhaseState.SecondPhase;
        }
    }

    /// <summary>
    /// 現在のプロセスステート変更
    /// </summary>
    /// <param name="state">変えたいステート</param>
    public void ChangeNowProcessState(ProcessState state)
    {
        _nowProcessState = state;
    }

    /// <summary>
    /// ターンを加算
    /// </summary>
    /// <param name="value"></param>
    public void AddTurnCount(int value)
    {
        _turnCount += value;
    }

    /// <summary>
    /// 叩いた（選択した）箇所を記録する
    /// </summary>
    public void RecordStrikePoint(int depth, int width)
    {
        _strikePoint.depth = depth;
        _strikePoint.width = width;
        _strikePointList.Add(_strikePoint);
    }

    [System.Serializable]
    struct StrikePoint // 構造体の定義
    {
        public int depth;
        public int width;
    }
}