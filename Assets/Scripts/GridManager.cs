using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Tooltip("�C���X�^���X���擾���邽�߂̃p�u���b�N�ϐ�")] public static GridManager Instance = default;
    [SerializeField, Tooltip("�c�̌�")] int _depth = 4;
    [SerializeField, Tooltip("���̌�")] int _wide = 4;
    static int[,] _array = default;

    #region �v���p�e�B
    public int Depth { get => _depth; }
    public int Wide { get => _wide; }
    public int[,] Array { get => _array; set => _array = value; }

    #endregion

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public enum GridState
    {
        /// <summary> �\�� </summary>
        Reservation = -1,
        /// <summary> �� </summary>
        Empty,
        /// <summary> ���ɂ��� </summary>
        Exist
    }

    void Start()
    {
        _array = new int[Depth, Wide];
    }

    void Update()
    {

    }

    /// <summary>
    /// -1 : �\��
    /// 0 : �����Ȃ�
    /// 1 : ���łɂ���
    /// </summary>
    /// <param name="d"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    public int CheckArray(int d, int w)
    {
        return Array[d, w];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state">-1, 0, 1</param>
    /// <param name="d"></param>
    /// <param name="w"></param>
    public void ChangeArray(GridState state, int d, int w)
    {
        Array[d, w] = (int)state;
    }
}
