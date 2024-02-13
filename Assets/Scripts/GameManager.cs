using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの状態管理をする
/// 逃げる側のターンと、叩く側のターンを管理
/// </summary>
public class GameManager : MonoBehaviour
{
    #region 宣言

    [Tooltip("インスタンスを取得するためのパブリック変数")] public static GameManager Instance = default;

    [Header("現在のゲームステート")] [SerializeField, Tooltip("現在のゲームステート")]
    GameState _gameState = GameState.InGame;

    [Header("現在のシーンステート")] [SerializeField, Tooltip("現在のシーンステート")]
    SceneState _sceneState = SceneState.InGame;

    [Header("現在のプロセスステート")] [SerializeField, Tooltip("現在のプロセスステート")]
    ProcessState _nowProcessState = ProcessState.None;

    [Header("１フレーム前のプロセスステート")] [SerializeField, Tooltip("１フレーム前のプロセスステート")]
    ProcessState _oldProcessState = ProcessState.None;

    [Header("現在のフェーズ（インゲーム中）")] [SerializeField, Tooltip("現在のフェーズ（インゲーム中）")]
    PhaseState _nowPhaseState = PhaseState.FirstPhase;

    [Header("１フレーム前のフェーズ（インゲーム中）")] [SerializeField, Tooltip("１フレーム前のフェーズ（インゲーム中）")]
    PhaseState _oldPhaseState = PhaseState.FirstPhase;

    [Header("何ターン制か")] [SerializeField, Tooltip("何ターン制か")]
    int _totalTurn = default;

    [Header("何ターン経過したか")] [SerializeField, Tooltip("何ターン経過したか")]
    int _turnCount = default;

    [Header("選択した箇所の記録")] [SerializeField, Tooltip("選択した箇所の記録")]
    StrikePoint _strikePoint = default;

    [Header("リスト（選択した箇所の記録）")] [SerializeField, Tooltip("リスト（選択した箇所の記録）")]
    List<StrikePoint> _strikePointList = default;

    #endregion

    #region プロパティ

    /// <summary> 現在のプロセスステート </summary>
    public ProcessState NowProcessState
    {
        get => _nowProcessState;
    }

    /// <summary> １フレーム前のプロセスステート </summary>
    public ProcessState OldProcessState
    {
        get => _oldProcessState;
    }

    /// <summary> 現在のプロセスステート </summary>
    public PhaseState NowPhaseState
    {
        get => _nowPhaseState;
    }

    /// <summary> １フレーム前のプロセスステート </summary>
    public PhaseState OldPhaseState
    {
        get => _oldPhaseState;
    }

    /// <summary> 何ターン制か </summary>
    public int TotalTurn
    {
        get => _totalTurn;
    }

    /// <summary> 何ターン経過したか </summary>
    public int TurnCount
    {
        get => _turnCount;
    }

    /// <summary> 何ターン経過したか </summary>
    public List<StrikePoint> StrikePointList
    {
        get => _strikePointList;
        set => _strikePointList = value;
    }

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

    /// <summary> プロセス状態を管理する列挙型 </summary>
    public enum ProcessState
    {
        None,

        /// <summary> 初期位置設定 </summary>
        SetStartPosition,
        
        /// <summary> 第２フェーズで使用 </summary>
        ListUpdate, 

        /// <summary> 逃げる側が出現 </summary>
        ShowRunaway, // 読み取られてはいない

        /// <summary> 更新 </summary>
        Update, // 読み取られてはいない
        // 恐らく、ステートの切り替えが早すぎて、思った挙動になっていない
        // その対策として、手動でRunawayControllerの_isGetComponentsを真にしてるから、要らないかも

        /// <summary> 第１フェーズで使用 叩く位置 記録 </summary>
        Record,

        /// <summary> 第２フェーズで使用 叩く位置 設定 </summary>
        PickUp,

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
        [Tooltip("プレイヤーが叩く側")] FirstPhase,
        [Tooltip("プレイヤーが逃げる側")] SecondPhase,
    }

    void Start()
    {
    }

    void Update()
    {
        _oldProcessState = _nowProcessState;
        _oldPhaseState = _nowPhaseState;

        if (TurnCount >= TotalTurn)
        {
            if (_nowPhaseState == PhaseState.FirstPhase)
            {
                _nowPhaseState = PhaseState.SecondPhase;
                _turnCount = 0;
            }
            else
            {
                Debug.Log("第二フェーズのターンが終了");
            }
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
        _strikePoint._depth = depth;
        _strikePoint._width = width;
        _strikePointList.Add(_strikePoint);
    }

    [System.Serializable]
    public struct StrikePoint // 構造体の定義
    {
        public int _depth;

        public int _width;
        //public int Depth { get => depth; set => depth = value; }
        //public int Width { get => width; set => width = value; }
    }

    /// <summary>
    /// ボタンで次のフェーズに進むことを確認させるときに、
    /// プロセスステートを変更
    /// </summary>
    // public void OnClick()
    // {
    //     _nowProcessState = ProcessState.PickUp;
    // }
}