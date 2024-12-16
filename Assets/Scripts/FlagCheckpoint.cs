using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlagCheckpoint : MonoBehaviour
{
    [SerializeField] private List<Transform> FlagsTransform;

    [SerializeField] private AudioClip clip;


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CharacterMove>().CheckpointComplete(clip);

        foreach (Transform flagTransform in FlagsTransform)
        {
            flagTransform.DOLocalRotate(new Vector3(flagTransform.localEulerAngles.x, flagTransform.localEulerAngles.y, 0), 0.5f);
        }
    }
}
