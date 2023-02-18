using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Transform firePoint;
    [SerializeField] public Camera sceneCamera;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float movementSpeed;
    private Rigidbody2D body;
    bool onground = true;
    bool canShoot = true;
    Vector2 moveDirection;
    Vector2 mousePosition;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        
        moveDirection = new Vector2(moveX,0).normalized;
        mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetKey(KeyCode.Space) && onground)
        {
            Jump();
        }

        if (Input.GetMouseButton(0) && canShoot)
        {
            weapon.Fire();
            StartCoroutine(ShootCooldown());
        }

    }
    private void FixedUpdate()
    {
        body.velocity = new Vector2((moveDirection.x * movementSpeed), body.velocity.y);

        Vector2 aimDirection = mousePosition - body.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);
        firePoint.rotation = aimAngle;
    }
    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }

    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, (movementSpeed*2));
        onground = false;
    }   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            onground = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onground = false;
        }
    }
}
