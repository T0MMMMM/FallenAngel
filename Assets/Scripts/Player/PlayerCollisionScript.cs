using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    [SerializeField]
    public PlayerManagerScript _player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Double Jump Power" && other.transform.GetChild(1).gameObject != null)
        {
            other.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
           _player._data.maxJumpNumber += 1;
            Destroy(other.transform.GetChild(1).gameObject);
        }
        if (other.name == "Dash Power" && other.transform.GetChild(1).gameObject != null)
        {
            other.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            _player._data.dashUnlock = true;
            Destroy(other.transform.GetChild(1).gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
    }

    public void hit(int damage)
    {
        _player._data.currentHealth -= damage;
        if (_player._data.currentHealth <= 0)
        {
            _player._data.currentHealth = 50; //_player.starting();
            _player._rb.position = new Vector3(SaveManager.instance.position_x, SaveManager.instance.position_y, 0);
        } else
        {
            _player.transform.position = new Vector3(GameManager.instance.lastCheckPointPos.x, GameManager.instance.lastCheckPointPos.y, 0);
        }
    }

}
