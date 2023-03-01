using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int problemsEliminated = 0;
    public GameObject[] problems;

    void Start()
    {
        problems = GameObject.FindGameObjectsWithTag("Problem");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {

        }
        else if (collision.gameObject.tag == "Problem")
        {
            problemsEliminated++;
            if (problemsEliminated == problems.Length)
            {
                GameManager.ProblemsCleared?.Invoke();
            }
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
