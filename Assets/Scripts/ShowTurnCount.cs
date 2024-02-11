using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ���^�[���o�߂�������UI�ŕ\��
/// �^�[�������X�V���ꂽ�Ƃ������A�\�����e��ύX����
/// </summary>
public class ShowTurnCount : MonoBehaviour
{
    [Header("�e�L�X�g")]
    [SerializeField, Tooltip("�e�L�X�g")] Text _text = default;
    [SerializeField, Tooltip("�Â��l�̃J�E���g")] int _oldTurnCount = 0;
    [SerializeField, Tooltip("�X�V���ꂽ�l�̃J�E���g")] int _turnCount = 0;

    void Start()
    {
        _oldTurnCount = 0;
        _turnCount = 0;
    }

    void Update()
    {
        _turnCount = GameManager.Instance.TurnCount;
        if (_oldTurnCount != _turnCount)
        {
            _oldTurnCount = _turnCount;
            _text.text = _oldTurnCount.ToString("00");
        }
    }
}
