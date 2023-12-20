using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveZone : MonoBehaviour
{
    public GameObject savingText;
    public GameObject player;
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerMovement>().canSave = true;
            savingText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerMovement>().canSave = false;
            savingText.SetActive(false);
        }
    }
}
