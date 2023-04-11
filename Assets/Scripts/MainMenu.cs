using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer/* => Resources.Load<AudioMixer>("MainMixer")*/;

    public void ReturnToMainMenu()
    {
        GameManager.gameIsPaused = false;
        Player.canMove = true;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void Play()
    {
        SceneManager.LoadScene($"Level{GameManager.currentLevel}");
    }

    public void Quit()
    {
        print("Quitting Game...");
        Application.Quit();
    }
}
