using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    public GameObject PausedPanel;
    public GameObject playerObj;
    public PlayerManagerScript _player;

    void Start()
    {
        _player = playerObj.GetComponent<PlayerManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _player._data.isPaused = false;
            Time.timeScale = 1;
            PausedPanel.SetActive(false);
        }
    }

    public void Continue() {
        _player._data.isPaused = false;
        PausedPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Quit() {
        Application.Quit();
    }

}
