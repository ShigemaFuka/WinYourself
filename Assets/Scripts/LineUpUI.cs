using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 特定のオブジェクトを碁盤の目のように配置する
/// GridManagerのステートが変更するたびに、UIの表示も変更する
/// </summary>
public class LineUpUI : MonoBehaviour
{
    [SerializeField, Tooltip("配置したいオブジェクト")] GameObject _object = default;
    [Tooltip("縦の個数")] int _height = 0;
    [Tooltip("横の個数")] int _width = 0;
    [SerializeField, Tooltip("縦の間隔")] float _heightInterval = 1.0f;
    [SerializeField, Tooltip("横の間隔")] float _widthInterval = 1.0f;
    [Tooltip("UI")] GameObject[,] _gameObjectArray = default;
    [Tooltip("基準となる位置")] Vector3 _standardPos = default;

    [Tooltip("テキスト　インデックス番号")] Text[,] _indexArray = default;
    [Tooltip("テキスト　状態")] Text[,] _numArray = default;
    [SerializeField] bool _isShow = false;

    #region プロパティ
    public GameObject[,] GameObjectArray { get => _gameObjectArray; }
    #endregion

    void Start()
    {
        _height = GridManager.Instance.Depth;
        _width = GridManager.Instance.Width;
        _standardPos = transform.position;
        _gameObjectArray = new GameObject[_height, _width];

        _indexArray = new Text[_height, _width];
        _numArray = new Text[_height, _width];

        for (var i = 0; i < _height; i++)
        {
            for (var j = 0; j < _width; j++)
            {
                // 自身の子オブジェクトにする
                var go = Instantiate(_object, transform);
                if (go)
                {
                    float xPos = j * _widthInterval;
                    float yPos = -i * _heightInterval;
                    go.transform.position = _standardPos + new Vector3(xPos, yPos, 0f);
                    _gameObjectArray[i, j] = go;
                }
            }
        }
        GetComponets();
    }

    void Update()
    {
        if (_isShow)
        {
            for (var i = 0; i < _height; i++)
            {
                for (var j = 0; j < _width; j++)
                {
                    ShowText(i, j);
                }
            }
            //_isShow = false;
        }
    }

    void GetComponets()
    {
        for (var i = 0; i < _height; i++)
        {
            for (var j = 0; j < _width; j++)
            {
                _indexArray[i, j] = _gameObjectArray[i, j].transform.Find("Text_Index").GetComponent<Text>();
                _numArray[i, j] = _gameObjectArray[i, j].transform.Find("Text_Num").GetComponent<Text>();
            }
        }
    }

    void ShowText(int i, int j)
    {
        _indexArray[i, j].text = $"{i}, {j}";
        if (GridManager.Instance.IntArray != null)
            _numArray[i, j].text = $"{GridManager.Instance.IntArray[i, j]}";
    }
}