using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����̃I�u�W�F�N�g����Ղ̖ڂ̂悤�ɔz�u����
/// </summary>
public class LineUpUI : MonoBehaviour
{
    [SerializeField, Tooltip("�z�u�������I�u�W�F�N�g")] GameObject _object = default;
    [Tooltip("�c�̌�")] int _height = 0;
    [Tooltip("���̌�")] int _wide = 0;
    [SerializeField, Tooltip("�c�̊Ԋu")] float _heightInterval = 1.0f;
    [SerializeField, Tooltip("���̊Ԋu")] float _wideInterval = 1.0f;
    [Tooltip("UI")] GameObject[,] _gameObjectArray = default;
    [Tooltip("��ƂȂ�ʒu")] Vector3 _standardPos = default;

    [Tooltip("�e�L�X�g�@�C���f�b�N�X�ԍ�")] Text[,] _indexArray = default;
    [Tooltip("�e�L�X�g�@���")] Text[,] _numArray = default;
    [SerializeField] bool _isShow = false;

    #region �v���p�e�B
    public GameObject[,] GameObjectArray { get => _gameObjectArray; }
    #endregion

    void Start()
    {
        _height = GridManager.Instance.Depth;
        _wide = GridManager.Instance.Wide;
        _standardPos = transform.position;
        _gameObjectArray = new GameObject[_height, _wide];

        _indexArray = new Text[_height, _wide];
        _numArray = new Text[_height, _wide];

        for (var i = 0; i < _height; i++)
        {
            for (var j = 0; j < _wide; j++)
            {
                // ���g�̎q�I�u�W�F�N�g�ɂ���
                var go = Instantiate(_object, transform);
                if (go)
                {
                    float xPos = j * _wideInterval;
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
                for (var j = 0; j < _wide; j++)
                {
                    ShowText(i, j);
                }
            }
            _isShow = false;
        }
    }

    void GetComponets()
    {
        for (var i = 0; i < _height; i++)
        {
            for (var j = 0; j < _wide; j++)
            {
                _indexArray[i, j] = _gameObjectArray[i, j].transform.Find("Text_Index").GetComponent<Text>();
                _numArray[i, j] = _gameObjectArray[i, j].transform.Find("Text_Num").GetComponent<Text>();
            }
        }
    }

    void ShowText(int i, int j)
    {
        _indexArray[i, j].text = $"{i}, {j}";
        if (GridManager.Instance.Array != null)
            _numArray[i, j].text = $"{GridManager.Instance.Array[i, j]}";
    }
}