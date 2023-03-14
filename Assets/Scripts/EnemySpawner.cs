using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] prefabs;
    Enemy enemyToSpawn;
    int health;
    Vector2 spawnPoint;

    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y - 1);
        if(gameObject.name == "Cyst")
        {
            health = 5;
            enemyToSpawn = prefabs[0];
            StartCoroutine(Spawn(0));
            
        }
        if(gameObject.name == "Infection")
        {
            health = 5;
            enemyToSpawn = prefabs[1];
            StartCoroutine(Spawn(1));
        }
    }
    private void Update()
    {
        if (health <= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                 Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
    IEnumerator Spawn(int type)
    {
        while (true)
        {
            int randomNum2 = Random.Range(0, 8);
            if (health < randomNum2)
            {
                Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
                health++;
            }
            yield return new WaitForSeconds(5f);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Gastro Liquid(Clone)")
        {
            health -= 1;
        }
        else if(gameObject.name == "Infection" && collision.gameObject.name == "Antibiotic(Clone)")
        {
            health -= 1;
        }
        else if (gameObject.name == "Cyst" && collision.gameObject.name == "Anasthetic(Clone)")
        {
            health -= 1;
        }
    }

}
