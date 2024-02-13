using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 第二フェーズになったら非アクティブになっていたオブジェクトを全てアクティブにする。
/// </summary>
public class SetActiveObjects : MonoBehaviour
{
    [SerializeField, Tooltip("逃げる側のオブジェクトの配列")]
    List<GameObject> _runawayArray = default;

    [Tooltip("逃げる側のオブジェクトの配列")] GameManager _gameManager = default;

    [Header("アクティブ化"), SerializeField] [Tooltip("アクティブ化")]
    bool _isSetActiveRunaway = default;

    #region プロパティ

    public List<GameObject> RunawayArray
    {
        get => _runawayArray;
        //set => _runawayArray = value;
    }

    #endregion

    void Start()
    {
        _gameManager = GameManager.Instance;
        _runawayArray = GridManager.Instance.RunawayList;
    }

    void Update()
    {
        if (_gameManager.NowPhaseState == GameManager.PhaseState.FirstPhase)
        {
            return;
        }

        // 切り替わった瞬間だけ
        // if (_gameManager.NowPhaseState == GameManager.PhaseState.SecondPhase
        //     && _gameManager.OldPhaseState == GameManager.PhaseState.FirstPhase)
        // {
        //     _isSetActiveRunaway = true;
        // }

        if (_isSetActiveRunaway)
        {
            ResetIntArray();
            SetActiveTarget();
            _isSetActiveRunaway = false;
            _gameManager.ChangeNowProcessState(GameManager.ProcessState.ListUpdate);
        }
    }

    /// <summary>
    /// 逃げる側のオブジェクトを全てアクティブにする
    /// </summary>
    void SetActiveTarget()
    {
        for (var i = 0; i < _runawayArray.Count; i++)
        {
            _runawayArray[i].SetActive(true);
        }
    }

    void ResetIntArray()
    {
        var intArray = GridManager.Instance.IntArray;
        for (var i = 0; i < GridManager.Instance.Depth; i++)
        {
            for (var j = 0; j < GridManager.Instance.Width; j++)
            {
                GridManager.Instance.ChangeArray(GridManager.GridState.Empty, i, j);
            }
        }
    }

    public void OnClick(bool flag)
    {
        _isSetActiveRunaway = flag;
        Debug.LogWarning("_isSetActiveRunaway");
    }
}