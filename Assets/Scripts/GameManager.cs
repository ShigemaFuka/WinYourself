using UnityEngine;
/// <summary>
/// ゲームの状態管理をする
/// NPCのターンと、ハンマーのターンを管理
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("インスタンスを取得するためのパブリック変数")] public static GameManager Instance = default;
    [SerializeField, Tooltip("現在のゲームステート")] GameState _currentState = GameState.InGame;

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

    /// <summary> 卵側とハンマー側かの状態を管理する列挙型 </summary>
    public enum TurnState
    {
        /// <summary> 卵側：逃げる側 </summary>
        Egg,
        /// <summary> ハンマー側：叩く側 </summary>
        Hammer
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
