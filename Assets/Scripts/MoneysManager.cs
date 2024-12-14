using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneysManager : MonoBehaviour
{
    public static int moneys;
    public static int currentMoneys; //moneys in level

    [SerializeField] private TextMeshProUGUI moneysText;
    private static TextMeshProUGUI _moneysText;


    private void Start()
    {
        _moneysText = moneysText;
    }


    public static void PickUpMoney(int moneysForPick) //can also be a negative value
    {
        currentMoneys += moneysForPick;

        if (currentMoneys < 0)
            currentMoneys = 0;

        UpdateText();
    }

    private static void UpdateText()
    {
        _moneysText.text = currentMoneys.ToString();
    }
}
