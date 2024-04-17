using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private PlayerManagerScript _player;

    [SerializeField]
    private GameObject playerObject;


    private Image image;

    [SerializeField]
    public Sprite[] sprites;

    void Start()
    {
        _player = playerObject.GetComponent<PlayerManagerScript>();
        image = GetComponent<Image>();
    }

    void Update()
    {   
        image.sprite = sprites[5 - (int) _player._data.currentHealth/10];
        //Debug.Log(_player._data.currentHealth);
    }

}
