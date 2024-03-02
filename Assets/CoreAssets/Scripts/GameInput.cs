using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAltAction;
    public event EventHandler OnPauseAction;

    private PlayerInput playerInput;

    public enum Binding
    {
        Move_Up,
        Move_Left,
        Move_Down,
        Move_Right,
        Interact,
        Interact_Alt,
        Pause
    }

    private void Awake( )
    {
        if(Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy( this.gameObject );
        }

        playerInput = new PlayerInput( );
        playerInput.Player.Enable( );

        playerInput.Player.Interact.performed += Interact_performed;
        playerInput.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInput.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed( UnityEngine.InputSystem.InputAction.CallbackContext obj )
    {
        OnPauseAction?.Invoke( this, EventArgs.Empty );
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

    public string GetBindingText(Binding binding)
    {
        string bindingName;

        switch( binding )
        {
            default:
            case Binding.Move_Up:
                bindingName = playerInput.Player.Move.bindings[1].ToDisplayString( ); 
                break;
            case Binding.Move_Left:
                bindingName = playerInput.Player.Move.bindings[2].ToDisplayString( );
                break;
            case Binding.Move_Down:
                bindingName = playerInput.Player.Move.bindings[3].ToDisplayString( );
                break;
            case Binding.Move_Right:
                bindingName = playerInput.Player.Move.bindings[4].ToDisplayString( );
                break;
            case Binding.Interact:
                bindingName = playerInput.Player.Interact.bindings[0].ToDisplayString( );
                break;
            case Binding.Interact_Alt:
                bindingName = playerInput.Player.InteractAlternate.bindings[0].ToDisplayString( );
                break;
            case Binding.Pause:
                bindingName = playerInput.Player.Pause.bindings[0].ToDisplayString( );
                break;
        }

        return bindingName;
    }

    public void RebindBinding( Binding binding, Action onActionRebound )
    {
        playerInput.Player.Disable( );

        InputAction inputAction;
        int bindingIndex;

        switch ( binding )
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInput.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Left:
                inputAction = playerInput.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Down:
                inputAction = playerInput.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInput.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInput.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Alt:
                inputAction = playerInput.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInput.Player.Pause;
                bindingIndex = 0;
                break;
        }
        inputAction.PerformInteractiveRebinding( bindingIndex ).OnComplete( (callback ) =>
        {
            onActionRebound?.Invoke( );
            callback.Dispose( );
            playerInput.Player.Enable( );
        } ).Start();
    }

    private void OnDestroy( )
    {
        playerInput.Player.Interact.performed -= Interact_performed;
        playerInput.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInput.Player.Pause.performed -= Pause_performed;

        playerInput.Dispose( );
    }
}
