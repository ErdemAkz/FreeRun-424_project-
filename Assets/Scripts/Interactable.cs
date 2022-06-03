using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //looking interactable message
    public string promptMessage;

    //player will call this
    public void BaseInteract()
    {
        Interact();
    }

    //to be overriden by subclasses
    protected virtual void Interact()
    {

    }

}
