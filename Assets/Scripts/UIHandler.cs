using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;


public class UIHandler : MonoBehaviour
{
    public Player _Player;
    public TextMeshProUGUI death_message;
    // Start is called before the first frame update
    private void Awake()
    {
        //respawn_button = FindObjectOfType<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        _Player.RespawnRPC(); 
    }
}
