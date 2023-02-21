using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
/*    [SerializeField] public Enemy enemyPrefab;*/

    void Start()
    {
        StartCoroutine(Spawn());
    }
    // Update is called once per frame
    IEnumerator Spawn()
    {
        while (true)
        {
            int randomX = Random.Range(-10, 13);
            Vector3 spawnPoint = new Vector3(randomX, 6, 0);

            /*        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);*/
            Enemy newEnemy = Instantiate(Resources.Load("Enemy", typeof(GameObject)), spawnPoint, Quaternion.identity) as Enemy;
            yield return new WaitForSeconds(10f);
        }
    }
}
