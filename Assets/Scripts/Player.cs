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
        // assigns the weapon attached to the player to a variable
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

        // sets the position of the mouse of the screen to a Vector2 variable
        mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);

        // loads the Jump() method if space is pressed and the player is allowed to jump
        if(Input.GetKeyDown(KeyCode.Space) && canjump)
        {
            Jump();
        }
    }
    private void LateUpdate()
    {
        // moves the player in the direction pressed scaled by a public speed variable 
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

        // rotates the weapon around the player and points it towards the moust location
        firePoint.transform.rotation = Quaternion.Euler(firePoint.transform.rotation.x, firePoint.transform.rotation.y, aimAngle);

        // set jump/not jumping animation
        animator.SetBool("isJumping", jumping);
    }

    IEnumerator PlayerShooting()
    {
        while(true)
        {
            // checks if the player has pressed left click
            if (Input.GetMouseButton(0))
            {
                // loads the Fire() method in the weapon script
                weapon.Fire();

                // controls cooldown between shots fired
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

        // faster falling
        if (body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // peak height of jump decreased if the player taps space
        else if (body.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultipler - 1) * Time.deltaTime;
        }
    }

    void OnMedicinePickup(GameObject pickup)
    {
        // changes the ammo type to the respective ammo pool the player has collided with
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
        GameManager.PlaySound("reload");
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && body.velocity.y == 0 || collision.gameObject.tag == "Problem" && body.velocity.y == 0)
        {
            canjump = true;
            jumping = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // disallows the player to jump if they are already in the air
        if (collision.gameObject.tag == "Ground" && body.velocity.y != 0)
        {
            canjump = false;
        }
    }
}
