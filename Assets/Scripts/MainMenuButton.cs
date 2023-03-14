using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnQuitClick()
    {
        print("Quitting Game...");
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
