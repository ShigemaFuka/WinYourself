using UnityEngine;
/// <summary>
/// �����鑤�ƂȂ�I�u�W�F�N�g���O���b�h�̂ǂ��������ʒu�ɂ��邩
/// bool���^�Ƃ��z�u�ł���
/// </summary>
public class SetStartPosition : MonoBehaviour
{
    [SerializeField, Tooltip("�O���b�h�𐬂����т̃I�u�W�F�N�g�́A�z���ێ����Ă���N���X")] LineUpObjects _lineUpObjects = default;
    [Header("�u�������ꏊ�̃C���f�b�N�X�ԍ��i�c���j")]
    [Tooltip("�u�������ꏊ�i�c�j")] int _depth = 0;
    [Tooltip("�u�������ꏊ�i���j")] int _width = 0;
    [Header("�u�������ꏊ�i�����j")]
    [SerializeField, Tooltip("�u�������ꏊ�i�����j")] float _yPos = 0;
    [Header("�u�������I�u�W�F�N�g")]
    [SerializeField, Tooltip("�u�������I�u�W�F�N�g")] GameObject _gameObject = default;
    [Header("�I�u�W�F�N�g�̐�")]
    [SerializeField, Tooltip("�I�u�W�F�N�g�̐�")] int _amount = 2;
    [Header("�z�u���邩")]
    [SerializeField, Tooltip("�z�u���邩")] bool _isSet = false;
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
    /// �ʒu����
    /// </summary>
    void SetPosition()
    {
        var array = _lineUpObjects.GameObjectArray;

        // �O���b�h�����z�u�̂Ƃ�
        if (array.Length == 0)
        {
            Debug.LogError("�O���b�h�����z�u");
            return;
        }

        string message = "�󂪂���܂���";
        for (var i = 0; i < array.Length; i++)
        {
            _depth = Random.Range(0, array.GetLength(0));
            _width = Random.Range(0, array.GetLength(1));
            // ��̂Ƃ��낾������ʒu��ύX���A���[�v�𔲂���
            if (GridManager.Instance.CheckArray(_depth, _width) == (int)GridManager.GridState.Empty)
            {
                _position = array[_depth, _width].transform.position;
                message = $"{_depth} {_width}";
                break;
            }
        }
        // �󂫂��Ȃ��Ƃ������o�͂ŗǂ�
        if (message == "�󂪂���܂���")
            Debug.LogWarning(message);

        GridManager.Instance.ChangeArray(GridManager.GridState.Exist, _depth, _width);
        var go = Instantiate(_gameObject, transform);
        go.transform.position = new Vector3(_position.x, _yPos, _position.z);
        var move = go.GetComponent<Move>();

        move.CurrentIndex[0] = _depth;
        move.CurrentIndex[1] = _width;
    }
}