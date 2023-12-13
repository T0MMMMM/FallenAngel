using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject model;

    // Ground //
    private bool isGrounded = false;
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    // Ground End //

    // Double Jump //
    private int doubleJump = 0;
    private int maxJumpNumber = 0;
    public float jumpForce = 20f;
    private float jumpTimeCounter;
    public float jumpTime = 0.35f;
    private bool isJumping;
    // Double Jump End //

    // Dash //
    private bool canDash = false;
    private float dashTime = 0.4f;
    private float dashPower = 500f;
    //private float dashCooldown = 0.75f;
    // Dash End //

    // Character //
    public float movementSpeed = 20f;
    float moveDir = 1f;
    // Character End //

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -20f, 0);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, layerMask);

        if (isGrounded) {
            doubleJump = maxJumpNumber;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed);

        if (horizontalInput > 0) {
            if (canDash) { moveDir = -1; }
            model.transform.eulerAngles = new Vector3(0, 180, 0);
        } else if (horizontalInput < 0 && canDash) {
            if (canDash) { moveDir = -1; }
            model.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        // JUMP //
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || doubleJump >= 1 ))
        {
            rb.velocity = Vector3.up * jumpForce;
            isJumping = true;
            jumpTimeCounter = jumpTime;
            doubleJump -= 1;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector3.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        // JUMP END //

        // DASH //
        if (canDash && Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
        }
        // DASH END //

    }

    void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawCube(transform.position-transform.up*maxDistance, boxSize);
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
