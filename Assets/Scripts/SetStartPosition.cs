using UnityEngine;
/// <summary>
/// �I�u�W�F�N�g���O���b�h�̂ǂ��������ʒu�ɂ��邩
/// bool�̐؂�ւ��Ŕz�u�ł���
/// </summary>
public class SetStartPosition : MonoBehaviour
{
    [SerializeField, Tooltip("�O���b�h��ێ����Ă���N���X")] LineUpObjects _lineUpObjects = default;
    [Header("�u�������ꏊ�̃C���f�b�N�X�ԍ��i�c���j")]
    [Tooltip("�u�������ꏊ�i�c�j")] int _depth = 0;
    [Tooltip("�u�������ꏊ�i���j")] int _wide = 0;
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

        // �����Ȃ��Ƃ���ɂ̂ݒu��
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