using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���g�����Ԗڂ���UI�ŕ\��
/// ���ꂪ�A�^�b�`���ꂽ�I�u�W�F�N�g�����X�g����T���o���A���X�g�̉��Ԗڂ�������
/// </summary>
public class ShowSelfNumber : MonoBehaviour
{
    [SerializeField, Tooltip("�e�Ƃ������")] GameObject _parent = default;
    [SerializeField, Tooltip("���X�g�����I�u�W�F�N�g")] RunawayController _runawayController = default;
    [SerializeField, Tooltip("���g�����Ԏ肩")] int num = 0;
    [SerializeField, Tooltip("�\������e�L�X�g")] Text _text = default;
    [Tooltip("���X�g")] List<GameObject> _list = new List<GameObject>();

    void Start()
    {
        _runawayController = FindAnyObjectByType<RunawayController>();
        _list = _runawayController.MoveComponents;
        num = _list.FindIndex(item => item == transform.gameObject);
        _text.text = (num + 1).ToString();
    }

    void Update()
    {
        // ���X�g�ɕύX����������
        if (_list != _runawayController.MoveComponents)
        {
            _list = _runawayController.MoveComponents;
            num = _list.FindIndex(item => item == transform.gameObject);
            _text.text = (num + 1).ToString();
            Debug.Log("ShowSelfNumber");
        }
    }
}
