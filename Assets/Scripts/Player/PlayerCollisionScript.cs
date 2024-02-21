using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    [SerializeField]
    public PlayerManagerScript _player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Double Jump Power")
        {
           _player._data.maxJumpNumber += 1;
            Destroy(other.gameObject);
        }
        if (other.name == "Dash Power")
        {
            _player._data.dashUnlock = true;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Respawn"))
        {
            _player._data.canSave = true;
            _player._data.savingText.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            _player._data.canSave = false;
            _player._data.savingText.SetActive(false);
        }
    }
}
