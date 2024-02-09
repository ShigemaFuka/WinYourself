using System.Collections;
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
    //[SerializeField, Tooltip("�q�I�u�W�F�N�g�̐�")] int _childrenAmount = default;
    [Header("�����鑤�̃^�[����")]
    [SerializeField, Tooltip("�����鑤�̃^�[����")] bool _isRunawayTurn = false;
    [Tooltip("�ҋ@����")] WaitForSeconds _wfs = default;
    [Header("��������ړ��܂ł̑ҋ@����")]
    [SerializeField, Tooltip("��������ړ��܂ł̑ҋ@����")] float _waitTime = 1.5f;
    [Header("���ׂĂ̎q�I�u�W�F�N�g")]
    [SerializeField, Tooltip("���ׂĂ̎q�I�u�W�F�N�g")] Move[] _moveComponents = default;

    void Start()
    {
        _wfs = new WaitForSeconds(_waitTime);
    }

    void Update()
    {
        if (_isGetComponents)
        {
            // �q�I�u�W�F�N�g��S���擾
            //_childrenAmount = transform.childCount;
            _moveComponents = GetComponentsInChildren<Move>();
            _isGetComponents = false;
        }
        if (_isRunawayTurn)
        {
            StartCoroutine(PrepareAndMove());

            //for (var i = 0; i < _moveComponents.Length; i++)
            //{
            //    //    //StartCoroutine(PrepareAndMove(i));

            //    //    //_moveComponents[i].IsPrepare = true;
            //    //    //_moveComponents[i].IsMove = true;

            //    _moveComponents[i].CurrentState = Move.State.Prepare;

            //}
            _isRunawayTurn = false;
        }
    }

    IEnumerator PrepareAndMove()
    {
        for (var i = 0; i < _moveComponents.Length; i++)
        {
            _moveComponents[i].CurrentState = Move.State.Prepare;
            yield return _wfs;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state">�ς������X�e�[�g</param>
    void Runaway(Move.State state)
    {
        for (var i = 0; i < _moveComponents.Length; i++)
        {
            _moveComponents[i].CurrentState = state;
        }
        Debug.Log(state);
    }
}
