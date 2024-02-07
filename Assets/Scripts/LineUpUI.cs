using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����̃I�u�W�F�N�g����Ղ̖ڂ̂悤�ɔz�u����
/// GridManager�̃X�e�[�g���ύX���邽�тɁAUI�̕\�����ύX����
/// </summary>
public class LineUpUI : MonoBehaviour
{
    [SerializeField, Tooltip("�z�u�������I�u�W�F�N�g")] GameObject _object = default;
    [Tooltip("�c�̌�")] int _height = 0;
    [Tooltip("���̌�")] int _width = 0;
    [SerializeField, Tooltip("�c�̊Ԋu")] float _heightInterval = 1.0f;
    [SerializeField, Tooltip("���̊Ԋu")] float _widthInterval = 1.0f;
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
        _width = GridManager.Instance.Width;
        _standardPos = transform.position;
        _gameObjectArray = new GameObject[_height, _width];

        _indexArray = new Text[_height, _width];
        _numArray = new Text[_height, _width];

        for (var i = 0; i < _height; i++)
        {
            for (var j = 0; j < _width; j++)
            {
                // ���g�̎q�I�u�W�F�N�g�ɂ���
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