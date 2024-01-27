using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotSpeed = 15f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayer;

    private Vector3 moveDir;
    private bool isWalking = false;
    private ClearCounter selectedCounter = null;

    private void Awake( )
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy( gameObject );
        }
    }
    private void Start( )
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction( object sender, EventArgs e )
    {
        this.selectedCounter?.Interact( );
    }

    private void Update( )
    {
        HandleMovement( );
        HandleInteractions( );
    }

    private void HandleInteractions( )
    {
        float interactDistance = 1.5f;
        float bodyCentreOffset = 0.65f;
        Vector3 bodyCentre = transform.position + ( transform.up * bodyCentreOffset );
        if ( Physics.Raycast( bodyCentre, transform.forward, out RaycastHit hitInfo, interactDistance, counterLayer ) )
        {
            if ( hitInfo.transform.TryGetComponent<ClearCounter>( out ClearCounter clearCounter ) )
            {
                if ( this.selectedCounter != clearCounter )
                {
                    SelectedCounterChanged( clearCounter );
                }
            }
            else
            {
                SelectedCounterChanged( null );
            }
        }
        else
        {
            SelectedCounterChanged( null );
        }

    }

    /// <summary>
    /// Handles PlayerMovement and Rotation along with collision detection
    /// </summary>
    private void HandleMovement( )
    {
        Vector2 inputVector = gameInput.GetInputVectorNormalized( );

        moveDir = new Vector3( inputVector.x, 0f, inputVector.y );

        isWalking = moveDir != Vector3.zero;

        //Rotation of player before attempting to move
        if ( moveDir.x != 0f || moveDir.z != 0f )
            transform.forward = Vector3.Slerp( transform.forward, moveDir, Time.deltaTime * rotSpeed );

        //Movement and Collision Detection
        float playerRadius = 0.4f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast( transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, playerRadius );

        if ( !canMove )
        {
            //Attempt X direction movement alone
            moveDir = new Vector3( inputVector.x, 0f, 0f ).normalized;
            canMove = !Physics.CapsuleCast( transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, playerRadius );

            if ( !canMove )
            {
                //Now attempt Z direction movement alone
                moveDir = new Vector3( 0f, 0f, inputVector.y ).normalized;
                canMove = !Physics.CapsuleCast( transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, playerRadius );
                if ( !canMove )
                {
                    moveDir = Vector3.zero;
                }
            }
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void SelectedCounterChanged( ClearCounter clearCounter )
    {
        this.selectedCounter = clearCounter;

        OnSelectedCounterChanged?.Invoke( this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter } );
    }

    public bool IsWalking( )
    {
        return isWalking;
    }
}
