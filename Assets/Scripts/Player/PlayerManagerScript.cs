using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    #region player

    [System.NonSerialized]
    public Rigidbody _rb;

    #endregion


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.position = new Vector3(SaveManager.instance.position_x, SaveManager.instance.position_y, 0);
        Physics.gravity = _data.normalGravity;
        _data.model = GameObject.FindGameObjectsWithTag("model")[0];
        _data.savingText = GameObject.FindGameObjectsWithTag("savingText")[0]; 
        _data.pausedPanel = GameObject.Find("pausedMenu");
        _data.savingAnimation = GameObject.FindGameObjectsWithTag("savingAnimation")[0].GetComponent<Animator>();

        _data.dashUnlock = false;
        _data.jumpNumber = 0;
        _data.maxJumpNumber = 0;
        _data.blockMovement = false;
        _data.jumpTimeCounter = 5f;

}

    public void ChangeState(string newState)
    {
        if (newState != currentState)
        {
            // Change Animation
            currentState = newState;
        }
    }
}
