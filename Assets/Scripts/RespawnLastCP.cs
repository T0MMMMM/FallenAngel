using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RespawnLastCP : MonoBehaviour
{
    [SerializeField]
    private PlayerManagerScript _player;

    public Image black;
    public Animator anim;


    void OnEnable()
    {
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        anim.SetBool("startfade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        _player._data.currentHealth -= 10;
        _player.transform.position = new Vector3(GameManager.instance.lastCheckPointPos.x, GameManager.instance.lastCheckPointPos.y, 0);
        _player._rb.velocity = new Vector3(0, 0, 0);
        anim.SetBool("endfade", true);

        yield return new WaitUntil(() => black.color.a == 0);

        anim.SetBool("startfade", false);
        anim.SetBool("endfade", false);
        _player.ChangeState("playing");

    }

}