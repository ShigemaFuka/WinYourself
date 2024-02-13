using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自身が何番目かをUIで表示
/// これがアタッチされたオブジェクトをリストから探し出し、リストの何番目かを見る
/// </summary>
public class ShowSelfNumber : MonoBehaviour
{
    [SerializeField, Tooltip("リストを持つオブジェクト")]
    RunawayController _runawayController = default;

    [SerializeField, Tooltip("自身が何番手か")] int _num = 0;
    [SerializeField, Tooltip("表示するテキスト")] Text _text = default;
    [Tooltip("リスト")] List<GameObject> _list = default;
    
    void OnEnable()
    {
        _runawayController = FindAnyObjectByType<RunawayController>();
        _list = _runawayController.MoveComponents;
        _num = _list.FindIndex(item => item == transform.gameObject);
        _text.text = (_num + 1).ToString();
    }

    void Update()
    {
        // リストに変更があったら
        if (_list != _runawayController.MoveComponents)
        {
            _list = _runawayController.MoveComponents;
            _num = _list.FindIndex(item => item == transform.gameObject);
            _text.text = (_num + 1).ToString();
            Debug.Log("ShowSelfNumber");
        }
    }
}