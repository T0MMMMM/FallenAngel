using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    [SerializeField]
    public PlayerManagerScript _player;

    public void Update()
    {   
        if (_player._data.isPaused) { return; }
        if (Input.GetKeyDown(KeyCode.Escape) && !_player._data.isPaused)
        {
            _player._data.isPaused = true;
            _player._data.pausedPanel.SetActive(true);
            Time.timeScale = 0;
        }

        _player._data.pressSave = Input.GetKeyDown(KeyCode.Q);
        _player._data.holdSave = Input.GetKey(KeyCode.Q);
        _player._data.relasedSave = Input.GetKeyUp(KeyCode.Q);

        _player._data.pressJump = Input.GetKeyDown(KeyCode.Space);
        _player._data.holdJump = Input.GetKey(KeyCode.Space);
        _player._data.relasedJump = Input.GetKeyUp(KeyCode.Space);

        _player._data.pressWallJump = Input.GetKeyDown(KeyCode.Space) && _player._data.isOnWall && !_player._data.isGrounded;

        _player._data.simpleJump = _player._data.pressJump && (_player._data.isGrounded || (_player._data.jumpNumber >= 1 && !_player._data.isOnWall));


        _player._data.pressDash = _player._data.canDash && Input.GetButtonDown("Dash");

        _player._data.blockMovement = (Input.GetKeyDown(KeyCode.Space) && _player._data.isOnWall || (_player._data.timeAfterJump < 0.4 && _player._data.timeAfterJump != 0 && _player._data.isWallJumping)) ;

        _player._data.horizontalInput = Input.GetAxisRaw("Horizontal");
        _player._data.verticalInput = Input.GetAxisRaw("Vertical");




    }
}
