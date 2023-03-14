using Mono.Cecil;
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
    public static bool gameOver = false;
    bool canPlaySound;
    [SerializeField] public GameObject UI, victoryUI, gameOverUI;
    [SerializeField] public TextMeshProUGUI ammoText, enemyText;
    [SerializeField] public Weapon weapon;

    private void Awake()
    {
        walkSound = Resources.Load<AudioClip>("walk");
        deathSound = Resources.Load<AudioClip>("death");
        enemyHitSound = Resources.Load<AudioClip>("enemyhit");
        enemyDeathSound = Resources.Load<AudioClip>("enemydeath");
        playerShootSound = Resources.Load<AudioClip>("playershoot");
        victorySound = Resources.Load<AudioClip>("victory");
        gameOverSound = Resources.Load<AudioClip>("gameover");
        reloadSound = Resources.Load<AudioClip>("reload");

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayerExit += OnPlayerExit;
        PlayerDeath += OnPlayerDeath;
    }

    private void Update()
    {
        enemiesAlive = FindObjectsOfType<Enemy>();
        UpdateAmmoText();
        RetryCheck();
        UpdateEnemyText();
    }

    void UpdateAmmoText()
    {
        if (weapon.currentTank <= 0)
        {
            ammoText.color = Color.grey;
        }
        else
        {
            Color ammoColor = weapon.ammoType.GetComponent<SpriteRenderer>().color;
            ammoText.color = ammoColor;
        }
        ammoText.text = $"{weapon.currentTank} / {weapon.maxTank}";
    }

    void RetryCheck()
    {
        if (gameOver && Input.GetKey(KeyCode.Return))
        {
            NewGame();
        }
    }

    void UpdateEnemyText()
    {
        enemyText.text = enemiesAlive.Length.ToString();
    }

    void OnPlayerExit()
    {
        GameManager.PlaySound("victory");
        UI.SetActive(false);
        victoryUI.SetActive(true);
        Time.timeScale = 0;
    }

    void OnPlayerDeath()
    {
        GameManager.PlaySound("gameover");
        UI.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        gameOver = true;
    }

    void NewGame()
    {
        SceneManager.LoadScene("Level1");
        weapon.currentTank = 0;
        UI.SetActive(true);
        gameOverUI.SetActive(false);
        Time.timeScale = 1.0f;
        gameOver = false;
        PlayerDeath -= OnPlayerDeath;
    }

    public static void PlaySound(string clip)
    {
        audioSource.volume = .25f;
        if (gameOver == true)
        {
            audioSource.pitch *= 0.5f;
        }
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
