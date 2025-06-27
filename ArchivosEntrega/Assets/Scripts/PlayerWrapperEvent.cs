using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWrapperEvent : MonoBehaviour
{
    [SerializeField] private Transform _child;

    private void Update()
    {
        transform.position = _child.position;
    }
}