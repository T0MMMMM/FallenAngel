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
        anim.SetBool("fadeIN", true);
        yield return new WaitUntil(() => black.color.a == 1);
        _player._data.currentHealth -= 10;
        _player.transform.position = new Vector3(GameManager.instance.lastCheckPointPos.x, GameManager.instance.lastCheckPointPos.y, 0);
        anim.SetBool("fade", true);
        
        yield return new WaitUntil(() => black.color.a == 0);

        anim.SetBool("fadeIN", false);
        anim.SetBool("fade", false);
        _player.ChangeState("playing");

    }




}
