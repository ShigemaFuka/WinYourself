using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基本８方向の内の１方向へ移動
/// ８方向とも移動できなければ、その場で留まる
/// 移動範囲は０～。
/// </summary>
public class Move : MonoBehaviour
{
    //[Header("一度に移動できるマスの数")]
    //[SerializeField, Tooltip("一度に移動できるマスの数")] int _step = 1;
    //[Tooltip("縦の移動範囲")] int _depthMax = default;
    //[Tooltip("横の移動範囲")] int _widthMax = default;
    //[Tooltip("移動先X")] int _pointX = 0;
    //[Tooltip("移動先Y")] int _pointY = 0;
    [Tooltip("現在の位置  例）d, w")] int[] _currentIndex = new int[2];
    [Tooltip("移動できる範囲のリスト")] List<string> _mobileList = new();
    [Header("移動できるエリアを探す準備をするか")]
    [SerializeField, Tooltip("移動できるエリアを探す準備をするか")] bool _isPrepare = false;
    [Header("移動するか")]
    [SerializeField, Tooltip("移動するか")] bool _isMove = false;
    [Header("移動先")]
    [SerializeField, Tooltip("移動先")] int[] nums = new int[2];

    #region プロパティ
    public int[] CurrentIndex { get => _currentIndex; set => _currentIndex = value; }
    public bool IsPrepare { get => _isPrepare; set => _isPrepare = value; }
    public bool IsMove { get => _isMove; set => _isMove = value; }
    #endregion

    void Start()
    {
        //Debug.Log("CurrentIndex : " + CurrentIndex[0] + "  " + CurrentIndex[1]);
    }

    void Update()
    {
        if (_isPrepare)
            PreparationForMovement();

        if (_isMove)
            Movement();
    }

    /// <summary>
    /// _isMoveが真になるたびに呼び出す
    /// 移動準備
    /// </summary>
    void PreparationForMovement()
    {
        // 毎回リストを初期化
        _mobileList.Clear();
        AddMobileArea();
        Determine();
        _isPrepare = false;
    }

    /// <summary>
    /// 移動できるエリアをリストに格納
    /// </summary>
    void AddMobileArea()
    {
        var baseY = _currentIndex[0];
        var baseX = _currentIndex[1];

        //_depthMax = GridManager.Instance.Depth;
        //_widthMax = GridManager.Instance.Width;

        for (var i = baseY - 1; i < baseY + 2; i++)
        {
            for (var j = baseX - 1; j < baseX + 2; j++)
            {
                // 今いるところは飛ばす
                if (i == baseY && j == baseX)
                    continue;
                // 空きではないところは飛ばす
                else if (GridManager.Instance.CheckArray(i, j) != (int)GridManager.GridState.Empty)
                    continue;
                else
                {
                    _mobileList.Add($"{i} {j}");
                    GridManager.Instance.ChangeArray(GridManager.GridState.Option, i, j);
                }
            }
        }
    }

    /// <summary>
    /// 移動先を選択
    /// </summary>
    /// <returns></returns>
    string Select()
    {
        // ８方向とも移動できなければ、その場で待機
        if (_mobileList.Count == 0)
            return $"{CurrentIndex[0]} {CurrentIndex[1]}";
        else
            // リストから1つ選んで移動先とする
            return _mobileList[UnityEngine.Random.Range(0, _mobileList.Count)];
    }

    /// <summary>
    /// 移動先の情報を格納
    /// </summary>
    void Determine()
    {
        nums = Array.ConvertAll(Select().Split(), int.Parse);
    }

    void Movement()
    {
        foreach (var item in _mobileList)
        {
            // 移動できるエリアを-1で表示したあとは、自身が移動できるようにエリア内を0（空き）にする
            var nums = Array.ConvertAll(item.Split(), int.Parse);
            GridManager.Instance.ChangeArray(GridManager.GridState.Empty, nums[0], nums[1]);
        }

        // 移動とステートを更新
        var pos = GridManager.Instance.UseVector(nums[0], nums[1]);
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        GridManager.Instance.ChangeArray(GridManager.GridState.Exist, nums[0], nums[1]);

        // 元居た場所を空きにする その場待機のときは空きにしない
        if (_mobileList.Count != 0)
            GridManager.Instance.ChangeArray(GridManager.GridState.Empty, CurrentIndex[0], CurrentIndex[1]);

        // 今いる場所を更新
        CurrentIndex[0] = nums[0];
        CurrentIndex[1] = nums[1];
        Debug.Log($"CurrentIndex : {CurrentIndex[0]} {CurrentIndex[1]}");

        _isMove = false;
    }
}