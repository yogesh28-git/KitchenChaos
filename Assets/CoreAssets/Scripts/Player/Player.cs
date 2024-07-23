using System;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    //public static Player Instance { get; private set; }

    public static event EventHandler OnPickUpSomething;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotSpeed = 15f;
    [SerializeField] private LayerMask counterLayer;
    [SerializeField] private Transform kitchenObjectHoldTransform;

    private Vector3 moveDir;
    private bool isWalking = false;
    private BaseCounter selectedCounter = null;
    private KitchenObject kitchenObject;

    private void Awake( )
    {
        /*if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy( gameObject );
        }*/

    }
    private void Start( )
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAltAction += GameInput_OnInteractAltAction;
    }

    private void GameInput_OnInteractAction( object sender, EventArgs e )
    {
        if ( KitchenGameManager.Instance.isGamePlaying( ) )
            this.selectedCounter?.Interact( this );
    }

    private void GameInput_OnInteractAltAction( object sender, EventArgs e )
    {
        if ( KitchenGameManager.Instance.isGamePlaying( ) )
            this.selectedCounter?.InteractAlt( );
    }

    private void Update( )
    {
        if ( !IsOwner )
            return;

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
            if ( hitInfo.transform.TryGetComponent<BaseCounter>( out BaseCounter baseCounter ) )
            {
                if ( this.selectedCounter != baseCounter )
                {
                    SelectedCounterChanged( baseCounter );
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
        Vector2 inputVector = GameInput.Instance.GetInputVectorNormalized( );

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

    private void SelectedCounterChanged( BaseCounter baseCounter )
    {
        this.selectedCounter = baseCounter;

        OnSelectedCounterChanged?.Invoke( this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter } );
    }

    public bool IsWalking( )
    {
        return isWalking;
    }

    private void OnDestroy( )
    {
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAltAction -= GameInput_OnInteractAltAction;
    }

    public static void ResetStaticData( )
    {
        OnPickUpSomething = null;
    }

    #region Interface Methods
    public bool HasKitchenObject( )
    {
        return this.kitchenObject != null;
    }

    public KitchenObject GetKitchenObject( )
    {
        return this.kitchenObject;
    }
    public void SetKitchenObject( KitchenObject _kitchenObject )
    {
        if ( kitchenObject == null )
        {
            this.kitchenObject = _kitchenObject;
            OnPickUpSomething?.Invoke( this, EventArgs.Empty );
        }
    }

    public void ClearKitchenObject( )
    {
        this.kitchenObject = null;
    }

    public Transform GetKitchenObjectFollowTransform( )
    {
        return kitchenObjectHoldTransform;
    }

    #endregion
}
