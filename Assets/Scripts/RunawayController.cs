using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
/// <summary>
/// �����鑤
/// �q�I�u�W�F�N�g�ƂȂ�Move�N���X�����I�u�W�F�N�g���A�ォ�珇�ɍs��������e
/// �e�q�I�u�W�F�N�g��bool��^�ɂ��A���ׂĂ̎q�I�u�W�F�N�g�ɂ��̏������I�������ANPC�̍s���^�[�����I���������Ƃ�m�点��
/// </summary>
public class RunawayController : MonoBehaviour
{
    [Header("�q�I�u�W�F�N�g�̃R���|�[�l���g���擾���邩")]
    [SerializeField, Tooltip("�q�I�u�W�F�N�g�̃R���|�[�l���g���擾���邩")] bool _isGetComponents = false;
    [Header("�����鑤�̃^�[����")]
    [SerializeField, Tooltip("�����鑤�̃^�[����")] bool _isRunawayTurn = false;
    [Tooltip("�ҋ@����")] WaitForSeconds _wfs = default;
    [Header("��������ړ��܂ł̑ҋ@����")]
    [SerializeField, Tooltip("��������ړ��܂ł̑ҋ@����")] float _waitTime = 1.5f;
    [Header("���ׂĂ̎q�I�u�W�F�N�g")]
    [SerializeField] List<GameObject> _moveComponents = default;

    #region �v���p�e�B
    /// <summary> �O���b�h�𐬂��Q�[���I�u�W�F�N�g </summary>
    public List<GameObject> MoveComponents { get => _moveComponents; /*set => _moveComponents = value;*/ }
    #endregion


    void Start()
    {
        _wfs = new WaitForSeconds(_waitTime);
        _moveComponents = new List<GameObject>();
    }

    void Update()
    {
        if (_isGetComponents)
        {
            _moveComponents.Clear();

            _moveComponents = GameObject.FindGameObjectsWithTag("Runaway").ToList();
            Debug.Log("clear");
            _isGetComponents = false;
            GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.Record);
        }

        // �؂�ւ�����u��
        if (GameManager.Instance.NowProcessState == GameManager.ProcessState.MoveRunaway
            && GameManager.Instance.OldProcessState == GameManager.ProcessState.Record)
        {
            _isRunawayTurn = true;
            Debug.Log("_isRunawayTurn");
        }

        if (_isRunawayTurn)
        {
            StartCoroutine(PrepareAndMove());
            _isRunawayTurn = false;
        }
    }

    IEnumerator PrepareAndMove()
    {
        for (var i = 0; i < _moveComponents.Count; i++)
        {
            var move = _moveComponents[i].GetComponent<Move>();
            move.CurrentState = Move.State.Prepare;

            Debug.Log($"i : {i}    ���� : {_moveComponents.Count}");
            yield return _wfs;
        }
        GameManager.Instance.ChangeNowProcessState(GameManager.ProcessState.Check);
        Debug.Log($"check");
    }
}
