using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private PlayerManagerScript _player;


    // Start is called before the first frame update
    void Start()
    {
        _player._sound.Play("Forest");

    }

    // Update is called once per frame
    void Update()
    {
        if (_player._data.isPaused)
        {
            return;
        }

        CheckColliders();

        handleMovement();

        handleJump();

        HandleDash();


        if (_player._data.canSave)
        {
            handleSave();
        }
    }


    void CheckColliders()
    {

        #region Check Ground Collision

        _player._data.isGrounded = Physics.BoxCast(transform.position, _player._data.boxSizeGround, -transform.up, transform.rotation, _player._data.maxDistance, _player._data.layerMask);

        #endregion


        #region Check Wall Collision

        _player._data.isOnWallLeft = Physics.BoxCast(transform.position, _player._data.boxSizeWall, -transform.right, transform.rotation, _player._data.spacing, _player._data.layerMask);
        _player._data.isOnWallRight = Physics.BoxCast(transform.position, _player._data.boxSizeWall, transform.right, transform.rotation, _player._data.spacing, _player._data.layerMask);
        _player._data.isOnWall = _player._data.isOnWallLeft || _player._data.isOnWallRight;

        if (_player._data.isOnWall && _player._rb.velocity.y < 0)
        {
            _player._rb.velocity = new Vector3(_player._rb.velocity.x, -3, _player._rb.velocity.z);
        }

        #endregion 
    }

    #region Movement 

    void handleMovement()
    {

        if (_player._data.isGrounded)
        {
            _player._data.jumpNumber = _player._data.maxJumpNumber;
        }


        if (!_player._data.blockMovement && (!_player._data.isWallJumping || _player._data.horizontalInput != 0))
        {
            _player._rb.velocity = new Vector3(_player._data.horizontalInput * _player._data.movementSpeed, _player._rb.velocity.y, 0f);
            if ((_player._data.horizontalInput < 0 || _player._data.horizontalInput > 0) && _player._data.isWallJumping)
            {
                _player._data.wallJumpingCounter = 0f;
                //_player._rb.velocity = (0f, _player._rb.velocity.y, 0f); ptete un jour, ça servira ^^
                _player._data.isWallJumping = false;
            }
        }


        if ((_player._data.horizontalInput > 0))
        {
            _player._data.direction = 1;
            _player._data.model.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if ((_player._data.horizontalInput < 0))
        {
            _player._data.direction = -1;
            _player._data.model.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (!_player._data.dashing)
        {
            if (_player._data.horizontalInput == 0 && _player._data.verticalInput == 0)
            {
                _player._data.dashDirection = new Vector2(_player._data.direction, 0);
            }
            else
            {
                _player._data.dashDirection = new Vector2(_player._data.horizontalInput, _player._data.verticalInput);
            }
        }

    }

    void handleJump()
    {

        #region Jump Simple

        if (_player._data.simpleJump)
        {
            _player._rb.velocity = new Vector3(_player._rb.velocity.x, _player._data.jumpForce, 0f);
            _player._data.isJumping = true;
            _player._data.jumpTimeCounter = _player._data.jumpTime;
            _player._data.jumpNumber -= 1;
        }

        if (_player._data.relasedJump)
        {
            _player._data.isJumping = false;
        }

        #endregion

        #region Long Jump

        if (_player._data.holdJump && _player._data.isJumping)
        {
            if (_player._data.jumpTimeCounter > 0)
            {
                _player._rb.velocity = new Vector3(_player._rb.velocity.x, _player._data.jumpForce, 0f);
                _player._data.jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _player._data.isJumping = false;
            }
        }

        #endregion

        #region Wall Jump

        if (_player._data.pressWallJump)
        {
            _player._data.isWallJumping = true;
            if (_player._data.isOnWallLeft && _player._data.direction == -1)
            {
                _player._data.direction = -_player._data.direction;
                _player._data.model.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (_player._data.isOnWallRight && _player._data.direction == 1)
            {
                _player._data.direction = -_player._data.direction;
                _player._data.model.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            _player._rb.velocity = new Vector3(_player._data.direction * _player._data.wallJumpingPower.x, _player._data.wallJumpingPower.y, 0);
        }

        if (_player._data.isWallJumping) { _player._data.timeAfterJump += Time.deltaTime; }

        if (_player._data.isOnWall && !Input.GetKey(KeyCode.Space)) { _player._data.timeAfterJump = 0; }

        if (_player._data.isGrounded)
        {
            _player._data.timeAfterJump = 0;
            _player._data.isWallJumping = false;
        }

        #endregion
    }


    #endregion



    void OnDrawGizmos()
    {
        if (_player._data.isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawCube(transform.position - transform.up * _player._data.maxDistance, _player._data.boxSizeGround);
        Gizmos.DrawCube(transform.position - transform.right * _player._data.spacing, _player._data.boxSizeWall * 2);
        Gizmos.DrawCube(transform.position + transform.right * _player._data.spacing, _player._data.boxSizeWall * 2);

    }

    private void handleSave()
    {

        SaveManager.instance.position_x = _player._rb.position.x;
        SaveManager.instance.position_y = _player._rb.position.y;



        if (_player._data.pressSave && !_player._data.savingAnimation.GetBool("isSaving"))
        {
            _player._data.savingAnimation.SetBool("isSaving", true);
        }
        if (_player._data.holdSave || (_player._data.savingTimer < 2 && _player._data.savingTimer > 0))
        {
            _player._data.savingTimer -= Time.deltaTime;
        }
        if (_player._data.savingTimer < 2)
        {
            SaveManager.instance.Save();
        }
        if (_player._data.savingTimer <= 0)
        {
            _player._data.savingTimer = 5;
            _player._data.canSave = false;
            _player._data.savingAnimation.SetBool("isSaving", false);
        }

        if (_player._data.relasedSave && !(_player._data.savingTimer < 2))
        {
            _player._data.savingAnimation.SetBool("isSaving", false);
            _player._data.savingTimer = 5;
        }
    }


    private IEnumerator Dash()
    {
        _player._data.dashing = true;
        float startTime = Time.time;
        _player._data.wallJumpingCounter = 0f;
        _player._data.isWallJumping = false;
        _player._rb.velocity = new Vector3(0f, 0f, 0f);
        _player._data.timeAfterJump = 0;

        while (Time.time < startTime + _player._data.dashTime && _player._data.canDash)
        {
            _player._rb.velocity = new Vector3(_player._data.dashDirection.x * _player._data.dashPower.x, _player._data.dashDirection.y * _player._data.dashPower.y, 0f);

            yield return null;
        }
        _player._rb.velocity = new Vector3(_player._rb.velocity.x, _player._rb.velocity.y / 2, 0f);
        _player._data.dashDirection = new Vector2(0f, 0f);
        _player._data.dashing = false;
        _player._data.canDash = false;
        StartCoroutine(DashColdown());

    }

    private IEnumerator DashColdown()
    {
        while (_player._data.dashColdown > 0)
        {
            _player._data.dashColdown -= Time.deltaTime;
            yield return null;


        }
        _player._data.endDashColdown = true;
    }

    void HandleDash()
    {
        if (_player._data.pressDash  && _player._data.dashUnlock && _player._data.canDash)
        {
            StartCoroutine(Dash());
        }
        if (_player._data.endDashColdown && _player._data.isGrounded)
        {
            _player._data.canDash = true;
            _player._data.dashColdown = 2f;
            _player._data.endDashColdown = false;

        }
    }

    #region usefull fonction 

    void StopWallJumping() { _player._data.isWallJumping = false; }


    #endregion

}
