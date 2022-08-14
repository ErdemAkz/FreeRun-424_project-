using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    GameObject startwall;
    [SerializeField]
    GameObject endwall;
    [SerializeField]
    float distance = 20f;

    [SerializeField]
    TextMeshProUGUI TimerText;
    //[SerializeField]
    //TextMeshProUGUI GameEndedText;

    private float startTime;
    public bool started = false;


    // Start is called before the first frame update
    void Start()
    {
        //GameEndedText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray raystart = new Ray(new Vector3(startwall.transform.position.x, startwall.transform.position.y+1.5f, startwall.transform.position.z), startwall.transform.right);
        Ray rayend = new Ray(new Vector3(endwall.transform.position.x-6.0f, endwall.transform.position.y+1.5f, endwall.transform.position.z + 3.0f), endwall.transform.right);
        Debug.DrawRay(raystart.origin, raystart.direction * distance);
        Debug.DrawRay(rayend.origin, rayend.direction * distance);

        RaycastHit hitInfoStart; // store collision info
        RaycastHit hitInfoEnd; // store collision info
        if (Physics.Raycast(raystart, out hitInfoStart, distance))
        {
            Debug.Log(hitInfoStart.collider.gameObject.tag);
            if (hitInfoStart.collider.gameObject.CompareTag("Player") && !started)
            {
                Debug.Log("asdasdasdasdas");
                started = true;
                startTime = Time.time;
            }
        }

        if (Physics.Raycast(rayend, out hitInfoEnd, distance))
        {
            Debug.Log("end!!");
            if (hitInfoEnd.collider.gameObject.CompareTag("Player") && started)
            {
                //GameEndedText.enabled = true;
                started = false;
            }
        }

        if (started)
        {
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f0");

            TimerText.text = minutes + ":" + seconds;
            //GameEndedText.enabled = false;
        }


    }
}
