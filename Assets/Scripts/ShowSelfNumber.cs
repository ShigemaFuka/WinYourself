using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自身が何番目かをUIで表示
/// </summary>
public class ShowSelfNumber : MonoBehaviour
{
    [SerializeField, Tooltip("自身が何番手か")] int num = 0;
    [SerializeField, Tooltip("表示するテキスト")] Text _text = default;

    void Start()
    {
        num = transform.GetSiblingIndex();
        _text.text = (num + 1).ToString();
    }
}
