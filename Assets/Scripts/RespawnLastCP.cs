using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLastCP : MonoBehaviour
{
    [SerializeField]
    private PlayerManagerScript _player;


    void Start()
    {

    }

    void Update()
    {
        _player.ChangeState("playing");
    }

    public void enterScript()
    {
        _player._data.currentHealth -= 10;
        _player.transform.position = new Vector3(GameManager.instance.lastCheckPointPos.x, GameManager.instance.lastCheckPointPos.y, 0);
    }
}
