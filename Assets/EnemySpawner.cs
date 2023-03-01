using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefab;

    void Start()
    {
        StartCoroutine(Spawn(0));
    }
    // Update is called once per frame
    IEnumerator Spawn(int type)
    {
        while (true)
        {
            Vector2 spawnPoint = new Vector2(transform.position.x,transform.position.y - 2);
            Enemy enemyToSpawn = enemyPrefab[type];
            Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(10f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            GameManager.score += 300;
            for (int i = 0; i <= 3; i++)
            {
                Instantiate(enemyPrefab[0], transform.position, Quaternion.identity);
            }
            gameObject.SetActive(false);
        }
    }

}
