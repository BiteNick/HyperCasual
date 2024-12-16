using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private int moneysCoefficient;
    [SerializeField, Range(0, 32)] private int StatusIndexForOpen; // 0 - pooriest, 32 - richiest

    [SerializeField] private GameObject Door1;
    [SerializeField] private GameObject Door2;

    [SerializeField] private AudioClip clip;

    private static int doorsCount;

    private void Start()
    {
        doorsCount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterMove character = other.GetComponent<CharacterMove>();

        if (MoneysManager.currentMoneys >= character.MaximumMoneysOnLevel / doorsCount * StatusIndexForOpen)
        {
            Door1.transform.DOLocalRotate(new Vector3(0, 90, 0), 1f);
            Door2.transform.DOLocalRotate(new Vector3(0, -90, 0), 1f);
            InterfaceManager.moneysMultiplier = moneysCoefficient;
            character.DoorOpened(clip);
        }
        else
        {
            character.Finish();
        }
    }
}
