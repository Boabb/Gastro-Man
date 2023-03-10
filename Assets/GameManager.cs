using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Action EnemyDeath, ProblemsCleared, PlayerExit, PlayerDeath;
    public static int score = 0, enemiesAlive = 0;
    bool gameOver = false;
    [SerializeField] public GameObject UI, victoryUI, gameOverUI;
    [SerializeField] public TextMeshProUGUI ammoText, scoreText, enemyText;
    [SerializeField] public Weapon weapon;

    private void Start()
    {
        scoreText.text = "0";
        EnemyDeath += OnEnemyKilled;
        PlayerExit += OnPlayerExit;
        PlayerDeath += OnPlayerDeath;
    }

    private void Update()
    {
        UpdateScoreText();
        UpdateAmmoText();
        RetryCheck();
        UpdateEnemyText();
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    void UpdateAmmoText()
    {
        ammoText.text = $"{weapon.currentTank} / {weapon.maxTank}";
        Color ammoColor = weapon.ammoType.GetComponent<SpriteRenderer>().color;
        ammoText.color = ammoColor;
    }

    void RetryCheck()
    {
        if (gameOver && Input.GetKey(KeyCode.Return))
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
            gameOver = false;
            score = 0;
            UpdateScoreText();
            PlayerDeath -= OnPlayerDeath;
        }
    }

    void UpdateEnemyText()
    {
        enemiesAlive = FindObjectsOfType<Enemy>().Length;
        enemyText.text = enemiesAlive.ToString();
    }

    void OnEnemyKilled()
    {
        score += 50;
    }

    void OnPlayerExit()
    {
        UI.SetActive(false);
        victoryUI.SetActive(true);
        Time.timeScale = 0;
    }

    void OnPlayerDeath()
    {
        UI.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        gameOver = true;
    }
}
