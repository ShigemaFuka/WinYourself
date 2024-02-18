using UnityEngine;

/// <summary>
/// クリックした位置に逃げる者が居たら、叩く
/// 居なければ何もできない
/// </summary>
public class FirstPhaseStrike : MonoBehaviour
{
    [Header("対象の縦横のインデックス番号")] [SerializeField, Tooltip("対象の縦のインデックス番号")]
    int _depth = default;

    [SerializeField, Tooltip("対象の横のインデックス番号")]
    int _width = default;

    [Header("カメラ")] [SerializeField] Camera _cameraObject = default;
    [Tooltip("ヒットしたもの")] RaycastHit _hit = default;

    [Header("叩く側のターンか")] [SerializeField, Tooltip("叩く側のターンか")]
    bool _isStrikeTurn = false;

    [Header("叩く場所を決定したか")] [SerializeField, Tooltip("叩く場所を決定したか")]
    bool _isDecision = false;

    //public delegate void OnCompleteDelegate();
    //public OnCompleteDelegate onComplete;

    [Header("当たりか外れか")] [SerializeField, Tooltip("当たりか外れか")]
    bool _isHit = false;

    #region プロパティ

    /// <summary> 対象の縦のインデックス番号 </summary>
    public int Depth
    {
        get => _depth;
        set => _depth = value;
    }

    /// <summary> 対象の横のインデックス番号 </summary>
    public int Width
    {
        get => _width;
        set => _width = value;
    }

    #endregion

    void Start()
    {
        _isStrikeTurn = false;
        _isDecision = false;
    }

    void Update()
    {
        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.Check)
        {
            Check(Depth, Width);
            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.DeadOrLive);
        }

        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.DeadOrLive)
        {
            StrikeTarget();
            Debug.Log("StrikeTarget");
            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.Update);
        }

        if (GameManager.Instance.NowPhaseState == GameManager.PhaseState.SecondPhase)
        {
            return;
        } // 以降の処理は、第２フェーズでは使わない

        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.Record)
        {
            _isStrikeTurn = true;
        }

        if (_isStrikeTurn)
        {
            Click();
        }

        // uiか何かで、決定しますか？と聞く
        if (_isDecision)
        {
            _isStrikeTurn = false;
            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.MoveRunaway);
            _isDecision = false;
            //onComplete = MyMethod;
            //// コールバック
            //onComplete();
        }

        // if (GameManager.Instance.NowProcessState == GameManager.ProcessState.Check)
        // {
        //     Check(_depth, _width);
        //     GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.DeadOrLive);
        // }

        // if (GameManager.Instance.NowProcessState == GameManager.ProcessState.DeadOrLive)
        // {
        //     StrikeTarget();
        //     Debug.Log("StrikeTarget");
        //     GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.Update);
        // }
    }

    /// <summary>
    /// マウス左クリックでレイを飛ばして、Standタグがついた台となるオブジェクトを見る
    /// </summary>
    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //マウスのポジションを取得してRayに代入
            Ray ray = _cameraObject.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out _hit))
            {
                // 土台であるかどうか
                if (_hit.collider.gameObject.CompareTag("Stand"))
                {
                    RecordSelection();
                }
            }
        }
    }

    /// <summary>
    /// 選択した場所のインデックス番号を記録
    /// </summary>
    void RecordSelection()
    {
        var vectorArray = GridManager.Instance.VectorArray;
        var hitPoint = _hit.collider.gameObject.transform.position;
        // 当たったオブジェクトの位置情報から、インデックス番号を探す
        var hitZ = hitPoint.z;
        var hitX = hitPoint.x;
        for (var i = 0; i < GridManager.Instance.VectorArray.GetLength(0); i++)
        {
            for (var j = 0; j < GridManager.Instance.VectorArray.GetLength(1); j++)
            {
                if (vectorArray[i, j].x == hitX && vectorArray[i, j].z == hitZ)
                {
                    Depth = i;
                    Width = j;
                    Debug.Log(GridManager.Instance.IntArray[Depth, Width]);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 逃げる側が居たかどうか
    /// _isHit ( 真：当たり、　偽：外れ )
    /// </summary>
    /// <param name="depth"> 縦 </param>
    /// <param name="width"> 横 </param>
    public void Check(int depth, int width)
    {
        if (GridManager.Instance.IntArray[depth, width] == (int)GridManager.GridState.Exist)
        {
            Debug.Log($"当たり : {depth}  {width}");
            _isHit = true;
        }
        else
        {
            Debug.Log("外れ");
            _isHit = false;
        }
    }

    /// <summary>
    /// 記録と非アクティブ化
    /// </summary>
    void StrikeTarget()
    {
        var gameManager = GameManager.Instance;
        // 第二フェーズでは記録不要
        if (GameManager.Instance.NowPhaseState == GameManager.PhaseState.FirstPhase)
            gameManager.RecordStrikePoint(Depth, Width);

        var gridManager = GridManager.Instance;
        var array = gridManager.StandArray;
        if (_isHit)
        {
            // 非アクティブにするだけで、あとはリストを更新するだけでいい
            for (var i = 0; i < array[_depth, _width].transform.childCount; i++)
            {
                // 非表示にしているだけで、子オブジェクトままであるため
                array[_depth, _width].transform.GetChild(i).gameObject.SetActive(false);
            }

            // 状態を更新
            gridManager.ChangeArray(GridManager.GridState.Empty, _depth, _width);
        }

        gameManager.AddTurnCount(1);
    }

    /// <summary>
    /// ボタンで真偽を切り替える
    /// </summary>
    public void OnClick(bool flag)
    {
        _isDecision = flag;
    }
}