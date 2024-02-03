using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAltAction;
    private PlayerInput playerInput;
    private void Awake( )
    {
        playerInput = new PlayerInput( );
        playerInput.Player.Enable( );

        playerInput.Player.Interact.performed += Interact_performed;
        playerInput.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void Interact_performed( UnityEngine.InputSystem.InputAction.CallbackContext obj )
    {
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }

    private void InteractAlternate_performed( UnityEngine.InputSystem.InputAction.CallbackContext obj )
    {
        OnInteractAltAction?.Invoke( this, EventArgs.Empty );
    }

    public Vector2 GetInputVectorNormalized( )
    {
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>( );

        inputVector = inputVector.normalized;

        return inputVector;
    } 

    public Vector2 GetInputLegacy( )
    {
        Vector2 inputVector = Vector2.zero;

        if ( Input.GetKey( KeyCode.W ) )
            inputVector.y = 1;
        if ( Input.GetKey( KeyCode.S ) )
            inputVector.y = -1;
        if ( Input.GetKey( KeyCode.D ) )
            inputVector.x = 1;
        if ( Input.GetKey( KeyCode.A ) )
            inputVector.x = -1;

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
