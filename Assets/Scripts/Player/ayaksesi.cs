using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ayaksesi : MonoBehaviour
{
    AudioSource source;
    public AudioClip[] footstepsounds;
    bool isMoving;
    public float timebetweenSteps;
    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        if(horizontal != 0 || vertical != 0)
            isMoving = true;
        else
            isMoving = false;

        if(isMoving){
            
            timer -= Time.deltaTime;

            if(timer<=0){
                timer = timebetweenSteps;
                source.clip = footstepsounds[Random.Range(0,footstepsounds.Length-1)];
                source.Play();
            }
        }
        else{
            timer = timebetweenSteps;
        }
    }
}
