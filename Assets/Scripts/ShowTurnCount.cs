using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 何ターン経過したかをUIで表示
/// ターン数が更新されたときだけ、表示内容を変更する
/// </summary>
public class ShowTurnCount : MonoBehaviour
{
    [Header("テキスト")]
    [SerializeField, Tooltip("テキスト")] Text _text = default;
    [SerializeField, Tooltip("古い値のカウント")] int _oldTurnCount = 0;
    [SerializeField, Tooltip("更新された値のカウント")] int _turnCount = 0;

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
