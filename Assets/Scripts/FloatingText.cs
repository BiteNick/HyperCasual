using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private Color positiveColor;
    [SerializeField] private Color negativeColor;

    [SerializeField] private Vector3 positivePosition;
    [SerializeField] private Vector3 negativePosition;

    [SerializeField] private TextMeshProUGUI moneysText;

    private int _tempSum;
    [SerializeField] private float _timeToLoseSum;
    [SerializeField] private float _timeToLoseSumLeft;

    [SerializeField] private float flyOffsetY;

    private void Start()
    {
        _timeToLoseSumLeft = _timeToLoseSum;
    }

    public void FloatingTextCall(int moneys)
    {
        moneysText.DOFade(1, 0.6f).SetEase(Ease.InExpo);
        moneysText.DOKill();

        transform.DOScale(1f, 0.1f).From(0).SetEase(Ease.OutBounce);


        if (moneys > 0)
        {
            _tempSum += moneys;
            moneysText.color = positiveColor;
            transform.localPosition = positivePosition;
            moneysText.text = $"+{moneys}$";
        }
        else
        {
            _tempSum = moneys;
            moneysText.color = negativeColor;
            transform.localPosition = negativePosition;
            moneysText.text = $"{moneys}$";
        }
        transform.DOMoveY(transform.position.y + flyOffsetY, 1f);

        moneysText.DOFade(0, 1f);
    }

    private void Update()
    {
        _timeToLoseSumLeft -= Time.deltaTime;
        if (_timeToLoseSumLeft < 0)
        {
            _tempSum = 0;
        }
    }
}
