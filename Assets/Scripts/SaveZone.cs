using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveZone : MonoBehaviour
{
    public GameObject savingText;
    public Animator savingAnimation;
    private PlayerManagerScript _player;

    private bool canSave = false;
    private float savingTimer = 5f;
    private bool pressSave = false;
    private bool holdSave = false;
    private bool relasedSave = false;

    public void Start()
    {
        _player = PlayerManagerScript.instance;
    }

    public void Update()
    {

        pressSave = Input.GetKeyDown(KeyCode.Q);
        holdSave = Input.GetKey(KeyCode.Q);
        relasedSave = Input.GetKeyUp(KeyCode.Q);

        if (canSave)
        {
            handleSave();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            canSave = true;
            savingText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            canSave = false;
            savingAnimation.SetBool("isSaving", false);
            savingText.SetActive(false);
        }
    }

    private void handleSave()
    {
        SaveManager.instance.position_x = _player._rb.position.x;
        SaveManager.instance.position_y = _player._rb.position.y;
        SaveManager.instance.maxHealth = _player._data.maxHealth;

        if (pressSave && !savingAnimation.GetBool("isSaving"))
        {
            savingAnimation.SetBool("isSaving", true);
        }
        if (holdSave || (savingTimer < 2 && savingTimer > 0))
        {
            savingTimer -= Time.deltaTime;
        }
        if (savingTimer < 2)
        {
            SaveManager.instance.Save();
        }
        if (savingTimer <= 0)
        {
            savingTimer = 5;
            savingAnimation.SetBool("isSaving", false);
            canSave = false;
        }

        if (relasedSave && !(savingTimer < 2))
        {
            savingAnimation.SetBool("isSaving", false);
            savingTimer = 5;
        }
    }


}
