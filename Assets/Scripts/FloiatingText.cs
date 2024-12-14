using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloiatingText : MonoBehaviour
{
    [SerializeField] private Color _positiveColor;
    [SerializeField] private Color _negativeColor;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private float _waitingTime; //time to hide text
    private float _waitingTimeLeft;
    private int sum = 0;

    [SerializeField] private Vector3 offset;

    void Start()
    {
        _waitingTimeLeft = _waitingTime;
    }

    public void Init(int moneys, Vector3 position) //calls every time when adds moneys, can also be a negative value
    {
        if (_text.text == "0")
        {
            transform.position = position + offset;
        }
        sum += moneys;
        _waitingTimeLeft = _waitingTime;
        if (sum > 0)
        {
            _text.color = _positiveColor;
            _text.text = $"+{sum}$";
        }
        else
        {
            _text.color = _negativeColor;
            _text.text = $"{sum}$";
        }

    }

    private void Update()
    {
        _waitingTimeLeft -= Time.deltaTime;
        if ( _waitingTimeLeft < 0)
        {
            _text.text = "0";
            sum = 0;
            gameObject.SetActive(false);
        }
    }
}
