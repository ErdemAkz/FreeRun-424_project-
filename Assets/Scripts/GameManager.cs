using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool gameFinished { get; private set; }
    public static int numOfTargetsHit { get; private set; }

    [SerializeField]
    TextMeshProUGUI TimerText;

    public GameObject gameOverPanel;

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
            gameOverPanel.SetActive(true);
        }
    }

    public void TargetHit()
    {
        numOfTargetsHit++;
    }

    public void GameFinished(string timerText)
    {   
        TimerText.text = timerText;
        gameFinished = true;
    }
}
