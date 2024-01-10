using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }
    public GameObject automne;
    public GameObject mysticCaves;
    private GameObject currentLevel;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != this && instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
        currentLevel = automne;
    }

    public void ChangeLevel(string newLevel, Vector3 position, Vector3 velocity, UnityEngine.Rigidbody playerRb) 
    {

        currentLevel.SetActive(false);
        SaveManager.instance.currentLevel = newLevel;
        if (newLevel == "automne") {
            currentLevel = automne;
        }
        if (newLevel == "mysticCaves") {
            currentLevel = mysticCaves;
        }
        currentLevel.SetActive(true);
        playerRb.position = position;
        playerRb.velocity = velocity;
    }


}
