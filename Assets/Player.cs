using System.Collections;

using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] public GameObject firePoint;
    [SerializeField] public Camera sceneCamera;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float movementSpeed = 15f;
    [SerializeField] public float jumpForce = 10f;
    private Rigidbody2D body;
    bool onground = true;
    Vector2 moveDirection;
    Vector2 mousePosition;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        StartCoroutine(PlayerShooting());
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


        if (Input.GetKey(KeyCode.R))
        {
            weapon.Reload();
        }

    }
    private void LateUpdate()
    {
        body.velocity = new Vector2((moveDirection.x * movementSpeed), body.velocity.y);

        Vector2 aimDirection = mousePosition - body.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    IEnumerator PlayerShooting()
    {
        while(true)
        {
            if (Input.GetMouseButton(0))
            {
                weapon.Fire();
                yield return new WaitForSeconds(0.03f);
            }
            yield return null;
        }
    }
    
    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        onground = false;
    }   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            onground = true;
        }

        if (collision.gameObject.tag == "Medicine")
        {
            weapon.AddAmmo(weapon.maxTank);
            Destroy(collision.gameObject);
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
