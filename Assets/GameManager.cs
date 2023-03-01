using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Action EnemyDeath, ProblemsCleared, PlayerExit, PlayerDeath;
    public static int score = 0;
    bool gameOver = false;
    [SerializeField] public GameObject UI, victoryUI, gameOverUI;
    [SerializeField] public TextMeshProUGUI ammoText, scoreText;
    [SerializeField] public Weapon weapon;
    [SerializeField] public GameObject finish;

    private void Start()
    {
        scoreText.text = "0";
        EnemyDeath += OnEnemyKilled;
        ProblemsCleared += OnProblemsCleared;
        PlayerExit += OnPlayerExit;
        PlayerDeath += OnPlayerDeath;
    }

    private void Update()
    {
        UpdateScoreText();
        UpdateAmmoText();
        if(gameOver && Input.GetKey(KeyCode.Return))
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Level1");
            gameOver = false;
            score = 0;
            UpdateScoreText();
            PlayerDeath -= OnPlayerDeath;
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    void UpdateAmmoText()
    {
        ammoText.text = $"{weapon.currentTank} / {weapon.maxTank}";
        if (weapon.currentTank > weapon.maxTank)
        {
            ammoText.color = Color.cyan;
        }
        else if (weapon.currentTank <= 25)
        {
            ammoText.color = Color.grey;
        }
        else
        {
            ammoText.color = Color.green;
        }
    }

    void OnEnemyKilled()
    {
        score += 50;
    }

    void OnProblemsCleared()
    {
        finish?.SetActive(true);
    }

    void OnPlayerExit()
    {
        UI.SetActive(false);
        victoryUI.SetActive(true);
        Time.timeScale = 0;
        finish?.SetActive(false);
    }

    void OnPlayerDeath()
    {
        UI.SetActive(false);
        gameOverUI.SetActive(true);
        finish?.SetActive(false);
        Time.timeScale = 0;
        gameOver = true;
    }
}
