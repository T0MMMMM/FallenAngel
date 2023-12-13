using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject model;

    // Ground //
    private bool isGrounded = false;
    public Vector3 boxSizeGround;
    public float maxDistance;
    public LayerMask layerMask;
    // Ground End //

    // Wall //
    private bool isOnWall = false;
    private bool isOnWallLeft = false;
    private bool isOnWallRight = false;
    // pas encore fait ///// private bool sameWallDirection = false;
    public Vector3 boxSizeWall;
    public float spacing;
    // Wall End //

    // Double Jump //
    private int doubleJump = 0;
    private int maxJumpNumber = 0;
    public float jumpForce = 10f;
    private float jumpTimeCounter;
    public float jumpTime = 0.35f;
    private bool isJumping;
    // Double Jump End //

    // Dash //
    private bool dashUnlock = false;
    private bool canDash = false;
    private float dashTime = 0.15f;
    private float dashPower = 50f;
    //private float dashCooldown = 5f;
    // Dash End //

    // Gravity //
    public Vector3 normalGravity = new Vector3(0, -42f, 0);
    public Vector3 frictionGravity = new Vector3(0, -0f, 0);
    
     // Gravity //


    // Character //
    public float movementSpeed = 20f;
    float moveDir = 1f;
    // Character End //

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = normalGravity;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.BoxCast(transform.position, boxSizeGround, -transform.up, transform.rotation, maxDistance, layerMask);
        isOnWallLeft = Physics.BoxCast(transform.position, boxSizeWall, -transform.right, transform.rotation, spacing, layerMask);
        isOnWallRight = Physics.BoxCast(transform.position, boxSizeWall, transform.right, transform.rotation, spacing, layerMask);


        if (isGrounded) {
            doubleJump = maxJumpNumber;
            if ( dashUnlock ) {canDash = true;}
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed);


        // *** WALLS *** //

        // CONTRE UN MUR //
        if (isOnWallLeft || isOnWallRight) {
            isOnWall = true;
        } else {
            isOnWall = false;
        }
        // CONTRE UN MUR //


        // RALENTISSEMENT CONTRE UN MUR //
        if (isOnWall && rb.velocity.y < 1) 
        {   
            rb.velocity = new Vector3(rb.velocity.x, -2, rb.velocity.z);
            Physics.gravity = frictionGravity;
        } else {
            Physics.gravity = normalGravity;
        }
        // RALENTISSEMENT CONTRE UN MUR //


        // *** WALLS *** //





        // DIRECTION END //
        if (horizontalInput > 0) {
            if (canDash) {moveDir = 1;}
            model.transform.eulerAngles = new Vector3(0, 180, 0);
        } else if (horizontalInput < 0) {
            if (canDash) {moveDir = -1;}
            model.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        // DIRECTION END //




        // *** JUMP *** //


        // SIMPLE/DOUBLE JUMP //
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || (doubleJump >= 1 && !isOnWall)))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            doubleJump -= 1;
        }
        // SIMPLE/DOUBLE JUMP //

        // LONG JUMP //
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }
        // LONG JUMP //


        // WALL JUMP //






        // WALL JUMP //



        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        // *** JUMP *** //





        // DASH //
        if (canDash && Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
        }
        // DASH END //

    }

    void OnDrawGizmos()
    {   
        if (isOnWall) {
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSizeGround);
        Gizmos.DrawCube(transform.position - transform.right * spacing, boxSizeWall);
        Gizmos.DrawCube(transform.position + transform.right * spacing, boxSizeWall);


    }


    private IEnumerator Dash() {
        canDash = false;
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            rb.velocity = new Vector3(moveDir * dashPower, 0f, 0f);

            yield return null;
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
            dashUnlock = true;
            Destroy(other.gameObject);
        }
    }

}
