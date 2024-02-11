using OpenCover.Framework.Model;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// クリックした位置に逃げる者が居たら、叩く
/// 居なければ何もできない
/// </summary>
public class Strike : MonoBehaviour
{
    [Header("対象の縦横のインデックス番号")]
    [SerializeField, Tooltip("対象の縦のインデックス番号")] int _depth = default;
    [SerializeField, Tooltip("対象の横のインデックス番号")] int _width = default;
    [Header("カメラ")]
    [SerializeField] Camera _cameraObject = default;
    RaycastHit _hit = default;
    [Header("叩く側のターンか")]
    [SerializeField, Tooltip("叩く側のターンか")] bool _isStrikeTurn = false;
    [Header("叩く場所を決定したか")]
    [SerializeField, Tooltip("叩く場所を決定したか")] bool _isDecision = false;
    //[Header("グリッドを成す並びのオブジェクトの、配列を保持しているクラス")]
    //[SerializeField, Tooltip("グリッドを成す並びのオブジェクトの、配列を保持しているクラス")] LineUpObjects _lineUpObjects = default;

    //public delegate void OnCompleteDelegate();
    //public OnCompleteDelegate onComplete;

    void Start()
    {
        _isStrikeTurn = false;
        _isDecision = false;
    }

    void Update()
    {
        // 第２フェーズでは使わない
        if (GameManager.Instance.NowPhaseState == GameManager.PhaseState.SecondPhase)
        {
            return;
        }

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
            //GameManager.Instance.NowTurnState = GameManager.TurnState.MoveRunaway;

            //onComplete = MyMethod;
            //// コールバック
            //onComplete();

            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.MoveRunaway);
            _isDecision = false;
        }

        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.Check)
        {
            Debug.Log(Check(_depth, _width));
            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.DeadOrLive);
        }

        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.DeadOrLive)
        {
            StrikeTarget();
            Debug.Log("StrikeTarget");
            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.Update);
        }
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
        // 当たったオブジェクトの位置情報から、インデックス番号を探す
        var hitZ = _hit.collider.gameObject.transform.position.z;
        var hitX = _hit.collider.gameObject.transform.position.x;
        for (var i = 0; i < GridManager.Instance.VectorArray.GetLength(0); i++)
        {
            for (var j = 0; j < GridManager.Instance.VectorArray.GetLength(1); j++)
            {
                if (GridManager.Instance.VectorArray[i, j].x == hitX && GridManager.Instance.VectorArray[i, j].z == hitZ)
                {
                    _depth = i;
                    _width = j;
                    Debug.Log(GridManager.Instance.IntArray[_depth, _width]);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 逃げる側が居たかどうか
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="width"></param>
    /// <returns> 真：当たり、　偽：外れ </returns>
    bool Check(int depth, int width)
    {
        if (GridManager.Instance.IntArray[depth, width] == (int)GridManager.GridState.Exist)
        {
            Debug.Log("当たり");
            return true;
        }
        else
        {
            Debug.Log("外れ");
            return false;
        }
    }

    /// <summary>
    /// destroyする
    /// </summary>
    void StrikeTarget()
    {
        var gameManager = GameManager.Instance;
        gameManager.RecordStrikePoint(_depth, _width);

        var gridManager = GridManager.Instance;
        var array = gridManager.GameObjectArray;
        if (array[_depth, _width].transform.childCount != 0)
        {
            //Destroy(array[_depth, _width].transform.GetChild(0).gameObject);
            // FindGameObjectsWithTagは非アクティブなオブジェクトを探せない　

            // →　非アクティブにするだけで、あとはリストを更新するだけでいい
            for (var i = 0; i < array[_depth, _width].transform.childCount; i++)
            {
                array[_depth, _width].transform.GetChild(i).gameObject.SetActive(false);
                // 非表示にしているだけで、子オブジェクトままであるため
            }

            // 状態を更新
            gridManager.ChangeArray(GridManager.GridState.Empty, _depth, _width);
        }
        gameManager.AddTurnCount(1);
    }
}