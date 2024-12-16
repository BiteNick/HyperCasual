using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private int moneysForPick; //can also be a negative value
    [SerializeField] private GameObject pickupParticle; //creates a particle when picked up
    [SerializeField] private Vector3 offsetParticleRotation;

    [SerializeField] private Transform transformToRotate;

    [SerializeField] private AudioClip[] clips;

    void Update()
    {
        if (transformToRotate != null)
        {
            transformToRotate.Rotate(0, _rotateSpeed * Time.deltaTime, 0);
        }
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (pickupParticle != null)
            Destroy(Instantiate(pickupParticle, transform.position, Quaternion.Euler(offsetParticleRotation)), 2f);
        
        other.GetComponent<CharacterMove>().PickUp(moneysForPick, clips);
        TriggerEnterEnd();
    }

    protected virtual void TriggerEnterEnd()
    {
        Destroy(gameObject);
    }
}
