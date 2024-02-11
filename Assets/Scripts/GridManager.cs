using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Tooltip("インスタンスを取得するためのパブリック変数")] public static GridManager Instance = default;
    [SerializeField, Tooltip("縦の個数")] int _depth = 4;
    [SerializeField, Tooltip("横の個数")] int _width = 4;
    [Tooltip("各グリッドの状態をintで格納した配列")] static int[,] _intArray = default;
    [Tooltip("各グリッドの位置情報を格納した配列")] static Vector3[,] _vectorArray = default;
    [Tooltip("グリッドを成すゲームオブジェクト")] static GameObject[,] _gameObjectArray = default;

    #region プロパティ
    /// <summary> 縦の個数 </summary>
    public int Depth { get => _depth; }

    /// <summary> 横の個数 </summary>
    public int Width { get => _width; }

    /// <summary> 各グリッドの「状態」をintで格納した配列 </summary>
    public int[,] IntArray { get => _intArray; /*set => _intArray = value;*/ }

    /// <summary> 各グリッドの「位置情報」を格納した配列 </summary>
    public Vector3[,] VectorArray { get => _vectorArray; set => _vectorArray = value; }

    /// <summary> グリッドを成すゲームオブジェクト </summary>
    public GameObject[,] GameObjectArray { get => _gameObjectArray; set => _gameObjectArray = value; }
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
        GameObjectArray = new GameObject[Depth, Width];
    }

    void Update()
    {

    }

    /// <summary>
    /// -1 : 予約
    /// 0 : 何もない
    /// 1 : すでにいる
    /// -100 : out of range
    /// </summary>
    /// <param name="d"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    public int CheckArray(int d, int w)
    {
        if (d >= Depth || w >= Width || d < 0 || w < 0)
            return -100;
        return _intArray[d, w];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state">-1, 0, 1</param>
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
