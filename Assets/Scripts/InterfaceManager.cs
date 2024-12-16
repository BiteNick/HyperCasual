using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject RunUI;
    [SerializeField] private GameObject FinishUI;

    [SerializeField] private CharacterMove characterScript;

    [SerializeField] private GameObject _FinishUIPanel;

    [SerializeField] private TextMeshProUGUI RewardText;

    [SerializeField] private RouletteArrow rouletteArrow;

    public static int moneysMultiplier;

    private bool _isStarted = false;

    private void Awake()
    {
        characterScript.enabled = false;
    }

    void Update()
    {
        if (!_isStarted && Input.GetMouseButtonDown(0))
        {
            _isStarted = true;
            characterScript.enabled = true;
            characterScript.Init();
            MenuUI.SetActive(false);
            RunUI.SetActive(true);
        }
    }

    public void Finish()
    {
        RunUI.SetActive(false);
        FinishUI.SetActive(true);

        rouletteArrow.Init(moneysMultiplier * MoneysManager.currentMoneys);

        DG.Tweening.Sequence animation = DOTween.Sequence();
        animation.Append(_FinishUIPanel.transform.DOScale(1, 0.5f).From(0).SetEase(Ease.OutBounce));



        RewardText.text = (moneysMultiplier * MoneysManager.currentMoneys).ToString();

    }

}
