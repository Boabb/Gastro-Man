using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Array of enemy prefabs to spawn
    public Enemy[] prefabs;

    // The current enemy prefab to spawn
    Enemy enemyToSpawn;
    
    // Number values for the current health of this enemy spawner and the enemySpawnCooldown
    public int spawnerHealth, burstAmount, enemyHealth;
    public float enemySpawnCooldown = 7.5f;

    // The spawn point for enemies
    Vector3 spawnPoint;

    // A bool that stops multiple enemy spawns in a set period
    public static bool enemyCanSpawn;

    void Start()
    {
        // Set the spawn point for enemies to be just below the spawner object
        spawnPoint = new Vector2(transform.position.x, transform.position.y - 1);

        // An enemy can spawn
        enemyCanSpawn = true;

        // If this spawner is the "Ulcer" problem
        if (gameObject.name == "Ulcer")
        {
            // Set the enemy to be spawned to be the "Bacteria" enemy type
            enemyToSpawn = prefabs[0];
        }

        // If this spawner is the "Infection" problem
        if (gameObject.name == "Infection")
        {
            // Set the enemy to be spawned to be the "Virus" enemy type
            enemyToSpawn = prefabs[1];
        }
        if(gameObject.name == "Streptococcus")
        {
            // Set the enemy to be spawned to be the "Germ" enemy type
            enemyToSpawn = prefabs[2];
        }
        // Set health of the enemy 
        enemyToSpawn.health = enemyToSpawn.maxHealth = enemyHealth;

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

            // Only spawn an enemy if one hasn't spawned already and theres less than 5 enemies alive and the player has moved from their spawn location
            if (enemyCanSpawn && GameManager.enemiesAlive.Length < 5 && GameManager.playerHasMoved)
            {
                // Quite a likely outcome but allows the spawner that the enemy comes from to be random
                if (randomNum < spawnerHealth)
                {

                    // Spawn a new enemy of the given type at the spawn point and thus an enemy has spawned
                    Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
                    
                    enemyCanSpawn = false;            
                    
                    // Wait a set period before trying to spawn another enemy
                    yield return new WaitForSeconds(enemySpawnCooldown);
                    enemyCanSpawn = true;
                }
            }
            yield return null;
        }
    }

    // Called when the enemy collides with another object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If this spawner collided with "Gastro Liquid"
        if (collision.gameObject.name == "Gastro Liquid(Clone)")
        {
            // Decrease the enemy's health by 1
            spawnerHealth -= 1;
        }
        // If this is an "Infection" and it collided with "Antibiotic"
        else if (gameObject.name == "Infection" && collision.gameObject.name == "Antibiotic(Clone)")
        {
            // Decrease the enemy's health by 1
            spawnerHealth -= 1;
        }
        // If this is an "Ulcer" and it collided with "Antacid"
        else if (gameObject.name == "Ulcer" && collision.gameObject.name == "Antacid(Clone)")
        {
            // Decrease the enemy's health by 1
            spawnerHealth -= 1;
        }
        // If this is a "Streptococcus" and it collided with "Penicillin"
        else if (gameObject.name == "Streptococcus" && collision.gameObject.name == "Penicillin(Clone)")
        {
            // Decrease the enemy's health by 1
            spawnerHealth -= 1;
        }
        // If the enemy's health has reached 0 or below
        if (spawnerHealth <= 0)
        {
            // Spawn 3 new enemies of the same type at the same location
            for (int i = 0; i < burstAmount; i++)
            {
                Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
            }

            // Destroy this enemy object
            Destroy(gameObject);
        }
    }
}
