using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

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
    public float CurrentTime;


    // Start is called before the first frame update
    void Start()
    {
        //GameEndedText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray raystart = new Ray(new Vector3(startwall.transform.position.x, startwall.transform.position.y+1.0f, startwall.transform.position.z), startwall.transform.right);
        Ray rayend = new Ray(new Vector3(endwall.transform.position.x-6.0f, endwall.transform.position.y+1.5f, endwall.transform.position.z + 3.0f), endwall.transform.right);
        Debug.DrawRay(raystart.origin, raystart.direction * distance);
        Debug.DrawRay(rayend.origin, rayend.direction * distance);
        updateTimer();
        RaycastHit hitInfoStart; // store collision info
        RaycastHit hitInfoEnd; // store collision info
        if (Physics.Raycast(raystart, out hitInfoStart, distance))
        {
            if (hitInfoStart.collider.gameObject.CompareTag("Player") && !started)
            {
                started = true;
                startTime = Time.time;
            }
        }

        if (Physics.Raycast(rayend, out hitInfoEnd, distance))
        {
            
            if (hitInfoEnd.collider.gameObject.CompareTag("Player") && started)
            {
                //GameEndedText.enabled = true;
                started = false;
                GameManager.Instance.GameFinished(CurrentTime);
            }
        }

        if (started)
        {
            //float t = Time.time - startTime;
            //string minutes = ((int)t / 60).ToString();
            //string seconds = (t % 60).ToString("f0");

            //TimerText.text = minutes + ":" + seconds;
            //GameEndedText.enabled = false;
        }


    }

    void updateTimer()
    {
        if (started)
        {
            CurrentTime += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(CurrentTime);
            TimerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);//currentTime.ToString("0.00");
        }
    }
}
