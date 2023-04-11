using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static AudioClip walkSound, enemyHitSound, enemyDeathSound, playerShootSound, victorySound, gameOverSound, reloadSound;
    static AudioSource audioSource;

    public static Action PlayerExit, PlayerDeath;

    public static Enemy[] enemiesAlive => FindObjectsOfType<Enemy>();
    public static EnemySpawner[] problemsLeft => FindObjectsOfType<EnemySpawner>();
    public static Vector3 playerSpawnPoint;

    public GameObject ui, victoryUI, gameOverUI, pauseMenu, finish;

    [SerializeField] public TextMeshProUGUI ammoText, enemyText, problemsText;
    [SerializeField] public AudioSource BGM;
    [SerializeField] public Weapon weapon;

    public static bool gameOver, gameIsPaused, playerHasMoved;
    public static string currentLevelName => SceneManager.GetActiveScene().name;
    public static int currentLevel = 1;

    void Awake()
    {
        // Load all the required audio clips from Resources folder
        walkSound = Resources.Load<AudioClip>("walk");
        enemyHitSound = Resources.Load<AudioClip>("enemyhit");
        enemyDeathSound = Resources.Load<AudioClip>("enemydeath");
        playerShootSound = Resources.Load<AudioClip>("playershoot");
        victorySound = Resources.Load<AudioClip>("victory");
        gameOverSound = Resources.Load<AudioClip>("gameover");
        reloadSound = Resources.Load<AudioClip>("reload");

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        playerSpawnPoint = FindObjectOfType<Player>().transform.position;
        // Allow the game over and victory effects to take place

        PlayerExit += OnPlayerExit; // Subscribe the OnPlayerExit method to the PlayerExit Action

        PlayerDeath += OnPlayerDeath; // Subscribe the OnPlayerDeath method to the PlayerDeath Action
    }

    void Update()
    {
        // Update the UI text for the ammo, number of remaining enemies and problems
        UpdateAmmoText();
        UpdateEnemyText();
        UpdateProblemText();

        // Check if win condition is met
        CheckWinCondition();

        // Check if the player is attempting to restart or pause the game
        HasPlayerMoved();
        RetryCheck();
        PauseCheck();
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

    void CheckWinCondition()
    {
        if (enemyText.color == Color.green && problemsText.color == Color.green)
        {
            finish.SetActive(true);
        }
        else
        {
            finish.SetActive(false);
        }
    }

    // Update the UI text for the number of remaining enemies
    void UpdateEnemyText()
    {
        if (enemiesAlive.Length <= 0)
        {
            enemyText.color = Color.green;
        }
        else
        {
            enemyText.color = Color.white;
        }
        
        enemyText.text = $"Enemies Remaining: {enemiesAlive.Length}";
    }

    void UpdateProblemText()
    {
        if (problemsLeft.Length <= 0)
        {
            problemsText.color = Color.green;
        }
        else
        {
            problemsText.color = Color.white;
        }
        problemsText.text = $"Problems Remaining: {problemsLeft.Length}";
    }

    // Check if the game is over and the player is attempting to restart
    void RetryCheck()
    {
        if (gameOver && Input.GetKey(KeyCode.Return))
        {
            NewGame();
        }
    }

    void PauseCheck()
    {
        if (pauseMenu != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !gameIsPaused)
            {
                gameIsPaused = true;
                BGM.Pause();
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Player.canMove = false;
            }
            else if (gameIsPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                gameIsPaused = false;
                BGM.UnPause();
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                Player.canMove = true;
            }
        }

    }

    // Handle player victory
    void OnPlayerExit()
    {
        Player.canMove = false;
        // Play victory sound
        if (audioSource != null)
        {
            PlaySound("victory");
        }

        // Disable the game UI and show the victory screen
        if (ui != null)
        {
            ui.SetActive(false);
        }
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }
        if (BGM.clip != null)
        {
            BGM.Stop();
        }

        // Freeze time to prevent any further game updates
        Time.timeScale = 0;
    }

    // Handle player death
    void OnPlayerDeath()
    {
        
        

        // Disable the game UI and show the game over screen
        if (ui != null)
        {
            ui.SetActive(false);
        }
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Stop the background music
        if(BGM.clip != null)
        {
            BGM.Stop();
        }
        
        // Freeze time to prevent any further game updates
        Time.timeScale = 0;
        
        // Play game over sound
        GameManager.PlaySound("gameover");

        // Set the game over flag to true
        gameOver = true;
    }

    void HasPlayerMoved()
    {
        if (FindObjectOfType<Player>() != null)
        {
            if (playerSpawnPoint == FindObjectOfType<Player>().transform.position)
            {
                playerHasMoved = false;
            }
            else
            {
                playerHasMoved = true;
            }
        }
    }

    // Restart the game
    void NewGame()
    {
        PlayerDeath -= OnPlayerDeath;
        // Load the current level
        SceneManager.LoadScene($"{currentLevelName}");

        // Reset the current ammo count to zero
        weapon.currentTank = 0;

        // Enable the game UI and hide the game over screen if they are not null
        if (ui != null)
        {
            ui.SetActive(true);
        }
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        //Unfreeze time
        Player.canMove = true;
        playerHasMoved = false;
        Time.timeScale = 1.0f;
        gameOver = false;
        BGM.Play();
    }
    public void NextLevel()
    {
        // Player is on the next level
        currentLevel++;

        // Load the next level
        SceneManager.LoadScene($"Level{currentLevel}");

        // Reset the current ammo count to zero
        weapon.currentTank = 0;

        // Unfreeze time
        Time.timeScale = 1.0f;

        Player.canMove = true;
    }

    public static void PlaySound(string clip)
    {
        // Play the appropriate sound clip based on the string parameter passed in
        switch (clip)
        {
            case "walk":
                audioSource.PlayOneShot(walkSound);
                break;
            case "enemyhit":
                audioSource.PlayOneShot(enemyHitSound, .5f);
                break;
            case "enemydeath":
                audioSource.PlayOneShot(enemyDeathSound, .5f);
                break;
            case "playershoot":
                audioSource.PlayOneShot(playerShootSound, .5f);
                break;
            case "victory":
                audioSource.PlayOneShot(victorySound);
                break;
            case "gameover":
                audioSource.PlayOneShot(gameOverSound);
                break;
            case "reload":
                audioSource.PlayOneShot(reloadSound, 3f);
                break;
        }
    }
}
