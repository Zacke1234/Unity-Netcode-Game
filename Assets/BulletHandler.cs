using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletHandler : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player"))
        {
            Destroy(this);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
