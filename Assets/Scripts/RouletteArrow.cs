using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class RouletteArrow : MonoBehaviour
{
    [SerializeField] private List<Vector2> _rotationsZ; //angles for indicate current index
    [SerializeField] private List<int> _moneyModifiers; //must be same length with _positionsZ
    [SerializeField] private int _currentIndex;

    [SerializeField] private TextMeshProUGUI _moneysWithModifierText;

    private int _currentMoneys;

    void Start()
    {
        if (_rotationsZ.Count != _moneyModifiers.Count)
        {
            Debug.Log("<color=red> moneyModifiers and positionsZ need same Length!!!</color>");
        }

        transform.DOLocalRotate(new Vector3(0, 0, 180), 1.5f).From(new Vector3(0,0,0)).SetEase(Ease.InExpo).SetLoops(-1, LoopType.Yoyo);
    }

    public void Init(int moneys)
    {
        _currentMoneys = moneys;
    }


    void Update()
    {

        for (int i = 0; i < _rotationsZ.Count; i++)
        {
            if (transform.localEulerAngles.z >= _rotationsZ[i].x && transform.localEulerAngles.z < _rotationsZ[i].y)
            {
                _currentIndex = i;
                break;
            }
        }

        _moneysWithModifierText.text = (_currentMoneys * _moneyModifiers[_currentIndex]).ToString();
    }
}
