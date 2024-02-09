using UnityEngine;
/// <summary>
/// 逃げる側となるオブジェクトをグリッドのどこを初期位置にするか
/// boolが真とき配置できる
/// </summary>
public class SetStartPosition : MonoBehaviour
{
    [SerializeField, Tooltip("グリッドを成す並びのオブジェクトの、配列を保持しているクラス")] LineUpObjects _lineUpObjects = default;
    [Header("置きたい場所のインデックス番号（縦横）")]
    [Tooltip("置きたい場所（縦）")] int _depth = 0;
    [Tooltip("置きたい場所（横）")] int _width = 0;
    [Header("置きたい場所（高さ）")]
    [SerializeField, Tooltip("置きたい場所（高さ）")] float _yPos = 0;
    [Header("置きたいオブジェクト")]
    [SerializeField, Tooltip("置きたいオブジェクト")] GameObject _gameObject = default;
    [Header("オブジェクトの数")]
    [SerializeField, Tooltip("オブジェクトの数")] int _amount = 2;
    [Header("配置するか")]
    [SerializeField, Tooltip("配置するか")] bool _isSet = false;
    Vector3 _position = default;

    void Start()
    {

    }

    void Update()
    {
        if (_isSet)
        {
            for (var i = 0; i < _amount; i++)
            {
                SetPosition();
            }
            _isSet = false;
        }
    }

    /// <summary>
    /// 位置調整
    /// </summary>
    void SetPosition()
    {
        var array = _lineUpObjects.GameObjectArray;

        // グリッドが未配置のとき
        if (array.Length == 0)
        {
            Debug.LogError("グリッドが未配置");
            return;
        }

        string message = "空がありません";
        for (var i = 0; i < array.Length; i++)
        {
            _depth = Random.Range(0, array.GetLength(0));
            _width = Random.Range(0, array.GetLength(1));
            // 空のところだったら位置を変更し、ループを抜ける
            if (GridManager.Instance.CheckArray(_depth, _width) == (int)GridManager.GridState.Empty)
            {
                _position = array[_depth, _width].transform.position;
                message = $"{_depth} {_width}";
                break;
            }
        }
        // 空きがないときだけ出力で良い
        if (message == "空がありません")
            Debug.LogWarning(message);

        GridManager.Instance.ChangeArray(GridManager.GridState.Exist, _depth, _width);
        var go = Instantiate(_gameObject, transform);
        go.transform.position = new Vector3(_position.x, _yPos, _position.z);
        var move = go.GetComponent<Move>();

        move.CurrentIndex[0] = _depth;
        move.CurrentIndex[1] = _width;
    }
}