using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Tooltip("インスタンスを取得するためのパブリック変数")] public static GridManager Instance = default;
    [SerializeField, Tooltip("縦の個数")] int _depth = 4;
    [SerializeField, Tooltip("横の個数")] int _wide = 4;
    static int[,] _array = default;

    #region プロパティ
    public int Depth { get => _depth; }
    public int Wide { get => _wide; }
    public int[,] Array { get => _array; set => _array = value; }

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
        /// <summary> 予約 </summary>
        Reservation = -1,
        /// <summary> 空 </summary>
        Empty,
        /// <summary> 既にいる </summary>
        Exist
    }

    void Start()
    {
        _array = new int[Depth, Wide];
    }

    void Update()
    {

    }

    /// <summary>
    /// -1 : 予約
    /// 0 : 何もない
    /// 1 : すでにいる
    /// </summary>
    /// <param name="d"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    public int CheckArray(int d, int w)
    {
        return Array[d, w];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state">-1, 0, 1</param>
    /// <param name="d"></param>
    /// <param name="w"></param>
    public void ChangeArray(GridState state, int d, int w)
    {
        Array[d, w] = (int)state;
    }
}
