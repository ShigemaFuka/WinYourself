using UnityEngine;

/// <summary>
/// クリックした位置に逃げる者が居たら、叩く
/// 居なければ何もできない
/// </summary>
public class Strike : MonoBehaviour
{
    [Header("縦横のインデックス番号")]
    [SerializeField, Tooltip("縦のインデックス番号")] int _depth = default;
    [SerializeField, Tooltip("横のインデックス番号")] int _width = default;
    [Header("カメラ")]
    [SerializeField] Camera _cameraObject = default;
    RaycastHit _hit = default;
    [Header("叩く側のターンか")]
    [SerializeField, Tooltip("叩く側のターンか")] bool _isStrikeTurn = false;
    [Header("叩く場所を決定したか")]
    [SerializeField, Tooltip("叩く場所を決定したか")] bool _isDecision = false;

    void Start()
    {
        _isStrikeTurn = false;
    }

    void Update()
    {
        if (_isStrikeTurn)
        {
            Click();
        }
        if (_isDecision)
            _isStrikeTurn = false;
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
                    //return GridManager.Instance.IntArray[_depth, _width];
                    //_isStrikeTurn = false;
                    return;
                }
            }
        }
        //return -100;
    }
}
