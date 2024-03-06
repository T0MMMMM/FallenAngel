using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManagerScript : MonoBehaviour
{
    private string currentState;


    [SerializeField]
    public PlayerInputScript _inputScript;

    [SerializeField]
    public PlayerMovementScript _movementScript;

    [SerializeField]
    public PlayerCollisionScript _collisionScript;

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
        _rb = GetComponent<Rigidbody>();
        _rb.position = new Vector3(SaveManager.instance.position_x, SaveManager.instance.position_y, 0);
        Physics.gravity = _data.normalGravity;
        _data.model = GameObject.FindGameObjectWithTag("model");
        _data.savingText = GameObject.FindGameObjectsWithTag("savingText")[0]; 
        _data.pausedPanel = GameObject.FindGameObjectWithTag("pausedMenu");
        _data.pausedPanel.SetActive(false);
        _data.savingAnimation = GameObject.FindGameObjectWithTag("savingAnimation").GetComponent<Animator>();

        _data.dashUnlock = false;
        _data.jumpNumber = 0;
        _data.maxJumpNumber = 0;
        _data.blockMovement = false;
        _data.jumpTimeCounter = 5f;
        _data.isPaused = false;
        _data.canDash = true;

        _data.currentHealth = 50; // SaveManager.instance.maxHealth;
        _data.maxHealth = 50; //SaveManager.instance.maxHealth;



    }

    public void ChangeState(string newState)
    {
        if (newState != currentState)
        {
            // Change Animation
            currentState = newState;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            _data.currentHealth -= 10;
            transform.position = new Vector3(GameManager.instance.lastCheckPointPos.x, GameManager.instance.lastCheckPointPos.y, 0);
        }
    }
}
