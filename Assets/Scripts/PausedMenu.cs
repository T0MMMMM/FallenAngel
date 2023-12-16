using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    public GameObject PausedPanel;
    public GameObject playerObj;
    private PlayerMovement player;

    void Start()
    {
        player = playerObj.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            player.isPaused = false;
            PausedPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Continue() {
        player.isPaused = false;
        PausedPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Quit() {
        Application.Quit();
    }

}
