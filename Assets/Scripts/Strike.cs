using UnityEngine;

/// <summary>
/// �N���b�N�����ʒu�ɓ�����҂�������A�@��
/// ���Ȃ���Ή����ł��Ȃ�
/// </summary>
public class Strike : MonoBehaviour
{
    [Header("�c���̃C���f�b�N�X�ԍ�")]
    [SerializeField, Tooltip("�c�̃C���f�b�N�X�ԍ�")] int _depth = default;
    [SerializeField, Tooltip("���̃C���f�b�N�X�ԍ�")] int _width = default;
    [Header("�J����")]
    [SerializeField] Camera _cameraObject = default;
    RaycastHit _hit = default;
    [Header("�@�����̃^�[����")]
    [SerializeField, Tooltip("�@�����̃^�[����")] bool _isStrikeTurn = false;
    [Header("�@���ꏊ�����肵����")]
    [SerializeField, Tooltip("�@���ꏊ�����肵����")] bool _isDecision = false;

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
    /// �}�E�X���N���b�N�Ń��C���΂��āAStand�^�O��������ƂȂ�I�u�W�F�N�g������
    /// </summary>
    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //�}�E�X�̃|�W�V�������擾����Ray�ɑ��
            Ray ray = _cameraObject.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out _hit))
            {
                // �y��ł��邩�ǂ���
                if (_hit.collider.gameObject.CompareTag("Stand"))
                {
                    RecordSelection();
                }
            }
        }
    }

    /// <summary>
    /// �I�������ꏊ�̃C���f�b�N�X�ԍ����L�^
    /// </summary>
    void RecordSelection()
    {
        // ���������I�u�W�F�N�g�̈ʒu��񂩂�A�C���f�b�N�X�ԍ���T��
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
