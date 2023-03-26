using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static AudioClip walkSound, deathSound, enemyHitSound, enemyDeathSound, playerShootSound, victorySound, gameOverSound, reloadSound;
    public static Action PlayerExit, PlayerDeath;
    static AudioSource audioSource;
    public Enemy[] enemiesAlive;
    public EnemySpawner[] problemsLeft;
    public static bool gameOver = false, enemyHasSpawned;
    public GameObject ui, victoryUI, gameOverUI, finish;
    static int currentLevel = 1;
    [SerializeField] public TextMeshProUGUI ammoText, enemyText, problemsText;
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
        // Assign all active Enemy and EnemySpawner objects in the scene to two seperate arrays
        enemiesAlive = FindObjectsOfType<Enemy>();
        problemsLeft = FindObjectsOfType<EnemySpawner>();

        // Update the UI text for the ammo, number of remaining enemies and problems
        UpdateAmmoText();
        UpdateEnemyText();
        UpdateProblemText();

        // Check if win condition is met
        CheckWinCondition();

        // Check if the player is attempting to restart the game
        RetryCheck();

        if (enemyHasSpawned)
        {
            StartCoroutine(EnemySpawnCooldown());
        }
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

    IEnumerator EnemySpawnCooldown()
    {
        yield return new WaitForSeconds(2);
        enemyHasSpawned = false;
    }

    // Handle player victory
    void OnPlayerExit()
    {
        // Play victory sound
        GameManager.PlaySound("victory");

        // Disable the game UI and show the victory screen
        ui.SetActive(false);
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
        ui.SetActive(false);
        gameOverUI.SetActive(true);

        // Freeze time to prevent any further game updates
        Time.timeScale = 0;

        // Set the game over flag to true
        gameOver = true;
    }

    // Restart the game
    void NewGame()
    {
        PlayerDeath -= OnPlayerDeath;
        // Load the first level
        SceneManager.LoadScene($"Level{currentLevel}");

        // Reset the current ammo count to zero
        weapon.currentTank = 0;

        // Enable the game UI and hide the game over screen
        ui.SetActive(true);
        gameOverUI.SetActive(false);

        //Unfreeze time
        Time.timeScale = 1.0f;
        gameOver = false;
    }
    public void NextLevel()
    {
        // Load the next level
        SceneManager.LoadScene($"Level{currentLevel+1}");

        // Reset the current ammo count to zero
        weapon.currentTank = 0;

        // Unfreeze time
        Time.timeScale = 1.0f;
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
