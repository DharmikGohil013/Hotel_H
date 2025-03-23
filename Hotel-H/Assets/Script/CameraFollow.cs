using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public Vector3 offset = new Vector3(0, 2, -5); // Adjust camera position

    public float smoothSpeed = 0.1f;

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.LookAt(player); // Keep camera looking at the player
    }
}
