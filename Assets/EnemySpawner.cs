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
        if(gameObject.name == "Cyst")
        {
            health = 5;
            StartCoroutine(Spawn(0));
        }
        if(gameObject.name == "Infection")
        {
            health = 5;
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
            GameManager.score += 300;
        }
    }
    IEnumerator Spawn(int type)
    {
        yield return new WaitForSeconds(5f);
        while (true)
        {
            spawnPoint = new Vector2(transform.position.x,transform.position.y - 1);
            enemyToSpawn = prefabs[type];
            Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
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
