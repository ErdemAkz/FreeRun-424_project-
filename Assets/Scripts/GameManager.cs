using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool gameFinished { get; private set; }
    public static int numOfTargetsHit { get; private set; }

    float currentTime;
    [SerializeField]
    TextMeshProUGUI InitialTimerText;
    [SerializeField]
    TextMeshProUGUI TargetHitsText;
    [SerializeField]
    TextMeshProUGUI FinalTimerText;

    public PauseManager PauseManager;

    Transform tempParent;

    private void Awake()
    {
        Instance = this;

        gameFinished = false;
        numOfTargetsHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameFinished)
        {
            //CurrentTime += Time.deltaTime;
            TimeSpan initialTimeSpan = TimeSpan.FromSeconds(currentTime);
            TimeSpan finalTimeSpan = initialTimeSpan.Subtract(new TimeSpan(0, 0, numOfTargetsHit*3));

            InitialTimerText.text = "Initial Time Score: " + string.Format("{0:D2}:{1:D2}:{2:D2}", initialTimeSpan.Minutes, initialTimeSpan.Seconds, initialTimeSpan.Milliseconds);
            TargetHitsText.text = "Number of Target Hits: " + numOfTargetsHit.ToString();
            FinalTimerText.text = "Final Time Score: " + string.Format("{0:D2}:{1:D2}:{2:D2}", finalTimeSpan.Minutes, finalTimeSpan.Seconds, finalTimeSpan.Milliseconds);

            /*Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);*/
            PauseManager.ScorePanel();
        }
    }

    public void TargetHit()
    {
        numOfTargetsHit++;
    }

    public void GameFinished(float currentTime)
    {
        this.currentTime = currentTime;
        gameFinished = true;
    }

}
