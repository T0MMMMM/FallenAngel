using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject model;


    // Movement //
    public bool blockMovement = false;
    // Movement //



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


    // Wall jump //
    public float timeAfterJump;
    public bool isWallJumping;
    public bool hasWallJumping;
    public bool isWallSliding;
    public float wallJumpingTime = 1f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 2f;
     Vector3 wallJumpingPower = new Vector3(12f, 16f, 0f);
    // Wall jump //



    // *** Wall End *** //

    // Double Jump //
    private int doubleJump = 0;
    private int maxJumpNumber = 0;
    public float jumpForce = 10f;
    private float jumpTimeCounter;
    public float jumpTime = 0.1f;
    private bool isJumping;
    // Double Jump End //

    // Dash //
    private bool dashing = false;
    private bool dashUnlock = false;
    private bool canDash = false;
    private float dashTime = 0.2f;
    private float dashPower = 30f;
    public Vector2 dashDirection = new Vector2(0f, 0f);

    //private float dashCooldown = 5f;
    // Dash End //

    // Gravity //
    public Vector3 normalGravity = new Vector3(0, -42f, 0);
     // Gravity //


    // Character //
    public float movementSpeed = 20f;
    public float direction = 1f;
    public bool canSave = false;
    //public ParticleSystem saveParticles;
    [SerializeField] private Animator savingAnimation;
    //[SerializeField] private Animator _anim;
    public bool isPaused = false;
    // Character End //

    public GameObject savingText;
    public GameObject PausedPanel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.position = new Vector3(SaveManager.instance.position_x, SaveManager.instance.position_y, 0);
        Physics.gravity = normalGravity;
        savingAnimation = savingAnimation.GetComponent<Animator>();
        Debug.Log(savingAnimation);
        //saveParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) {
            isPaused = true;
            PausedPanel.SetActive(true);
            Time.timeScale = 0;
        }

        Movement();
        CollisionWall();
        JumpSimple();
        JumpLong();
        JumpWall();
        DashUnlock();
  


        if (canSave)
        {
            Save();
        }

    }
    

    void OnDrawGizmos()
    {   
        if (isOnWall) {
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSizeGround);
        Gizmos.DrawCube(transform.position - transform.right * spacing, boxSizeWall*2);
        Gizmos.DrawCube(transform.position + transform.right * spacing, boxSizeWall*2);


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
    
    //timer

    private float timer = 5;

    //timer end
    private void Save()
    {
        
        SaveManager.instance.position_x = rb.position.x;
        SaveManager.instance.position_y = rb.position.y;

        

        if (Input.GetKeyDown(KeyCode.Q) && !savingAnimation.GetBool("isSaving"))
        {
            savingAnimation.SetBool("isSaving", true);
        }
        if (Input.GetKey(KeyCode.Q) || (timer < 2 && timer > 0))
        {
            timer -= Time.deltaTime;
        }
        if (timer < 2)
        {
            SaveManager.instance.Save();
        }
        if (timer <= 0)
        {
            timer = 5;
            canSave = false;
            savingAnimation.SetBool("isSaving", false);
        }

        if (Input.GetKeyUp(KeyCode.Q) && !(timer < 2))
        {
            savingAnimation.SetBool("isSaving", false);
            timer = 5;
        }
    }
    void Movement() {
        // MOUVEMENTS //
        if (isGrounded) {
            doubleJump = maxJumpNumber;
            if ( dashUnlock ) {canDash = true;}
        }
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        if ((!isWallJumping || horizontalInput != 0) && !blockMovement) {
            rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, 0f);
            if ((horizontalInput < 0 || horizontalInput > 0) && isWallJumping) {
                wallJumpingCounter = 0f;
                //rb.velocity = (0f, rb.velocity.y, 0f);
                isWallJumping = false;
            }
        }
        // DIRECTION //
        


        if ((horizontalInput > 0 && !blockMovement)) {
            direction = 1;
            model.transform.eulerAngles = new Vector3(0, 180, 0);
        } else if ((horizontalInput < 0 && !blockMovement)) {
            direction = -1;
            model.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        // DASH DIRECTION //
        if (!dashing) {
            if (horizontalInput == 0 && verticalInput == 0) {
                dashDirection = new Vector2(direction, 0);
            } else {
                dashDirection = new Vector2(horizontalInput, verticalInput);
            }
        }
        

    }
    
    void CollisionWall() {
        isGrounded = Physics.BoxCast(transform.position, boxSizeGround, -transform.up, transform.rotation, maxDistance, layerMask);
        isOnWallLeft = Physics.BoxCast(transform.position, boxSizeWall, -transform.right, transform.rotation, spacing, layerMask);
        isOnWallRight = Physics.BoxCast(transform.position, boxSizeWall, transform.right, transform.rotation, spacing, layerMask);
        isOnWall = isOnWallLeft || isOnWallRight;
        if (isOnWall && rb.velocity.y < 0) 
        {   
            rb.velocity = new Vector3(rb.velocity.x, -3, rb.velocity.z);
        }
    }


    void JumpSimple() {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || (doubleJump >= 1 && !isOnWall)))
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
                isJumping = true;
                jumpTimeCounter = jumpTime;
                doubleJump -= 1;
            }

        if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }

    }

    void JumpLong() {
        if (Input.GetKey(KeyCode.Space) && isJumping) {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }
    }

    void StopWallJumping() {
        isWallJumping = false;
    }

    void JumpWall() {
        /**
        if (isWallJumping) {timeAfterJump += Time.deltaTime;}

        if (Input.GetKeyDown(KeyCode.Space) && isOnWall || (timeAfterJump < 0.4 && timeAfterJump != 0 && isWallJumping)) {
                blockMovement = true;
        } else {blockMovement = false;}

        if (isOnWall) {timeAfterJump = 0;}

        if (isGrounded) {
            timeAfterJump = 0;
            isWallJumping = false;}

        if (Input.GetKeyDown(KeyCode.Space) && isOnWall && !isGrounded) { 
            isWallJumping = true;
            if (isOnWallLeft && direction == -1) {
                direction = -direction;
                model.transform.eulerAngles = new Vector3(0, 180, 0);
            } else if (isOnWallRight && direction == 1) {
                direction = -direction;
                model.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            rb.velocity = new Vector3(direction * wallJumpingPower.x, wallJumpingPower.y, 0);
        }
        */
        
        

        
        timeAfterJump += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && isOnWall || (timeAfterJump < 0.3 && timeAfterJump != 0 && isWallJumping)) {
                blockMovement = true;
        } else {
             blockMovement = false;
        } 
        if (isOnWall && isWallJumping && timeAfterJump > 0.1) {
            isWallJumping = false;
        } else if (isOnWall && !isGrounded) {
            timeAfterJump = 0;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        } else if (isGrounded) {
            wallJumpingCounter = 0;
            timeAfterJump = 0;
            isWallJumping = false;
        } else {
            wallJumpingCounter -= Time.deltaTime;
        }
    
        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f) {
            if (timeAfterJump > 0.11) {
                isWallJumping = true;
            }
            if (isOnWallLeft && direction == -1) {
                direction = -direction;
                model.transform.eulerAngles = new Vector3(0, 180, 0);
            } else if (isOnWallRight && direction == 1) {
                direction = -direction;
                model.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            isWallJumping = true;
            rb.velocity = new Vector3(direction * wallJumpingPower.x, wallJumpingPower.y, 0);
            wallJumpingCounter = 0f;
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
        
    }

    private IEnumerator Dash() {
        dashing = true;
        canDash = false;
        float startTime = Time.time;
        wallJumpingCounter = 0f;
        isWallJumping = false;
        rb.velocity = new Vector3(0f, 0f, 0f);
        timeAfterJump = 0;     

        while(Time.time < startTime + dashTime)
        {
            rb.velocity = new Vector3(dashDirection.x * dashPower,  dashDirection.y * dashPower, 0f);

            yield return null;
        }
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y/2, 0f);
        dashDirection = new Vector2(0f, 0f);
        dashing = false; 
        
    }

    void DashUnlock() {
        if (canDash && Input.GetButtonDown("Dash")) {
            StartCoroutine(Dash());
        }
    }

}