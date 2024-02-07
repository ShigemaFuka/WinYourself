using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Tooltip("�C���X�^���X���擾���邽�߂̃p�u���b�N�ϐ�")] public static GridManager Instance = default;
    [SerializeField, Tooltip("�c�̌�")] int _depth = 4;
    [SerializeField, Tooltip("���̌�")] int _width = 4;
    [Tooltip("�e�O���b�h�̏�Ԃ�int�Ŋi�[�����z��")] static int[,] _intArray = default;
    [Tooltip("�e�O���b�h�̈ʒu�����i�[�����z��")] static Vector3[,] _vector3Array = default;
    //[Tooltip("�O���b�h�𐬂��Q�����z��")] static GameObject[,] _gameObjectArray = default;

    #region �v���p�e�B
    public int Depth { get => _depth; }
    public int Width { get => _width; }
    public int[,] IntArray { get => _intArray; set => _intArray = value; }
    public Vector3[,] VectorArray { get => _vector3Array; set => _vector3Array = value; }
    //public GameObject[,] GameObjectArray { get => _gameObjectArray; set => _gameObjectArray = value; }

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
        /// <summary> �ړ���̌�� </summary>
        Option = -1,
        /// <summary> �� </summary>
        Empty,
        /// <summary> ���ɂ��� </summary>
        Exist
    }

    void Start()
    {
        IntArray = new int[Depth, Width];
        VectorArray = new Vector3[Depth, Width];
        //_gameObjectArray = new GameObject[Depth, Width];
    }

    void Update()
    {

    }

    /// <summary>
    /// -1 : �\��
    /// 0 : �����Ȃ�
    /// 1 : ���łɂ���
    /// -100 : out of range
    /// </summary>
    /// <param name="d"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    public int CheckArray(int d, int w)
    {
        if (d >= Depth || w >= Width || d < 0 || w < 0)
            return -100;
        return IntArray[d, w];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state">-1, 0, 1</param>
    /// <param name="d"></param>
    /// <param name="w"></param>
    public void ChangeArray(GridState state, int d, int w)
    {
        IntArray[d, w] = (int)state;
    }

    public void SetInitializeVector(GameObject[,] go, int i, int j)
    {
        VectorArray[i, j] = go[i, j].transform.position;
    }

    public Vector3 UseVector(int i, int j)
    {
        return VectorArray[i, j];
    }
}
