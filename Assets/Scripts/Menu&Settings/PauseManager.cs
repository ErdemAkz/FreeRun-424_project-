using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    Transform tempParent;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (GameIsPaused && pauseMenuUI != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Resume()
    {
        for (int i = 0; i < pauseMenuUI.transform.GetChild(1).childCount; i++)
        {
            pauseMenuUI.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        pauseMenuUI.transform.GetChild(0).gameObject.SetActive(false);
        pauseMenuUI.transform.GetChild(1).gameObject.SetActive(false);
        //pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        for (int i = 0; i < pauseMenuUI.transform.GetChild(1).childCount; i++)
        {
            pauseMenuUI.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        pauseMenuUI.transform.GetChild(0).gameObject.SetActive(true);
        pauseMenuUI.transform.GetChild(1).gameObject.SetActive(true);
        pauseMenuUI.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        //pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Tech Demo Scene");
    }

    public void MoveToFront(GameObject currentObj)
    {
        //tempParent = currentObj.transform.parent;
        tempParent = currentObj.transform;
        tempParent.SetAsLastSibling();
    }
}
