using UnityEngine;
/// <summary>
/// 特定のオブジェクトを碁盤の目のように配置する
/// </summary>
public class LineUpObjects : MonoBehaviour
{
    [SerializeField, Tooltip("配置したいオブジェクト")] GameObject _object = default;
    [Tooltip("縦の個数")] int _depth = 0;
    [Tooltip("横の個数")] int _wide = 0;
    [SerializeField, Tooltip("縦の間隔")] float _depthInterval = 1.0f;
    [SerializeField, Tooltip("横の間隔")] float _wideInterval = 1.0f;
    [Tooltip("グリッドを成す並びのオブジェクトの配列")] GameObject[,] _gameObjectArray = default;
    [Tooltip("基準となる位置")] Vector3 _standardPos = default;

    #region プロパティ
    public GameObject[,] GameObjectArray { get => _gameObjectArray; }
    #endregion

    void Start()
    {
        _depth = GridManager.Instance.Depth;
        _wide = GridManager.Instance.Width;
        // これをアタッチするオブジェクトの位置は(0,0,0)に。
        _standardPos = transform.position;
        _gameObjectArray = new GameObject[_depth, _wide];

        for (var i = 0; i < _depth; i++)
        {
            for (var j = 0; j < _wide; j++)
            {
                // 自身の子オブジェクトにする
                var go = Instantiate(_object, transform);
                if (go)
                {
                    float xPos = j * _wideInterval;
                    float zPos = -i * _depthInterval;
                    go.transform.position = _standardPos + new Vector3(xPos, 0f, zPos);
                    _gameObjectArray[i, j] = go;
                    // 
                }
            }
        }
        SetVector();
    }

    /// <summary>
    /// GridManagerに位置情報を設定する
    /// </summary>
    void SetVector()
    {
        var array = GameObjectArray;

        // グリッドが未配置のとき
        if (array.Length == 0)
        {
            Debug.LogError("グリッドが未配置");
            return;
        }

        for (var i = 0; i < array.GetLength(0); i++)
        {
            for (var j = 0; j < array.GetLength(1); j++)
            {
                // 位置情報を設定する
                GridManager.Instance.SetInitializeVector(array, i, j);
            }
        }
    }
}
