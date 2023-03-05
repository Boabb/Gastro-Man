using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    int health = 10, maxHealth = 10;
    public float speed = 100f, nextWaypointDistance = 3f;
    public GameObject healthBarUI;
    public Slider healthSlider;
    Transform target, enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;

    Seeker seeker;
    Rigidbody2D rb;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemyGFX = GetComponentInChildren<SpriteRenderer>().transform;
        target = FindObjectOfType<Player>().transform;

        InvokeRepeating("UpdatePath", 0f, .5f);
        speed = Random.Range(200f, 450f);
        healthSlider.value = CalculateHealth();
    }

    private void Update()
    {
        CheckHealth();
        Pathing();
    }

    private void LateUpdate()
    {
        healthSlider.value = CalculateHealth();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            health -= 1;
        }

        else if(collision.gameObject.tag == "Player")
        {
            GameManager.PlayerDeath?.Invoke();
            collision.gameObject.SetActive(false);
        }
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.EnemyDeath?.Invoke();
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
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= 0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
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
}
