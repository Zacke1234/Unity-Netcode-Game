using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    private GameInput gameInput;
    public event UnityAction<Vector3> MoveEvent = delegate { };
    public event UnityAction<Vector2> CameraEvent = delegate { };
    public event UnityAction ShootEvent = delegate { };
    public event UnityAction SendEvent = delegate { };

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector3>());
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        CameraEvent.Invoke(context.ReadValue<Vector2>());
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed) { ShootEvent.Invoke(); }
    }

    public void OnSend(InputAction.CallbackContext context)
    {
        if (context.performed) { SendEvent.Invoke(); }
    }

    private void OnEnable()
    {
        if (gameInput == null) 
        {
            gameInput = new GameInput();
            gameInput.Gameplay.SetCallbacks(this);
            gameInput.Gameplay.Enable();
        }
    }
}
