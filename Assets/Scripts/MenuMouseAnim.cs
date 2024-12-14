using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuMouseAnim : MonoBehaviour
{
    [SerializeField] private float XWidthCoefficientDivide;
    [SerializeField] private float CursorDuration;
    private float XMoveTo;


    void Start()
    {
        XMoveTo = Screen.width / XWidthCoefficientDivide;
        transform.DOMoveX(XMoveTo, CursorDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
