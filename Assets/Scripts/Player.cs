using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Unity.Netcode;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Cursor = UnityEngine.Cursor;


public class Player : NetworkBehaviour
{
    private Rigidbody bulletRigidbody;
    public UIHandler uIHandler;
    //public List<GameObject> Players;

    private int amountOfPlayers;
    //public bool Paused;
    [SerializeField] private Camera camera;
    private Rigidbody rigidbody;
    private GameObject head;
    public bool TabbedOut = false;
    //private TextMeshProUGUI death_message;
    
    private int amountOfKills;
    private float amountOfDamage;
    
    [SerializeField] private float damage;
    [SerializeField] private float health;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float distance;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject bullet;
    Vector3 lookDirection;
    
    NetworkVariable<Vector3> moveInput = new NetworkVariable<Vector3>();
    NetworkVariable<Vector2> rotateInput = new NetworkVariable<Vector2>();

    private NetworkObject ob;
    //NetworkVariable<Rigidbody> networkRigidbody = new NetworkVariable<Rigidbody>();

    //private CharacterController characterController;

    // Start is called before the first frame update

    private void Awake()
    {
        
        uIHandler = GetComponent<UIHandler>(); 
        //death_message = GetComponent<TextMeshProUGUI>();
        amountOfPlayers+=1;
        //Debug.Log(this.GetComponent<Collider>());
        //characterController = GetComponent<CharacterController>();
        //camera = GetComponent<Camera>();
        rigidbody = GetComponent<Rigidbody>();

        //networkRigidbody = bullet.GetComponent<NetworkVariable<Rigidbody>>();
        head = GameObject.Find("Head");
    }

    // private void OnApplicationPause(bool pauseStatus)
    // {
    //     TabbedOut = true;
    // }
    //
    // private void OnApplicationFocus(bool hasFocus)
    // {
    //     TabbedOut = false;
    // }

    void Start()
    {
        
        if (_inputReader != null && IsLocalPlayer)
        {
            camera.enabled = true;
            OnNetworkSpawn();
            {
                Debug.Log("Object Spawned");
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _inputReader.MoveEvent += OnMove;
            _inputReader.ShootEvent += ShootRPC;
            _inputReader.CameraEvent += MoveCamera;
        }
    }
    
    private void MoveCamera(Vector2 camInput)
    {
        MoveCameraRPC(camInput);
        // if (TabbedOut == false)
        // {
        //     
        // }
    }

    private void OnMove(Vector3 input)
    {
        MoveRPC(input);
        // if (TabbedOut == false)
        // {
        //     
        // }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsServer)
        {
            KeyBinds();
            CameraMovement();
            PlayerMovement();
        }
    }
   
    
    void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        TabbedOut = false;
    }
    
    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        TabbedOut = true;
    }
    
    // private void Update()
    // {
    //     if (IsServer)
    //     {
    //         KeyBinds(); 
    //     }
    // }
    
    private void KeyBinds()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Amount of players: " + amountOfPlayers);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TabbedOut)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            
            
            
            // if (paused == true)
            // {
            //     paused = false;
            //     Cursor.visible = false;
            // }
           
            
        }
    }

    
    private void PlayerMovement()
    {
        
        //Vector3 movementDirection = new Vector3();
        //characterController.Move(moveDirection * Time.deltaTime);
        // transform.Translate(); = moveDirection * Time.deltaTime;
        //var position = transform.position;
        
        
        transform.position += moveInput.Value / 4;
        //transform.position = moveInput.Value * lookDirection.x;
        //transform.Translate(lookDirection, Space.World);
        
        //transform.Translate(moveInput.Value, Space.World);
       
    }


    private void CameraMovement()
    {
        if (IsServer)
        {
            float y = rotateInput.Value.y * Time.deltaTime;
            float x = rotateInput.Value.x * Time.deltaTime;
            //y -= x;
            //Debug.Log("Y: " + y);
            float xHeadTransform = head.transform.rotation.x;
            Debug.Log(xHeadTransform);
            
            lookDirection = Vector3.forward;
            lookDirection = camera.transform.TransformDirection(lookDirection);
            rigidbody.transform.localRotation *= quaternion.Euler(0,x,0);
            head.transform.localRotation *= quaternion.Euler(-y , 0, 0);
            Mathf.Clamp(y, -0.45f, 0.45f); // does not work??
            
        }
       
        
    }

    

    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector3 data)
    {
        moveInput.Value = data;
    }

    [Rpc(SendTo.Server)]
    private void MoveCameraRPC(Vector2 data)
    {
        rotateInput.Value = data;
        //Debug.Log(data);
    }

    [Rpc(SendTo.Server)]
    private void ShootRPC()
    {
        if (IsServer)
        {
            ob = (NetworkObject)Instantiate(bullet, spawnPoint.position, spawnPoint.rotation)
                .GetComponent<NetworkObject>();
            Physics.IgnoreCollision(ob.GetComponent<Collider>(), this.GetComponent<Collider>());

        
            bulletRigidbody = ob.GetComponent<Rigidbody>();
            ob.Spawn();

            bulletRigidbody.AddForce(lookDirection * 33, ForceMode.Impulse);
        }
        
        
       
        // if (TabbedOut == false)
        // {
        //     
        // }
        
        // IEnumerator PhysicsLate (Rigidbody rb){
        //     yield return new WaitForSeconds(0.01f);
        //     rb.AddForce(Vector3.forward * 100, ForceMode.Impulse);

        // if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hit))
        // {
        //     
        //     if (bulletRigidbody == bullet.GetComponent<Rigidbody>())
        //     {
        //         Debug.Log("Got the rigidbody");
        //     }
        //    
        //    
        //     if (hit.transform.tag == "Player")
        //     {
        //         //DamageRPC();
        //     }
        // }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (IsServer)
        {
            if (other.collider.CompareTag("Bullet"))
            {
                Debug.Log("Did something");
                DamageRPC();
            } 
        }
        
    }

    [Rpc(SendTo.Server)]
    private void DamageRPC()
    {
        if (IsServer)
        {
            health -= damage;
            Debug.Log("Health: " + health);
            if (health <= 0)
            {
                DeathRPC();
            }
        }
        
    }
    [Rpc(SendTo.Server)]
    private void DeathRPC()
    {
        if (IsServer)
        {
            uIHandler.death_message.enabled = true;
            this.enabled = false;
            Debug.Log("death");
        }
       
        
    }
    [Rpc(SendTo.Server)]
    public void RespawnRPC()
    {
        if (IsServer)
        {
            uIHandler.death_message.enabled = false;
            uIHandler.death_message.text = "You died!\nKills: " + amountOfKills +" \nDamage dealt: " + amountOfDamage;
            this.enabled = true;
            this.transform.position = transform.position + new Vector3(0, 10, 0);
        }
        
    }
    
}