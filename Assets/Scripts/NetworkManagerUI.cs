using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
   // private Player _player = new Player();
    
    private void OnGUI()
    {
       
        if (GUILayout.Button("Host"))
        {
            networkManager.StartHost();
            //_player.Players = new List<GameObject>();
        }

        if (GUILayout.Button("Join"))
        { 
            // for (int p = 0; p < 10; )
            // {
            //     _player.Players = new List<GameObject>();
            // }
            
            networkManager.StartClient();
        }
        if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
