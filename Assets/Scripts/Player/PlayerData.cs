using UnityEngine;

[CreateAssetMenu(menuName = "Player Data", fileName = "Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    public Vector3 normalGravity = new Vector3(0, -42f, 0);

    [Header("Ground Check Collision")]
    [HideInInspector] public bool isGrounded = false;
    public Vector3 boxSizeGround;
    public float maxDistance;
    public LayerMask layerMask;


    [Header("Wall Check Collision")]
    [HideInInspector] public bool isOnWall = false;
    [HideInInspector] public bool isOnWallLeft = false;
    [HideInInspector] public bool isOnWallRight = false;
    public Vector3 boxSizeWall;
    public float spacing;


    [Header("Wall Jump")]
    public float timeAfterJump;
    [HideInInspector] public bool blockMovement = false;
    [HideInInspector] public bool isWallJumping;
    [HideInInspector] public bool pressWallJump;
    [HideInInspector] public bool hasWallJumping;
    [HideInInspector] public bool isWallSliding;
    [HideInInspector] public float wallJumpingTime = 1f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 2f;
    public Vector3 wallJumpingPower = new Vector3(12f, 16f, 0f);


    [Header("Jump")]
    [HideInInspector] public int jumpNumber = 1;
    [HideInInspector] public int maxJumpNumber = 1;
    public float jumpForce = 10f;
    [HideInInspector] public float jumpTimeCounter;
    public float jumpTime = 0.1f;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool simpleJump;
    [HideInInspector] public bool pressJump;
    [HideInInspector] public bool holdJump;
    [HideInInspector] public bool relasedJump;


    [Header("Dash")]
    [HideInInspector] public bool pressDash = false;
    [HideInInspector] public bool dashing = false;
    [HideInInspector] public bool dashUnlock = false;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool endDashColdown = true;

    public float dashTime = 0.2f;
    public Vector2 dashPower = new Vector2(30f, 20f);
    public float dashColdown = 2f;
    public Vector2 dashDirection = new Vector2(0f, 0f);


    [Header("Character")]
    public GameObject model;
    public float movementSpeed = 20f;
    [HideInInspector] public float direction = 1f;
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public bool canSave = false;
    [HideInInspector] public bool isPaused = false;




    [Header("Save")]
    public GameObject savingText;
    public GameObject pausedPanel;
    public float savingTimer = 5f;
    [HideInInspector] public bool pressSave = false;
    [HideInInspector] public bool holdSave = false;
    [HideInInspector] public bool relasedSave = false;
    

    [Header("Animation")]
    public Animator savingAnimation;
    //public Animator _anim;
}
