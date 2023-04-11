using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public int health = 5, maxHealth = 5, currentWaypoint = 0;
    public float speed = 100f, nextWaypointDistance = 1f;

    // Get a reference to the SpriteRenderer component on a child object of the enemy object
    public Sprite[] enemyGFX;
    SpriteRenderer currentGFX => GetComponentInChildren<SpriteRenderer>();
    public GameObject healthBarUI;
    public Slider healthSlider;

    Path path;
    Transform target => FindObjectOfType<Player>(true).transform;

    // Get references to the Seeker and Rigidbody2D components attached to the enemy object
    Seeker seeker => GetComponent<Seeker>();
    Rigidbody2D rb => GetComponent<Rigidbody2D>();

     void Start()
    {
        // Start a repeating method to update the enemy's path
        InvokeRepeating("UpdatePath", 0f, .5f);

        // Set the enemy's speed to a random value between 50 and 150
        speed = Random.Range(50, 150);

        // Update the value of the health slider to reflect the enemy's current health
        healthSlider.value = CalculateHealth();
    }

     void Update()
    {
        // Check the enemy's health and update its pathing and direction
        CheckHealth();
        Pathing();
        AnimateSprite();
    }

     void LateUpdate()
    {
        // Update the value of the health slider to reflect the enemy's current health
        healthSlider.value = CalculateHealth();
    }

     void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy collides with a Gastro Liquid projectile, decrease its health and play a hit sound
        if (collision.gameObject.name == "Gastro Liquid(Clone)")
        {
            GameManager.PlaySound("enemyhit");
            Destroy(collision.gameObject);
            health -= 1;
        }
        // If the enemy collides with an Antibiotic projectile and the enemy is a virus, decrease its health and play a hit sound
        if (collision.gameObject.name == "Antibiotic(Clone)" && gameObject.tag == "Virus")
        {
            GameManager.PlaySound("enemyhit");
            Destroy(collision.gameObject);
            health -= 1;
        }
        // If the enemy collides with an Anasthetic projectile and the enemy is a bacteria, decrease its health and play a hit sound
        else if (collision.gameObject.name == "Antacid(Clone)" && gameObject.tag == "Bacteria")
        {
            GameManager.PlaySound("enemyhit");
            Destroy(collision.gameObject);
            health -= 1;
        }
        // If the enemy collides with the player, play a death sound, trigger the player's death event, and deactivate the player object
        else if (collision.gameObject.tag == "Player")
        {
            GameManager.PlaySound("death");
            GameManager.PlayerDeath?.Invoke();
            collision.gameObject.SetActive(false);
        }
    }

    // This method checks the enemy's health and destroys the enemy if its health falls to or below 0
    void CheckHealth()
    {
        if (health <= 0)
        {
            GameManager.PlaySound("enemydeath"); // Play a sound effect for the enemy's death
            Destroy(gameObject); // Destroy the enemy game object
        }
    }

    // This method calculates the current health of the enemy as a float between 0 and 1
    float CalculateHealth()
    {
        return (float)health / maxHealth;
    }

    // This method updates the enemy's path by using the Seeker component to find a path to the target player object
    void UpdatePath()
    {
        if (seeker.IsDone()) // If the Seeker component has finished finding a path
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete); // Start finding a new path from the enemy's current position to the target player's position
        }
    }

    void Pathing()
    {
        // If there is no path or the current waypoint is beyond the end of the path, return and do nothing
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // Determine the direction to move towards the current waypoint and apply a force in that direction
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        // If the enemy is close enough to the current waypoint, move to the next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // This method is called when the Seeker component finishes finding a path
    void OnPathComplete(Path p)
    {
        // If there was no error finding the path, update the enemy's path and set the current waypoint to the first waypoint
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // This method updates the enemy's direction based on its velocity
    void AnimateSprite()
    {
        if (rb.velocity.x >= 0.1f && rb.velocity.y >= 0.1f)
        {
            currentGFX.sprite = enemyGFX[6];
        }
        else if (rb.velocity.x >= 0.1f && rb.velocity.y <= -0.1f)
        {
            currentGFX.sprite = enemyGFX[1];
        }
        else if (rb.velocity.x >= 0.1f)
        {
            currentGFX.sprite = enemyGFX[0];
        }
        else if (rb.velocity.x <= -0.1f && rb.velocity.y <= -0.1f)
        {
            currentGFX.sprite = enemyGFX[4];
        }
        else if (rb.velocity.x <= -0.1f && rb.velocity.y >= 0.1f)
        {
            currentGFX.sprite = enemyGFX[3];
        }
        else if (rb.velocity.x <= -0.1f)
        {
            currentGFX.sprite = enemyGFX[2];
        }
        else
        {
            currentGFX.sprite = enemyGFX[5];
        }
    }
}
