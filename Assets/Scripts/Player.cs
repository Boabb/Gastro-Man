using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // reference to the player's fire point and graphics (used for weapon aiming)
    [SerializeField] public GameObject firePoint, playerGFX;

    // movement and jump related variables that can be tweaked in the Unity Editor
    [SerializeField] float movementSpeed = 15f, jumpForce = 10f, fallMultiplier = 5f;

    // reference to the player's rigidbody, animator, weapon, and scene camera
    Rigidbody2D body => GetComponent<Rigidbody2D>();
    Animator animator => GetComponentInChildren<Animator>();
    Weapon weapon => GetComponentInChildren<Weapon>();
    Camera sceneCamera => FindObjectOfType<Camera>();

    // direction in which the player is moving and the position of the mouse on screen
    Vector2 moveDirection, mousePosition;

    // flags for whether the player is currently jumping and if they are able to jump
    public static bool canjump = true, canMove = true;

    void Start()
    {
        // start shooting coroutine to handle player shooting
        StartCoroutine(PlayerShooting());
    }

    void Update()
    {
        // retrieve the player's horizontal movement input and normalize it
        float moveX = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector2(moveX, 0).normalized;

        // set the position of the mouse on screen to a Vector2 variable
        mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    // allows all other actions to take place before considering movement
    void LateUpdate()
    {
        // checks if the player is allowed to move (gameover/victory/pause screens)
        if (canMove)
        {
            PlayerMovement();
        }
    }

    void PlayerMovement()
    {
        // move the player horizontally according to the input direction and speed
        body.velocity = new Vector2((moveDirection.x * movementSpeed), body.velocity.y);

        // calculate the direction the weapon should point based on the mouse's position
        Vector2 aimDirection = mousePosition - body.position;

        // calculate the angle the weapon should be at based on the aim direction
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (aimAngle == 180 || aimAngle == -180 || aimAngle == 0)
        {
            aimAngle = 0;
        }
        else if (aimAngle >= -90 && aimAngle <= 90)
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

        // load the Jump() method if the player presses space and is allowed to jump
        if (Input.GetKeyDown(KeyCode.Space) && canjump)
        {
            Jump();
        }

        // set walk/not walking animation
        SetAnimation();
        
    }

    void SetAnimation()
    {
        if (body.velocity.x > 0.5f && body.velocity.y <= 0.5f || body.velocity.x > - 0.5f && body.velocity.y <= 0.5f)
        {
            animator.SetBool("isWalking", true);
        }
    }

    IEnumerator PlayerShooting()
    {
        while(true)
        {
            // checks if the player has pressed left click
            if (Input.GetMouseButton(0) && canMove)
            {
                // loads the Fire() method in the weapon script
                weapon.Fire();

                // controls cooldown between shots fired
                yield return new WaitForSeconds(0.08f);
            }
            yield return null;
        }
    }
    
    void Jump()
    {
        // add jump force value to y velocity
        body.velocity = new Vector3(body.velocity.x, jumpForce);

        // disable walking animation
        animator.SetBool("isJumping", true);

        // disallow the player from jumping twice
        canjump = false;

        // faster falling
        if (body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
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
        else if (pickup.name == "Antacids")
        {
            weapon.ChangeAmmoType("Antacid");
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" && body.velocity.y == 0 || collision.gameObject.tag == "Problem" && body.velocity.y == 0)
        {
            canjump = true;
            animator.SetBool("isJumping", false);
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

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && body.velocity.y == 0 || collision.gameObject.tag == "Problem" && body.velocity.y == 0)
        {
            canjump = true;
            animator.SetBool("isJumping", false);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        // disallows the player to jump if they are already in the air
        if (collision.gameObject.tag == "Ground" && body.velocity.y != 0)
        {
            animator.SetBool("isJumping", true);
            canjump = false;
        }
    }
}
