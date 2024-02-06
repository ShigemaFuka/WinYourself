using UnityEngine;
/// <summary>
/// オブジェクトをグリッドのどこを初期位置にするか
/// boolの切り替えで配置できる
/// </summary>
public class SetStartPosition : MonoBehaviour
{
    [SerializeField, Tooltip("グリッドを保持しているクラス")] LineUpObjects _lineUpObjects = default;
    [Header("置きたい場所のインデックス番号（縦横）")]
    [Tooltip("置きたい場所（縦）")] int _depth = 0;
    [Tooltip("置きたい場所（横）")] int _wide = 0;
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

        // 何もないところにのみ置く
        while (GridManager.Instance.CheckArray(_depth, _wide) != 0)
        {
            _depth = Random.Range(0, array.GetLength(0));
            _wide = Random.Range(0, array.GetLength(1));
            _position = array[_depth, _wide].transform.position;
        }
        GridManager.Instance.ChangeArray(GridManager.GridState.Exist, _depth, _wide);
        var go = Instantiate(_gameObject, transform);
        go.transform.position = new Vector3(_position.x, _yPos, _position.z);
        //Debug.Log($" {_depth}  {_wide}");
    }
}