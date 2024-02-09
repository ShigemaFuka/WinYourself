using UnityEngine;
/// <summary>
/// �Q�[���̏�ԊǗ�������
/// NPC�̃^�[���ƁA�n���}�[�̃^�[�����Ǘ�
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("�C���X�^���X���擾���邽�߂̃p�u���b�N�ϐ�")] public static GameManager Instance = default;
    [SerializeField, Tooltip("���݂̃Q�[���X�e�[�g")] GameState _currentState = GameState.InGame;

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

    /// <summary> �����ƃn���}�[�����̏�Ԃ��Ǘ�����񋓌^ </summary>
    public enum TurnState
    {
        /// <summary> �����F�����鑤 </summary>
        Egg,
        /// <summary> �n���}�[���F�@���� </summary>
        Hammer
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
