using OpenCover.Framework.Model;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �N���b�N�����ʒu�ɓ�����҂�������A�@��
/// ���Ȃ���Ή����ł��Ȃ�
/// </summary>
public class Strike : MonoBehaviour
{
    [Header("�Ώۂ̏c���̃C���f�b�N�X�ԍ�")]
    [SerializeField, Tooltip("�Ώۂ̏c�̃C���f�b�N�X�ԍ�")] int _depth = default;
    [SerializeField, Tooltip("�Ώۂ̉��̃C���f�b�N�X�ԍ�")] int _width = default;
    [Header("�J����")]
    [SerializeField] Camera _cameraObject = default;
    RaycastHit _hit = default;
    [Header("�@�����̃^�[����")]
    [SerializeField, Tooltip("�@�����̃^�[����")] bool _isStrikeTurn = false;
    [Header("�@���ꏊ�����肵����")]
    [SerializeField, Tooltip("�@���ꏊ�����肵����")] bool _isDecision = false;
    //[Header("�O���b�h�𐬂����т̃I�u�W�F�N�g�́A�z���ێ����Ă���N���X")]
    //[SerializeField, Tooltip("�O���b�h�𐬂����т̃I�u�W�F�N�g�́A�z���ێ����Ă���N���X")] LineUpObjects _lineUpObjects = default;

    //public delegate void OnCompleteDelegate();
    //public OnCompleteDelegate onComplete;

    void Start()
    {
        _isStrikeTurn = false;
        _isDecision = false;
    }

    void Update()
    {
        // ��Q�t�F�[�Y�ł͎g��Ȃ�
        if (GameManager.Instance.NowPhaseState == GameManager.PhaseState.SecondPhase)
        {
            return;
        }

        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.Record)
        {
            _isStrikeTurn = true;
        }

        if (_isStrikeTurn)
        {
            Click();
        }

        // ui�������ŁA���肵�܂����H�ƕ���
        if (_isDecision)
        {
            _isStrikeTurn = false;
            //GameManager.Instance.NowTurnState = GameManager.TurnState.MoveRunaway;

            //onComplete = MyMethod;
            //// �R�[���o�b�N
            //onComplete();

            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.MoveRunaway);
            _isDecision = false;
        }

        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.Check)
        {
            Debug.Log(Check(_depth, _width));
            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.DeadOrLive);
        }

        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.DeadOrLive)
        {
            StrikeTarget();
            Debug.Log("StrikeTarget");
            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.Update);
        }
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
                    return;
                }
            }
        }
    }

    /// <summary>
    /// �����鑤���������ǂ���
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="width"></param>
    /// <returns> �^�F������A�@�U�F�O�� </returns>
    bool Check(int depth, int width)
    {
        if (GridManager.Instance.IntArray[depth, width] == (int)GridManager.GridState.Exist)
        {
            Debug.Log("������");
            return true;
        }
        else
        {
            Debug.Log("�O��");
            return false;
        }
    }

    /// <summary>
    /// destroy����
    /// </summary>
    void StrikeTarget()
    {
        var gameManager = GameManager.Instance;
        gameManager.RecordStrikePoint(_depth, _width);

        var gridManager = GridManager.Instance;
        var array = gridManager.GameObjectArray;
        if (array[_depth, _width].transform.childCount != 0)
        {
            //Destroy(array[_depth, _width].transform.GetChild(0).gameObject);
            // FindGameObjectsWithTag�͔�A�N�e�B�u�ȃI�u�W�F�N�g��T���Ȃ��@

            // ���@��A�N�e�B�u�ɂ��邾���ŁA���Ƃ̓��X�g���X�V���邾���ł���
            for (var i = 0; i < array[_depth, _width].transform.childCount; i++)
            {
                array[_depth, _width].transform.GetChild(i).gameObject.SetActive(false);
                // ��\���ɂ��Ă��邾���ŁA�q�I�u�W�F�N�g�܂܂ł��邽��
            }

            // ��Ԃ��X�V
            gridManager.ChangeArray(GridManager.GridState.Empty, _depth, _width);
        }
        gameManager.AddTurnCount(1);
    }
}