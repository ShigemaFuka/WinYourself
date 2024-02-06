using UnityEngine;
/// <summary>
/// ����̃I�u�W�F�N�g����Ղ̖ڂ̂悤�ɔz�u����
/// </summary>
public class LineUpObjects : MonoBehaviour
{
    [SerializeField, Tooltip("�z�u�������I�u�W�F�N�g")] GameObject _object = default;
    [Tooltip("�c�̌�")] int _depth = 0;
    [Tooltip("���̌�")] int _wide = 0;
    [SerializeField, Tooltip("�c�̊Ԋu")] float _depthInterval = 1.0f;
    [SerializeField, Tooltip("���̊Ԋu")] float _wideInterval = 1.0f;
    [Tooltip("�O���b�h�𐬂��Q�����z��")] GameObject[,] _gameObjectArray = default;
    [Tooltip("��ƂȂ�ʒu")] Vector3 _standardPos = default;

    #region �v���p�e�B
    public GameObject[,] GameObjectArray { get => _gameObjectArray; }
    #endregion

    void Start()
    {
        _depth = GridManager.Instance.Depth;
        _wide = GridManager.Instance.Wide;
        // ������A�^�b�`����I�u�W�F�N�g�̈ʒu��(0,0,0)�ɁB
        _standardPos = transform.position;
        _gameObjectArray = new GameObject[_depth, _wide];

        for (var i = 0; i < _depth; i++)
        {
            for (var j = 0; j < _wide; j++)
            {
                // ���g�̎q�I�u�W�F�N�g�ɂ���
                var go = Instantiate(_object, transform);
                if (go)
                {
                    float xPos = j * _wideInterval;
                    float zPos = -i * _depthInterval;
                    go.transform.position = _standardPos + new Vector3(xPos, 0f, zPos);
                    _gameObjectArray[i, j] = go;
                }
            }
        }
    }

    void Update()
    {

    }
}
