using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    public GameObject PausedPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PausedPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Continue() {
        PausedPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Quit() {
        Application.Quit();
    }

}
