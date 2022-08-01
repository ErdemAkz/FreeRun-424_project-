using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    [Range(5, 100)]
    public float bounceHeight;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject bouncer = collision.gameObject;
        Rigidbody rb = bouncer.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * bounceHeight, ForceMode.Impulse);
    }

}
