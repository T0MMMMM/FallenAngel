using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Power : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(90f, Time.time * 100f, 0);
    }
    
}
