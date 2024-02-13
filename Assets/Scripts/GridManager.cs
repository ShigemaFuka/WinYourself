using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Tooltip("インスタンスを取得するためのパブリック変数")] public static GridManager Instance = default;
    [SerializeField, Tooltip("縦の個数")] int _depth = 4;
    [SerializeField, Tooltip("横の個数")] int _width = 4;
    [Tooltip("各グリッドの状態をintで格納した配列")] static int[,] _intArray = default;
    [Tooltip("各グリッドの位置情報を格納した配列")] static Vector3[,] _vectorArray = default;
    [Tooltip("グリッドを成すゲームオブジェクト")] static GameObject[,] _standArray = default;
    [Tooltip("逃げる側のオブジェクトの配列")] static List<GameObject> _runawayList = default;

    #region プロパティ

    /// <summary> 縦の個数 </summary>
    public int Depth
    {
        get => _depth;
    }

    /// <summary> 横の個数 </summary>
    public int Width
    {
        get => _width;
    }

    /// <summary> 各グリッドの「状態」をintで格納した配列 </summary>
    public int[,] IntArray
    {
        get => _intArray; /*set => _intArray = value;*/
    }

    /// <summary> 各グリッドの「位置情報」を格納した配列 </summary>
    public Vector3[,] VectorArray
    {
        get => _vectorArray;
        set => _vectorArray = value;
    }

    /// <summary> グリッドを成すゲームオブジェクト </summary>
    public GameObject[,] StandArray
    {
        get => _standArray;
        set => _standArray = value;
    }

    /// <summary> 逃げる側のオブジェクトの配列 </summary>
    public List<GameObject> RunawayList
    {
        get => _runawayList;
        set => _runawayList = value;
    }

    #endregion

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public enum GridState
    {
        /// <summary> 移動先の候補 </summary>
        Option = -1,

        /// <summary> 空 </summary>
        Empty,

        /// <summary> 既にいる </summary>
        Exist
    }

    void Start()
    {
        _intArray = new int[Depth, Width];
        VectorArray = new Vector3[Depth, Width];
        StandArray = new GameObject[Depth, Width];
        RunawayList = new List<GameObject>();
    }

    void Update()
    {
    }

    /// <summary>
    /// 特定のグリッドの状態を知ることができる
    /// </summary>
    /// <param name="d"> 縦 </param>
    /// <param name="w"> 横 </param>
    /// <returns>-1 : 予約 0 : 何もない 1 : すでにいる -100 : out of range</returns>
    public int CheckArray(int d, int w)
    {
        if (d >= Depth || w >= Width || d < 0 || w < 0)
            return -100;
        return _intArray[d, w];
    }

    /// <summary>
    /// 特定のグリッドの状態を変更する
    /// </summary>
    /// <param name="state">変えたいステート：　-1, 0, 1</param>
    /// <param name="d"></param>
    /// <param name="w"></param>
    public void ChangeArray(GridState state, int d, int w)
    {
        _intArray[d, w] = (int)state;
    }

    public void SetInitializeVector(GameObject[,] go, int i, int j)
    {
        VectorArray[i, j] = go[i, j].transform.position;
    }

    public Vector3 UseVector(int i, int j)
    {
        return VectorArray[i, j];
    }
}