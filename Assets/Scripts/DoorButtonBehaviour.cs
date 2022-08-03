using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButtonBehaviour : MonoBehaviour
{
    public GameObject Door;
    public GameObject Player;

    private int requiredDistanceToBePressed = 3;

    void Update()
    {
        Debug.Log(Vector3.Distance(Player.transform.position, transform.position));
        if (Input.GetKey(KeyCode.F) && Vector3.Distance(Player.transform.position, transform.position) < requiredDistanceToBePressed)
        {
            Door.GetComponent<Animation>().Play();
        }
    }

}
