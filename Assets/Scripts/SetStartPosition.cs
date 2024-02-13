using UnityEngine;

/// <summary>
/// 逃げる側となるオブジェクトをグリッドのどこを初期位置にするか
/// boolが真とき配置できる
/// </summary>
public class SetStartPosition : MonoBehaviour
{
    [Header("置きたい場所のインデックス番号（縦横）")] [Tooltip("置きたい場所（縦）")]
    int _depth = 0;

    [Tooltip("置きたい場所（横）")] int _width = 0;

    [Header("置きたい場所（高さ）")] [SerializeField, Tooltip("置きたい場所（高さ）")]
    float _yPos = 0;

    [Header("置きたいオブジェクト")] [SerializeField, Tooltip("置きたいオブジェクト")]
    GameObject _gameObject = default;

    [Header("オブジェクトの数")] [SerializeField, Tooltip("オブジェクトの数")]
    int _amount = 2;

    [Header("配置するか")] [SerializeField, Tooltip("配置するか")]
    bool _isSet = false;

    [Tooltip("変更するための位置情報")] Vector3 _position = default;
    [Tooltip("グリッドマネージャーのゲームオブジェクト型の配列")] GameObject[,] _array = default;

    GameManager _gameManager = default;

    void Start()
    {
        _array = GridManager.Instance.StandArray;
        _gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (_gameManager.NowProcessState == GameManager.ProcessState.SetStartPosition)
        {
            _isSet = true;
            Debug.Log("ProcessState SetPosition");
        }

        if (_isSet)
        {
            for (var i = 0; i < _amount; i++)
            {
                SetPosition(i);
            }

            Debug.Log("SetPosition");
            _isSet = false;
            if (GameManager.Instance.NowPhaseState == GameManager.PhaseState.FirstPhase)
            {
                GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.ShowRunaway);
            }
            else
            {
                GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.None);
            }
        }
    }

    /// <summary>
    /// 位置調整
    /// </summary>
    public void SetPosition(int index)
    {
        // グリッドが未配置のとき
        if (_array.Length == 0)
        {
            Debug.LogError("グリッドが未配置");
            return;
        }

        string message = "空がありません";
        for (var i = 0; i < _array.Length; i++)
        {
            _depth = Random.Range(0, _array.GetLength(0));
            _width = Random.Range(0, _array.GetLength(1));
            // 空のところだったら位置を変更し、ループを抜ける
            if (GridManager.Instance.CheckArray(_depth, _width) == (int)GridManager.GridState.Empty)
            {
                _position = _array[_depth, _width].transform.position;
                message = $"{_depth} {_width}";
                break;
            }
        }

        // 空きがないときだけ出力で良い
        if (message == "空がありません")
            Debug.LogWarning(message);

        GridManager.Instance.ChangeArray(GridManager.GridState.Exist, _depth, _width);

        GameObject go = default;
        if (GameManager.Instance.NowPhaseState == GameManager.PhaseState.FirstPhase)
        {
            go = Instantiate(_gameObject);
            GridManager.Instance.RunawayList.Add(go);
        }
        else
        {
            // 第二フェーズでは生成せずとも、既にある
            go = GridManager.Instance.RunawayList[index];
        }

        // 親の指定と位置設定
        go.transform.parent = GridManager.Instance.StandArray[_depth, _width].transform;
        go.transform.position = new Vector3(_position.x, _yPos, _position.z);
        var move = go.GetComponent<Move>();
        // 初期位置を教える
        move.CurrentIndex[0] = _depth;
        move.CurrentIndex[1] = _width;
    }

    /// <summary>
    /// ボタンで呼ぶ
    /// </summary>
    /// <param name="flag"></param>
    public void OnClick(bool flag)
    {
        _isSet = flag;
        Debug.LogWarning("_isSet");
    }
}