using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���g�����Ԗڂ���UI�ŕ\��
/// </summary>
public class ShowSelfNumber : MonoBehaviour
{
    [SerializeField, Tooltip("���g�����Ԏ肩")] int num = 0;
    [SerializeField, Tooltip("�\������e�L�X�g")] Text _text = default;

    void Start()
    {
        num = transform.GetSiblingIndex();
        _text.text = (num + 1).ToString();
    }
}
