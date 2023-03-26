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
    Vector2 spawnPoint;

    void Start()
    {
        // Set the spawn point for enemies to be just below the spawner object
        spawnPoint = new Vector2(transform.position.x, transform.position.y - 1);

        // If this spawner is the "Cyst" problem
        if (gameObject.name == "Cyst")
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

    private void Update()
    {
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

    // Coroutine that spawns enemies of the given type
    IEnumerator SpawnEnemies()
    {
        // Loop indefinitely
        while (true)
        {
            // Generate a random number between 0 and 8
            int randomNum2 = Random.Range(0, 9);

            // If the enemy's health is less than the random number
            if (health < randomNum2 && GameManager.enemyHasSpawned == false)
            {
                // Spawn a new enemy of the given type at the spawn point
                Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);

                GameManager.enemyHasSpawned = true;
            }

            // Wait for 5 seconds before trying to spawn another enemy
            yield return new WaitForSeconds(5f);
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

        // If this is a "Cyst" enemy and it collided with "Anasthetic"
        else if (gameObject.name == "Cyst" && collision.gameObject.name == "Anasthetic(Clone)")
        {
            // Decrease the enemy's health by 1
            health -= 1;
        }
    }

}
