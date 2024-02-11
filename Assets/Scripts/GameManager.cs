using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// �Q�[���̏�ԊǗ�������
/// NPC�̃^�[���ƁA�n���}�[�̃^�[�����Ǘ�
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("�C���X�^���X���擾���邽�߂̃p�u���b�N�ϐ�")] public static GameManager Instance = default;

    [Header("���݂̃Q�[���X�e�[�g")]
    [SerializeField, Tooltip("���݂̃Q�[���X�e�[�g")] GameState _gameState = GameState.InGame;
    [Header("���݂̃V�[���X�e�[�g")]
    [SerializeField, Tooltip("���݂̃V�[���X�e�[�g")] SceneState _sceneState = SceneState.InGame;
    [Header("���݂̃v���Z�X�X�e�[�g")]
    [SerializeField, Tooltip("���݂̃v���Z�X�X�e�[�g")] ProcessState _nowProcessState = ProcessState.None;
    [Header("�P�t���[���O�̃v���Z�X�X�e�[�g")]
    [SerializeField, Tooltip("�P�t���[���O�̃v���Z�X�X�e�[�g")] ProcessState _oldProcessState = ProcessState.None;

    [Header("���݂̃t�F�[�Y�i�C���Q�[�����j")]
    [SerializeField, Tooltip("���݂̃t�F�[�Y�i�C���Q�[�����j")] PhaseState _nowPhaseState = PhaseState.FistPhase;

    [Header("���^�[������")]
    [SerializeField, Tooltip("���^�[������")] int _totalTurn = default;
    [Header("���^�[���o�߂�����")]
    [SerializeField, Tooltip("���^�[���o�߂�����")] int _turnCount = default;

    [Header("�I�������ӏ��̋L�^")]
    [SerializeField, Tooltip("�I�������ӏ��̋L�^")]
    StrikePoint _strikePoint = default;

    [Header("���X�g�i�I�������ӏ��̋L�^�j")]
    [SerializeField, Tooltip("���X�g�i�I�������ӏ��̋L�^�j")] List<StrikePoint> _strikePointList = default;

    #region �v���p�e�B
    /// <summary> ���݂̃v���Z�X�X�e�[�g </summary>
    public ProcessState NowProcessState { get => _nowProcessState; }

    /// <summary> �P�t���[���O�̃v���Z�X�X�e�[�g </summary>
    public ProcessState OldProcessState { get => _oldProcessState; }

    /// <summary> �P�t���[���O�̃v���Z�X�X�e�[�g </summary>
    public PhaseState NowPhaseState { get => _nowPhaseState; }

    /// <summary> ���^�[������ </summary>
    public int TotalTurn { get => _totalTurn; }

    /// <summary> ���^�[���o�߂����� </summary>
    public int TurnCount { get => _turnCount; }
    #endregion

    /// <summary>
    /// GameManager�̃C���X�^���X���i�[
    /// </summary>
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    /// <summary> �Q�[���̏�Ԃ��Ǘ�����񋓌^ </summary>
    public enum GameState
    {
        Start,
        InGame,
        Pause,
        GameOver,
        Result,
    }

    /// <summary> �V�[���̏�Ԃ��Ǘ�����񋓌^ </summary>
    public enum SceneState
    {
        Start,
        InGame,
        //GameOver,
        Result,
    }

    /// <summary> �i�����ƃn���}�[�����́j��Ԃ��Ǘ�����񋓌^ </summary>
    public enum ProcessState
    {
        None,

        /// <summary> �����ʒu�ݒ� </summary>
        SetStartPosition,

        /// <summary> �����鑤���o�� </summary>
        ShowRunaway,

        /// <summary> �X�V </summary>
        Update,
        // ���炭�A�X�e�[�g�̐؂�ւ����������āA�v���������ɂȂ��Ă��Ȃ�
        // ���̑΍�Ƃ��āA�蓮��RunawayController��_isGetComponents��^�ɂ��Ă邩��A�v��Ȃ�����

        /// <summary> �@���ʒu�L�^ </summary>
        Record,

        /// <summary> �����鑤���s�� </summary>
        MoveRunaway,

        /// <summary> �@���邩���� </summary>
        Check,

        /// <summary> �����鑤���������or�Ƃ�� </summary>
        DeadOrLive
    }

    /// <summary> �C���Q�[���̃t�F�[�Y�̏�Ԃ��Ǘ�����񋓌^ </summary>
    public enum PhaseState
    {
        [Tooltip("�v���C���[���@����")]
        FistPhase,
        [Tooltip("�v���C���[�������鑤")]
        SecondPhase,
    }

    void Start()
    {

    }

    void Update()
    {
        _oldProcessState = _nowProcessState;

        if (TurnCount >= TotalTurn)
        {
            _nowPhaseState = PhaseState.SecondPhase;
        }
    }

    /// <summary>
    /// ���݂̃v���Z�X�X�e�[�g�ύX
    /// </summary>
    /// <param name="state">�ς������X�e�[�g</param>
    public void ChangeNowProcessState(ProcessState state)
    {
        _nowProcessState = state;
    }

    /// <summary>
    /// �^�[�������Z
    /// </summary>
    /// <param name="value"></param>
    public void AddTurnCount(int value)
    {
        _turnCount += value;
    }

    /// <summary>
    /// �@�����i�I�������j�ӏ����L�^����
    /// </summary>
    public void RecordStrikePoint(int depth, int width)
    {
        _strikePoint.depth = depth;
        _strikePoint.width = width;
        _strikePointList.Add(_strikePoint);
    }

    [System.Serializable]
    struct StrikePoint // �\���̂̒�`
    {
        public int depth;
        public int width;
    }
}