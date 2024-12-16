using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsDoors : Pickups
{
    [SerializeField] private GameObject _secondDoor;

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        OnTriggerEnd();
    }

    protected void OnTriggerEnd()
    {
        Destroy(_secondDoor);
        Destroy(gameObject);
    }
}
