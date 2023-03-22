using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static AudioClip walkSound, deathSound, enemyHitSound, enemyDeathSound, playerShootSound, victorySound, gameOverSound, reloadSound;
    public static Action PlayerExit, PlayerDeath;
    static AudioSource audioSource;
    public Enemy[] enemiesAlive;
    public static bool gameOver = false;
    bool canPlaySound;
    public GameObject UI, victoryUI, gameOverUI;
    [SerializeField] public TextMeshProUGUI ammoText, enemyText;
    [SerializeField] public Weapon weapon;

    private void Awake()
    {
        // Load all the required audio clips from Resources folder
        walkSound = Resources.Load<AudioClip>("walk");
        deathSound = Resources.Load<AudioClip>("death");
        enemyHitSound = Resources.Load<AudioClip>("enemyhit");
        enemyDeathSound = Resources.Load<AudioClip>("enemydeath");
        playerShootSound = Resources.Load<AudioClip>("playershoot");
        victorySound = Resources.Load<AudioClip>("victory");
        gameOverSound = Resources.Load<AudioClip>("gameover");
        reloadSound = Resources.Load<AudioClip>("reload");

        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Allow the game over and victory effects to take place

        PlayerExit += OnPlayerExit; // Subscribe the OnPlayerExit method to the PlayerExit Action

        PlayerDeath += OnPlayerDeath; // Subscribe the OnPlayerDeath method to the PlayerDeath Action
    }

    private void Update()
    {
        // Find all active Enemy objects in the scene
        enemiesAlive = FindObjectsOfType<Enemy>();

        // Update the UI text for the ammo count
        UpdateAmmoText();

        // Check if the player is attempting to restart the game
        RetryCheck();

        // Update the UI text for the number of remaining enemies
        UpdateEnemyText();
    }

    // Update the UI text for the ammo count
    void UpdateAmmoText()
    {
        // If the current ammo count is zero, make the text color grey
        if (weapon.currentTank <= 0)
        {
            ammoText.color = Color.grey;
        }
        // Otherwise, set the text color to match the color of the current ammo type
        else
        {
            Color ammoColor = weapon.ammoType.GetComponent<SpriteRenderer>().color;
            ammoText.color = ammoColor;
        }
        // Update the text to display the current and max ammo count
        ammoText.text = $"{weapon.currentTank} / {weapon.maxTank}";
    }

    // Check if the game is over and the player is attempting to restart
    void RetryCheck()
    {
        if (gameOver && Input.GetKey(KeyCode.Return))
        {
            NewGame();
        }
    }

    // Update the UI text for the number of remaining enemies
    void UpdateEnemyText()
    {
        enemyText.text = enemiesAlive.Length.ToString();
    }

    // Handle player victory
    void OnPlayerExit()
    {
        // Play victory sound
        GameManager.PlaySound("victory");

        // Disable the game UI and show the victory screen
        UI.SetActive(false);
        victoryUI.SetActive(true);

        // Freeze time to prevent any further game updates
        Time.timeScale = 0;
    }

    // Handle player death
    void OnPlayerDeath()
    {
        // Play game over sound
        GameManager.PlaySound("gameover");

        // Disable the game UI and show the game over screen
        UI.SetActive(false);
        gameOverUI.SetActive(true);

        // Freeze time to prevent any further game updates
        Time.timeScale = 0;

        // Set the game over flag to true
        gameOver = true;
    }

    // Restart the game
    void NewGame()
    {
        // Load the first level
        SceneManager.LoadScene("Level1");

        // Reset the current ammo count to zero
        weapon.currentTank = 0;

        // Enable the game UI and hide the game over screen
        UI.SetActive(true);
        gameOverUI.SetActive(false);
        Time.timeScale = 1.0f;
        gameOver = false;
        PlayerDeath -= OnPlayerDeath;
    }

    public static void PlaySound(string clip)
    {
        // Set the volume of the audio source
        audioSource.volume = .25f;

        // If the game is over, lower the pitch of the audio source
        if (gameOver == true)
        {
            audioSource.pitch *= 0.5f;
        }

        // Play the appropriate sound clip based on the string parameter passed in
        switch (clip)
        {
            case "walk":
                audioSource.PlayOneShot(walkSound);
                break;
            case "death":
                audioSource.PlayOneShot(deathSound);
                break;
            case "enemyhit":
                audioSource.PlayOneShot(enemyHitSound);
                break;
            case "enemydeath":
                audioSource.PlayOneShot(enemyDeathSound);
                break;
            case "playershoot":
                audioSource.PlayOneShot(playerShootSound);
                break;
            case "victory":
                audioSource.PlayOneShot(victorySound);
                break;
            case "gameover":
                audioSource.PlayOneShot(gameOverSound);
                break;
            case "reload":
                audioSource.PlayOneShot(reloadSound);
                break;
        }
    }

    /*    void EnemyHitSound()
        {
            if(canPlaySound)
            {
                canPlaySound = false;
                audioSource.volume = 0.5f;
                audioSource.PlayOneShot(enemyHitSound);
                yield return new WaitForSeconds(.5f);
                canPlaySound = true;
            }
            yield return null;
        }*/
}
