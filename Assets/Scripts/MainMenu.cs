using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        GameManager.gameIsPaused = false;
        Player.canMove = true;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Play()
    {
        SceneManager.LoadScene($"Level{GameManager.currentLevel}");
    }

    public void Quit()
    {
        print("Quitting Game...");
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
