using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        transform.position = cameraPosition.position;
    }
}