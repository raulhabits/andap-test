using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEvent : MonoBehaviour
{
    public Transform target;         // Player
    public Vector3 offset = new Vector3(0f, 5f, -7f);
    public float followSpeed = 10f;
    public bool lookAtTarget = true;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}
