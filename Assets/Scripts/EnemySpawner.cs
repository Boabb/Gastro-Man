using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Array of enemy prefabs to spawn
    public Enemy[] prefabs;

    // The current enemy prefab to spawn
    Enemy enemyToSpawn;

    // The current health of the enemy spawner
    public int health;

    // The spawn point for enemies
    Vector3 spawnPoint, playerSpawnPoint;

    // Boolean for spawning enemies
    public static bool playerHasMoved = false;

    private void Awake()
    {
        playerSpawnPoint = FindObjectOfType<Player>().transform.position;
    }
    void Start()
    {
        // Set the spawn point for enemies to be just below the spawner object
        spawnPoint = new Vector2(transform.position.x, transform.position.y - 1);

        // If this spawner is the "Ulcer" problem
        if (gameObject.name == "Ulcer")
        {
            // Set the initial health to 5
            health = 5;

            // Set the enemy to be spawned to be the "Bacteria" enemy type
            enemyToSpawn = prefabs[0];
        }

        // If this spawner is the "Infection" problem
        if (gameObject.name == "Infection")
        {
            // Set the initial health to 5
            health = 5;

            // Set the enemy to be spawned to be the "Virus" enemy type
            enemyToSpawn = prefabs[1];
        }

        // Start spawning enemies of "enemyToSpawn" type
        StartCoroutine(SpawnEnemies());
    }

    // Coroutine that spawns enemies of the given type
    IEnumerator SpawnEnemies()
    {
        // Loop indefinitely
        while (true)
        {
            // Generate a random number between 0 and 8
            int randomNum = Random.Range(0, 9);

            // If the enemy's health is less than the random number
            if (GameManager.enemyHasSpawned == false && GameManager.enemiesAlive.Length < 5 && playerHasMoved)
            {
                // Spawn a new enemy of the given type at the spawn point
                Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);

                GameManager.enemyHasSpawned = true;
            }

            // Wait for 5 seconds before trying to spawn another enemy
            yield return new WaitForSeconds(5f);
        }

    }

    void Update()
    {
        HasPlayerMoved();
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

    // Called when the enemy collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy collided with "Gastro Liquid"
        if (collision.gameObject.name == "Gastro Liquid(Clone)")
        {
            // Decrease the enemy's health by 1
            health -= 1;
        }

        // If this is an "Infection" enemy and it collided with "Antibiotic"
        else if (gameObject.name == "Infection" && collision.gameObject.name == "Antibiotic(Clone)")
        {
            // Decrease the enemy's health by 1
            health -= 1;
        }

        // If this is a "Ulcer" enemy and it collided with "Antacid"
        else if (gameObject.name == "Ulcer" && collision.gameObject.name == "Antacid(Clone)")
        {
            // Decrease the enemy's health by 1
            health -= 1;
        }
        // If the enemy's health has reached 0 or below
        if (health <= 0)
        {
            // Spawn 3 new enemies of the same type at the same location
            for (int i = 0; i < 3; i++)
            {
                Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
            }

            // Destroy this enemy object
            Destroy(gameObject);
        }
    }
}
