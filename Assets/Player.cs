using System.Collections;

using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject firePoint, playerGFX;
    [SerializeField] public Camera sceneCamera;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float movementSpeed = 15f, jumpForce = 10f;
    Rigidbody2D body;
    bool onground = true;
    Vector2 moveDirection, mousePosition;

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

        if(Input.GetKeyDown(KeyCode.Space) && onground)
        {
            Jump();
        }
    }
    private void LateUpdate()
    {
        body.velocity = new Vector2((moveDirection.x * movementSpeed), body.velocity.y);

        Vector2 aimDirection = mousePosition - body.position;

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        Debug.Log(aimAngle);
        if (aimAngle >= -90 && aimAngle <= 90) 
        {
            playerGFX.transform.localScale = firePoint.transform.localScale = new Vector3(1f, 1f, 1f);
            aimAngle = Mathf.Clamp(aimAngle, -50, 30);
        }
        else if (aimAngle >= 90 && aimAngle < 180)
        {
            playerGFX.transform.localScale = firePoint.transform.localScale = new Vector3(-1f, 1f, 1f);
            aimAngle = Mathf.Clamp(aimAngle, 150, 180);
            aimAngle -= 180;
        }
        else
        {
            playerGFX.transform.localScale = firePoint.transform.localScale = new Vector3(-1f, 1f, 1f);
            aimAngle = Mathf.Clamp(aimAngle, -180, -90);
            aimAngle -= 180;
        }
        Debug.Log(aimAngle + "iudshfhjds");
        firePoint.transform.rotation = Quaternion.Euler(firePoint.transform.rotation.x, firePoint.transform.rotation.y, aimAngle);
    }

    IEnumerator PlayerShooting()
    {
        while(true)
        {
            if (Input.GetMouseButton(0))
            {
                weapon.Fire();
                yield return new WaitForSeconds(0.06f);
            }
            yield return null;
        }
    }
    
    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
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
        if (collision.gameObject.tag == "Finish")
        {
            GameManager.PlayerExit?.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && onground)
        {
            onground = false;
        }
    }
}
