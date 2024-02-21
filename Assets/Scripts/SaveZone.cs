using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveZone : MonoBehaviour
{
    public GameObject savingText;
    public GameObject _player;
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            _player.GetComponent<PlayerManagerScript>()._data.canSave = true;
            savingText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            _player.GetComponent<PlayerManagerScript>()._data.canSave = false;
            savingText.SetActive(false);
        }
    }
}
