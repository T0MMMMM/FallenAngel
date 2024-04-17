using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManagerScript : MonoBehaviour
{

    public static PlayerManagerScript instance { get; private set; }

    private string currentState;


    [SerializeField]
    public PlayerInputScript _inputScript;

    [SerializeField]
    public PlayerMovementScript _movementScript;

    [SerializeField]
    public PlayerCollisionScript _collisionScript;

    [SerializeField]
    public RespawnLastCP _respawnLastCP;



    [SerializeField]
    public PlayerData _data;

    [SerializeField]
    public AudioManager _sound;


    #region player

    [System.NonSerialized]
    public Rigidbody _rb;

    #endregion


    void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        _inputScript.enabled = false;
        _movementScript.enabled = false;
        _collisionScript.enabled = false;
        _respawnLastCP.enabled = false;


        _rb = GetComponent<Rigidbody>();
        _rb.position = new Vector3(SaveManager.instance.position_x, SaveManager.instance.position_y, 0);
        Physics.gravity = _data.normalGravity;
        _data.model = GameObject.FindGameObjectWithTag("model");
        _data.pausedPanel = GameObject.FindGameObjectWithTag("pausedMenu");
        _data.pausedPanel.SetActive(false);

        spawn();

    }

    public void spawn()
    {

        _data.dashUnlock = false;
        _data.jumpNumber = 0;
        _data.maxJumpNumber = 0;
        _data.blockMovement = false;
        _data.jumpTimeCounter = 5f;
        _data.isPaused = false;
        _data.canDash = true;
        _data.delayAfterWallJump = 0.6f;
        _data.dashColdown = 0.5f;
        _data.dashPower = new Vector3(30f, 30f, 0f);

        _data.currentHealth = 50; // SaveManager.instance.maxHealth;
        _data.maxHealth = 50; //SaveManager.instance.maxHealth;

        ChangeState("playing");
    }

    public void ChangeState(string newState)
    {
        if (newState != currentState)
        {
            switch (currentState)
            {
                case "playing":
                    _inputScript.enabled = false;
                    _movementScript.enabled = false;
                    _collisionScript.enabled = false;
                    break;
                case "respawnLastCP":
                    _respawnLastCP.enabled = false;
                    break;
                default:
                    break;

            }
            // Change Animation
            currentState = newState;

        }
    }

    void Update()
    {
        switch (currentState)
        {
            case "playing":
                _inputScript.enabled = true;
                _movementScript.enabled = true;
                _collisionScript.enabled = true;
                break;
            case "respawnLastCP":
                _respawnLastCP.enabled= true;
                break;

        }



        if (Input.GetKeyDown(KeyCode.J)) 
        {
            _collisionScript.hit(10);
            transform.position = new Vector3(GameManager.instance.lastCheckPointPos.x, GameManager.instance.lastCheckPointPos.y, 0);
        }
    }


}
