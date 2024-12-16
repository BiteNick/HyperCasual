using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRotation : MonoBehaviour
{
    [SerializeField] private float RotateDegrees;
    [SerializeField] private Transform parentTransform;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CharacterMove>().ChangeRotation(RotateDegrees, parentTransform.position);
    }

}
