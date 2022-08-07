using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 cameraOffset;
    public float cameraSpeed;

    void FixedUpdate()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + cameraOffset, cameraSpeed);
        }
    }
}
