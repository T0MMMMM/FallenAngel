using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionMysticCaves : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            LevelManager.instance.ChangeLevel("mysticCaves", new Vector3(0f, 1.5f, 0f), new Vector3(0f, 0f, 0f), other.attachedRigidbody);
        }
    }
}
