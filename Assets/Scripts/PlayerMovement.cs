using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private bool isGrounded = false;

    // Double Jump //
    private int doubleJump = 0;
    private int maxJumpNumber = 0;
    // Double Jump End //

    // Dash //
    private bool canDash = false;
    private float dashTime = 0.4f;
    private float dashPower = 500f;
    //private float dashCooldown = 0.75f;
    // Dash End //

    // Character //
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 50f;
    float moveDir = 1f;
    // Character End //

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -13f, 0);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        if (isGrounded) {
            doubleJump = maxJumpNumber;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed);

        if (horizontalInput > 0) {
            moveDir = 1;
        } else if (horizontalInput < 0) {
            moveDir = -1;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            } else if (doubleJump >= 1) {
                doubleJump -= 1;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
            
        }
        
        if (canDash && Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
        }

    }
    private IEnumerator Dash() {
        canDash = false;
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            rb.velocity = new Vector3(moveDir * dashPower * Time.deltaTime, rb.velocity.y, 0f);

            yield return null;
        }
        canDash = true;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody == null)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.rigidbody == null)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Double Jump Power")
        {
            maxJumpNumber += 1;
            Destroy(other.gameObject);
        }
        if (other.name == "Dash Power")
        {
            canDash = true;
            Destroy(other.gameObject);
        }
    }

}
