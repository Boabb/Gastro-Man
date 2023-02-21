using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int score = 0, health = 50, maxHealth = 50;
    public GameObject healthBarUI;
    public Slider healthSlider;

    private void Start()
    {
        health = maxHealth;
        healthSlider.value = CalculateHealth();
    }

    private void Update()
    {
        healthSlider.value = CalculateHealth();
        if (health <= 0)
        {
            EnemyKilled();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            health -= 1;
        }
    }
    void EnemyKilled()
    {
        Destroy(gameObject);
        score += 50;
    }
    
    float CalculateHealth()
    {
        return (float)health / maxHealth;
    }
}
