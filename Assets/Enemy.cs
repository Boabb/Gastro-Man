using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health = 10, maxHealth = 10, movementSpeed = 2;
    public GameObject healthBarUI;
    public Slider healthSlider;

    private void Start()
    {
        health = maxHealth;
        healthSlider.value = CalculateHealth();
    }

    private void Update()
    {
        CheckHealth();
        EnemyMovement(ChangeDirection());
    }

    private void LateUpdate()
    {
        healthSlider.value = CalculateHealth();
    }

    int ChangeDirection()
    {
        Vector3 currentPosition = (transform.position).normalized;
        int moveDirection = (int)currentPosition.x;
        return moveDirection;
    }

    void EnemyMovement(int direction)
    {
        Vector2 position = this.transform.position;
        position = new Vector2(position.x + Time.deltaTime * direction * movementSpeed, position.y);
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            health -= 1;
        }

        else if(collision.gameObject.tag == "Player")
        {
            GameManager.PlayerDeath?.Invoke();
            collision.gameObject.SetActive(false);
        }
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.EnemyDeath?.Invoke();
        }
    }

    float CalculateHealth()
    {
        return (float)health / maxHealth;
    }
}
