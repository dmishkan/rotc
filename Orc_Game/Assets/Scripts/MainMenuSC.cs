using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSC : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Platform2d");
    }
    
    public void GoToCredits()
    {
        SceneManager.LoadScene("CreditsMenu");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
