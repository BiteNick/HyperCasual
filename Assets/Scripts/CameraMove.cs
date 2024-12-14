using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerParent;

    void LateUpdate()
    {
        transform.position = _player.position;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _playerParent.eulerAngles.y, transform.eulerAngles.z);
    }
}
