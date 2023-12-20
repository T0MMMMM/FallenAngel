using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionAutomne : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            LevelManager.instance.ChangeLevel("automne", new Vector3(105f, 1.5f, 0f), new Vector3(0f, 0f, 0f), other.attachedRigidbody);
        }
    }
}
