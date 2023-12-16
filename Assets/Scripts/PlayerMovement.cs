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

    // *** Wall *** //
    private bool isOnWall = false;
    private bool isOnWallLeft = false;
    private bool isOnWallRight = false;
    public Vector3 boxSizeWall;
    public float spacing;

    private bool isWallJumping;
    private bool isWallSliding;
    private float wallJumpingTime = 1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 2f;
    private Vector3 wallJumpingPower = new Vector3(12f, 16f, 0f);




    // *** Wall End *** //

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
    private float dashDir = 1f;
    //private float dashCooldown = 5f;
    // Dash End //

    // Gravity //
    public Vector3 normalGravity = new Vector3(0, -42f, 0);
     // Gravity //


    // Character //
    public float movementSpeed = 20f;
    private float direction = 1;
    private bool canSave = false;
    // Character End //

    public GameObject savingText;
    public GameObject PausedPanel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.position = new Vector3(SaveManager.instance.position_x, SaveManager.instance.position_y, 0);
        Physics.gravity = normalGravity;
    }

    // Update is called once per frame
    void Update()
    {
        
        //////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PausedPanel.SetActive(true);
            Time.timeScale = 0;
        }
        //////////////////////////////////////////

        isGrounded = Physics.BoxCast(transform.position, boxSizeGround, -transform.up, transform.rotation, maxDistance, layerMask);
        isOnWallLeft = Physics.BoxCast(transform.position, boxSizeWall, -transform.right, transform.rotation, spacing, layerMask);
        isOnWallRight = Physics.BoxCast(transform.position, boxSizeWall, transform.right, transform.rotation, spacing, layerMask);


        if (isGrounded) {
            doubleJump = maxJumpNumber;
            if ( dashUnlock ) {canDash = true;}
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");


        if (!isWallJumping) {
            rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed);
        }


        // *** WALLS *** //

        // CONTRE UN MUR //
        isOnWall = isOnWallLeft || isOnWallRight;
        // CONTRE UN MUR //


        // RALENTISSEMENT CONTRE UN MUR //
        if (isOnWall && rb.velocity.y < 0) 
        {   
            rb.velocity = new Vector3(rb.velocity.x, -3, rb.velocity.z);
        }
        // RALENTISSEMENT CONTRE UN MUR //


        // *** WALLS *** //




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
        if (isGrounded) {
            wallJumpingCounter = 0;
        }

        if (isOnWall && !isGrounded) {
            isWallJumping = false;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        } else if (isGrounded) {
            isWallJumping = false;
            wallJumpingCounter = 0;
        } else {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f) {
            if ((isOnWallLeft && direction == -1) || (isOnWallRight && direction == 1)) {
                direction = -direction;
            }
            isWallJumping = true;
            rb.velocity = new Vector3(direction * wallJumpingPower.x, wallJumpingPower.y, 0);
            wallJumpingCounter = 0f;
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }

        // WALL JUMP END //


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




        // DIRECTION //
        if (rb.velocity.x > 0) {
            direction = 1;
            if (canDash) {dashDir = 1;}
            model.transform.eulerAngles = new Vector3(0, 180, 0);
        } else if (rb.velocity.x < 0) {
            direction = -1;
            if (canDash) {dashDir = -1;}
            model.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        // DIRECTION END //

        if (canSave)
        {
            Save();
        }

    }
    void StopWallJumping() {
        isWallJumping = false;
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
            rb.velocity = new Vector3(dashDir * dashPower, 0f, 0f);

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
        if (other.CompareTag("Respawn"))
        {
            canSave = true;
            savingText.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Respawn"))
        {
            canSave = false;
            savingText.SetActive(false);
        }
    }

    private void Save() {

        SaveManager.instance.position_x = rb.position.x;
        SaveManager.instance.position_y = rb.position.y;

        if (Input.GetKeyDown(KeyCode.A)) {
            SaveManager.instance.Save();
            canSave = false;
        }
    }

}
