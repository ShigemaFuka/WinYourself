using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��{�W�����̓��̂P�����ֈړ�
/// �W�����Ƃ��ړ��ł��Ȃ���΁A���̏�ŗ��܂�
/// �ړ��͈͂͂O�`�B
/// </summary>
public class Move : MonoBehaviour
{
    [Tooltip("���݂̈ʒu  ��jd, w")] int[] _currentIndex = new int[2];
    [Tooltip("�ړ��ł���͈͂̃��X�g")] List<string> _mobileList = new();
    [Header("�ړ��ł���G���A��T�����������邩")]
    [SerializeField, Tooltip("�ړ��ł���G���A��T�����������邩")] bool _isPrepare = false;
    [Header("�ړ����邩")]
    [SerializeField, Tooltip("�ړ����邩")] bool _isMove = false;
    [Header("�ړ���")]
    [SerializeField, Tooltip("�ړ���")] int[] nums = new int[2];
    [SerializeField, Tooltip("���݂̃X�e�[�g")] State _currentState = State.None;

    #region �v���p�e�B
    public int[] CurrentIndex { get => _currentIndex; set => _currentIndex = value; }
    public bool IsPrepare { get => _isPrepare; set => _isPrepare = value; }
    public bool IsMove { get => _isMove; set => _isMove = value; }
    public State CurrentState { get => _currentState; set => _currentState = value; }
    #endregion

    public enum State
    {
        None,
        Prepare,
        Move
    }

    void Start()
    {
        CurrentState = State.None;
    }

    void Update()
    {
        if (CurrentState == State.Prepare)
            PreparationForMovement();

        if (CurrentState == State.Move)
            Movement();
    }

    /// <summary>
    /// _isMove���^�ɂȂ邽�тɌĂяo��
    /// �ړ�����
    /// </summary>
    void PreparationForMovement()
    {
        // ���񃊃X�g��������
        _mobileList.Clear();
        AddMobileArea();
        Determine();
        //CurrentState = State.Stay;
        CurrentState = State.Move;
    }

    /// <summary>
    /// �ړ��ł���G���A�����X�g�Ɋi�[
    /// </summary>
    void AddMobileArea()
    {
        var baseY = _currentIndex[0];
        var baseX = _currentIndex[1];

        //_depthMax = GridManager.Instance.Depth;
        //_widthMax = GridManager.Instance.Width;

        for (var i = baseY - 1; i < baseY + 2; i++)
        {
            for (var j = baseX - 1; j < baseX + 2; j++)
            {
                // ������Ƃ���͔�΂�
                if (i == baseY && j == baseX)
                    continue;
                // �󂫂ł͂Ȃ��Ƃ���͔�΂�
                else if (GridManager.Instance.CheckArray(i, j) != (int)GridManager.GridState.Empty)
                    continue;
                else
                {
                    _mobileList.Add($"{i} {j}");
                    GridManager.Instance.ChangeArray(GridManager.GridState.Option, i, j);
                }
            }
        }
    }

    /// <summary>
    /// �ړ����I��
    /// </summary>
    /// <returns></returns>
    string Select()
    {
        // �W�����Ƃ��ړ��ł��Ȃ���΁A���̏�őҋ@
        if (_mobileList.Count == 0)
            return $"{CurrentIndex[0]} {CurrentIndex[1]}";
        else
            // ���X�g����1�I��ňړ���Ƃ���
            return _mobileList[UnityEngine.Random.Range(0, _mobileList.Count)];
    }

    /// <summary>
    /// �ړ���̏����i�[
    /// </summary>
    void Determine()
    {
        nums = Array.ConvertAll(Select().Split(), int.Parse);
    }

    void Movement()
    {
        foreach (var item in _mobileList)
        {
            // �ړ��ł���G���A��-1�ŕ\���������Ƃ́A���g���ړ��ł���悤�ɃG���A����0�i�󂫁j�ɂ���
            var nums = Array.ConvertAll(item.Split(), int.Parse);
            GridManager.Instance.ChangeArray(GridManager.GridState.Empty, nums[0], nums[1]);
        }

        // �ړ��ƃX�e�[�g���X�V

        transform.parent = GridManager.Instance.GameObjectArray[nums[0], nums[1]].transform;

        var pos = GridManager.Instance.UseVector(nums[0], nums[1]);
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        GridManager.Instance.ChangeArray(GridManager.GridState.Exist, nums[0], nums[1]);

        // �������ꏊ���󂫂ɂ��� ���̏�ҋ@�̂Ƃ��͋󂫂ɂ��Ȃ�
        if (_mobileList.Count != 0)
            GridManager.Instance.ChangeArray(GridManager.GridState.Empty, CurrentIndex[0], CurrentIndex[1]);

        // ������ꏊ���X�V
        CurrentIndex[0] = nums[0];
        CurrentIndex[1] = nums[1];
        //Debug.Log($"CurrentIndex : {CurrentIndex[0]} {CurrentIndex[1]}");

        //_isMove = false;
        CurrentState = State.None;
    }
}