using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerActions;
    void Awake()
    {
         playerActions = new PlayerInputActions();
        playerActions.Player.Enable();
    }
   
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public bool GetDash()
    {
        bool Dash = playerActions.Player.Dash.WasPressedThisFrame();
        return Dash;
    }
}
