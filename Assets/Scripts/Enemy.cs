using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    int health = 5, maxHealth = 5, currentWaypoint = 0;
    public float speed = 100f, nextWaypointDistance = 3f, gracePeriod = .5f;
    public GameObject healthBarUI;
    public Slider healthSlider;
    public Sprite[] enemyGFX;
    Transform target;
    SpriteRenderer currentGFX;

    Path path;
    

    Seeker seeker;
    Rigidbody2D rb;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        currentGFX = GetComponentInChildren<SpriteRenderer>();
        target = FindObjectOfType<Player>().transform;

        InvokeRepeating("UpdatePath", gracePeriod, .5f);
        speed = Random.Range(50, 150);
        healthSlider.value = CalculateHealth();
    }

    private void Update()
    {
        CheckHealth();
        Pathing();
        UpdateDirection();
    }

    private void LateUpdate()
    {
        healthSlider.value = CalculateHealth();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Gastro Liquid(Clone)")
        {
            GameManager.PlaySound("enemyhit");
            Destroy(collision.gameObject);
            health -= 1;
        }
        if (collision.gameObject.name == "Antibiotic(Clone)" && gameObject.tag == "Virus")
        {
            GameManager.PlaySound("enemyhit");
            Destroy(collision.gameObject);
            health -= 1;
        }
        else if (collision.gameObject.name == "Anasthetic(Clone)" && gameObject.tag == "Bacteria")
        {
            GameManager.PlaySound("enemyhit");
            Destroy(collision.gameObject);
            health -= 1;
        }
        else if(collision.gameObject.tag == "Player")
        {
            GameManager.PlaySound("death");
            GameManager.PlayerDeath?.Invoke();
            collision.gameObject.SetActive(false);
        }
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            GameManager.PlaySound("enemydeath");
            Destroy(gameObject);
        }
    }

    float CalculateHealth()
    {
        return (float)health / maxHealth;
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void Pathing()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        { 
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdateDirection()
    {
        if (rb.velocity.x >= 0.01f)
        {
            currentGFX.sprite = enemyGFX[2];
        }
        else if (rb.velocity.x <= -0.01f)
        {
            currentGFX.sprite = enemyGFX[0];
        }
        else
        {
            currentGFX.sprite = enemyGFX[1];
        }
    }
}
