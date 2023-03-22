using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Array of enemy prefabs to spawn
    public Enemy[] prefabs;

    // The current enemy prefab to spawn
    Enemy enemyToSpawn;

    // The current health of the enemy
    int health;

    // The spawn point for enemies
    Vector2 spawnPoint;

    void Start()
    {
        // Set the spawn point for enemies to be just below the spawner object
        spawnPoint = new Vector2(transform.position.x, transform.position.y - 1);

        // If this spawner is for the "Cyst" enemy type
        if (gameObject.name == "Cyst")
        {
            // Set the initial health to 5
            health = 5;

            // Set the enemy prefab to be the first one in the array
            enemyToSpawn = prefabs[0];

            // Start spawning enemies of this type
            StartCoroutine(Spawn(0));

        }

        // If this spawner is for the "Infection" enemy type
        if (gameObject.name == "Infection")
        {
            // Set the initial health to 5
            health = 5;

            // Set the enemy prefab to be the second one in the array
            enemyToSpawn = prefabs[1];

            // Start spawning enemies of this type
            StartCoroutine(Spawn(1));
        }
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
    IEnumerator Spawn(int type)
    {
        // Loop indefinitely
        while (true)
        {
            // Generate a random number between 0 and 8
            int randomNum2 = Random.Range(0, 8);

            // If the enemy's health is less than the random number
            if (health < randomNum2)
            {
                // Spawn a new enemy of the given type at the spawn point
                Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);

                // Increase the enemy's health by 1
                health++;
            }

            // Wait for 5 seconds before spawning another enemy
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
