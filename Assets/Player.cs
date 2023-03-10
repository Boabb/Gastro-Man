using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject firePoint, playerGFX;
    [SerializeField] private float movementSpeed = 15f, jumpForce = 10f, fallMultiplier = 2.5f ,lowJumpMultipler = 2f;
    Rigidbody2D body;
    Animator animator;
    Weapon weapon;
    Camera sceneCamera;
    
    Vector2 moveDirection, mousePosition;
    bool jumping, canjump;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<Weapon>();
        animator = GetComponentInChildren<Animator>();
        sceneCamera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        StartCoroutine(PlayerShooting());
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        moveDirection = new Vector2(moveX,0).normalized;
        mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetKeyDown(KeyCode.Space) && canjump)
        {
            Jump();
        }

        if(body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultipler - 1) * Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        body.velocity = new Vector2((moveDirection.x * movementSpeed), body.velocity.y);

        Vector2 aimDirection = mousePosition - body.position;

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (aimAngle >= -90 && aimAngle <= 90) 
        {
            playerGFX.transform.localScale = firePoint.transform.localScale = new Vector3(1f, 1f, 1f);
            aimAngle = Mathf.Clamp(aimAngle, -30, 40);
        }
        else if (aimAngle >= 90 && aimAngle < 180)
        {
            playerGFX.transform.localScale = firePoint.transform.localScale = new Vector3(-1f, 1f, 1f);
            aimAngle = Mathf.Clamp(aimAngle, 140, 180);
            aimAngle -= 180;
        }
        else
        {
            playerGFX.transform.localScale = firePoint.transform.localScale = new Vector3(-1f, 1f, 1f);
            aimAngle = Mathf.Clamp(aimAngle, -180, -150);
            aimAngle -= 180;
        }
        firePoint.transform.rotation = Quaternion.Euler(firePoint.transform.rotation.x, firePoint.transform.rotation.y, aimAngle);

        animator.SetBool("isJumping", jumping);
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
        body.velocity = new Vector3(body.velocity.x, jumpForce);
        canjump = false;
        jumping = true;
    }

    void OnMedicinePickup(GameObject pickup)
    {
        if (pickup.name == "Gastro Pool")
        {
            weapon.ChangeAmmoType("Gastro Liquid");
        }
        else if (pickup.name == "Antibiotics")
        {
            weapon.ChangeAmmoType("Antibiotic");
        }
        else if (pickup.name == "Anesthesia")
        {
            weapon.ChangeAmmoType("Anasthetic");
        }
        weapon.AddAmmo(weapon.maxTank);
        Destroy(pickup);
        Debug.Log($"You picked up: {pickup.name}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" && body.velocity.y == 0 || collision.gameObject.tag == "Problem" && body.velocity.y == 0)
        {
            canjump = true;
            jumping = false;
        }

        if (collision.gameObject.tag == "Medicine")
        {
            OnMedicinePickup(collision.gameObject);
        }

        if (collision.gameObject.tag == "Finish")
        {
            GameManager.PlayerExit?.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && body.velocity.y != 0)
        {
            canjump = false;
        }
    }
}
