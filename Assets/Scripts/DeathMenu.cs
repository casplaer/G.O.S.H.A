using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenu;
    public static bool isDead;

    void Start()
    {
        deathMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            deathMenu.SetActive(true);
            Time.timeScale = 0f;
            isDead = true;
        }
        else
        {
            deathMenu.SetActive(false);
        }
                
    }

    public void ResumeGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        deathMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isDead = false;
    }

    public void GoToMainMenu()
    {
        deathMenu.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
